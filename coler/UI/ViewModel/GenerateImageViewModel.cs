using coler.BusinessLogic.Manager;
using coler.Globals;
using coler.Model.ColorGen.Interface;
using coler.Model.Enum;
using coler.Model.GenImage;
using coler.Model.Parameter;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using MyToolkit.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using PixelData = coler.Model.PixelData;

namespace coler.UI.ViewModel
{
    public class GenerateImageViewModel : ViewModelBase
    {
        #region Fields

        private ColorGenManager _colorGenManager;
        private GenImageManager _genImageManager;

        private int _seed;

        public enum EnPointUpdateType
        {
            Refresh,
            Draw
        };

        #region Backing Fields

        private IColorGen _selectedColorGen;
        private ObservableDictionary<int, ParameterBase> _selectedParameters;

        private PixelData[][] _points;

        private int _width;
        private int _height;
        private EnGenType _genType;
        private EnGenType[] _allGenTypes;

        private bool _randomizeParameters;
        private int _cellsLoaded;
        private bool _loadingCells;
        private int _cellsToLoad;
        private GenImage _imageSource;
        private BitmapScalingMode _selectedScalingMode = BitmapScalingMode.Fant;
        private List<BitmapScalingMode> _scalingModes;
        private bool _useTemplate;
        private bool _invertTemplate;
        private string _templatePath;
        private Color _selectedColor = Color.FromRgb(255, 255, 255);
        private int _maskWidth;
        private int _maskHeight;
        private int _originalMaskWidth;
        private int _originalMaskHeight;
        private int _chunkWidth;
        private int _chunkHeight;

        #endregion Backing Fields

        #endregion Fields

        #region Properties

        public ObservableDictionary<int, ParameterBase> SelectedParameters
        {
            get => _selectedParameters;
            set
            {
                _selectedParameters = value;
                RaisePropertyChanged();
            }
        }

        public IColorGen SelectedColorGen
        {
            get => _selectedColorGen;
            set
            {
                _selectedColorGen = value;
                RaisePropertyChanged();
            }
        }

        public PixelData[][] Points
        {
            get => _points;
            set
            {
                if (Equals(value, _points)) return;
                _points = value;
                _colorGenManager.Points = _points;
                RaisePropertyChanged();
            }
        }

        public BitmapScalingMode SelectedScalingMode
        {
            get => _selectedScalingMode;
            set
            {
                if (value == _selectedScalingMode) return;
                _selectedScalingMode = value;
                RaisePropertyChanged();
            }
        }

        public List<BitmapScalingMode> ScalingModes
        {
            get => _scalingModes;
            set
            {
                if (Equals(value, _scalingModes)) return;
                _scalingModes = value;
                RaisePropertyChanged();
            }
        }

        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (value.Equals(_selectedColor)) return;
                _selectedColor = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(BackgroundColor));
            }
        }

        public GenImage CurrentImage
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                RaisePropertyChanged();
            }
        }

        public string TemplatePath
        {
            get => _templatePath;
            set
            {
                if (value == _templatePath) return;
                _templatePath = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TemplateName));
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;

                _colorGenManager.CanvasWidth = _width;

                RaisePropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;

                _colorGenManager.CanvasHeight = _height;

                RaisePropertyChanged();
            }
        }

        public int MaskWidth
        {
            get => _maskWidth;
            set
            {
                if (value == _maskWidth) return;
                _maskWidth = value;

                double scale = (double)_maskWidth / OriginalMaskWidth;

                _maskHeight = (int)(OriginalMaskHeight * scale);

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(MaskHeight));
            }
        }

        public int MaskHeight
        {
            get => _maskHeight;
            set
            {
                if (value == _maskHeight) return;
                _maskHeight = value;

                double scale = (double)_maskHeight / OriginalMaskHeight;

                _maskWidth = (int)(OriginalMaskWidth * scale);

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(MaskWidth));
            }
        }

        public int OriginalMaskWidth
        {
            get => _originalMaskWidth;
            set
            {
                if (value == _originalMaskWidth) return;
                _originalMaskWidth = value;
                _maskWidth = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(MaskWidth));
            }
        }

        public int OriginalMaskHeight
        {
            get => _originalMaskHeight;
            set
            {
                if (value == _originalMaskHeight) return;
                _originalMaskHeight = value;
                _maskHeight = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(MaskHeight));
            }
        }

        public int ChunkWidth
        {
            get => _chunkWidth;
            set
            {
                if (value == _chunkWidth) return;
                _chunkWidth = value;

                RaisePropertyChanged();
            }
        }

        public int ChunkHeight
        {
            get => _chunkHeight;
            set
            {
                if (value == _chunkHeight) return;
                _chunkHeight = value;

                RaisePropertyChanged();
            }
        }

        public EnGenType GenType
        {
            get => _genType;
            set
            {
                if (value == _genType) return;
                _genType = value;
                RaisePropertyChanged();

                SetSelectedColorGen();
            }
        }

        public EnGenType[] AllGenTypes
        {
            get => _allGenTypes;
            set
            {
                if (value == _allGenTypes) return;
                _allGenTypes = value;
                RaisePropertyChanged();
            }
        }

        private void SetSelectedColorGen()
        {
            SelectedColorGen = _colorGenManager.GetColorGen(GenType);
            SelectedParameters = SelectedColorGen?.GetParameters() ?? new ObservableDictionary<int, ParameterBase>();
        }

        public int CellsLoaded
        {
            get => _cellsLoaded;
            set
            {
                if (value == _cellsLoaded) return;
                _cellsLoaded = value;
                RaisePropertyChanged();
            }
        }

        public int CellsToLoad
        {
            get => _cellsToLoad;
            set
            {
                if (value == _cellsToLoad) return;
                _cellsToLoad = value;
                RaisePropertyChanged();
            }
        }

        public bool RandomizeParameters
        {
            get => _randomizeParameters;
            set
            {
                if (value == _randomizeParameters) return;
                _randomizeParameters = value;
                RaisePropertyChanged();
            }
        }

        public bool LoadingCells
        {
            get => _loadingCells;
            set
            {
                if (value == _loadingCells) return;
                _loadingCells = value;
                RaisePropertyChanged();
            }
        }

        public bool UseTemplate
        {
            get => _useTemplate;
            set
            {
                if (value == _useTemplate) return;
                _useTemplate = value;
                RaisePropertyChanged();
            }
        }

        public bool InvertTemplate
        {
            get => _invertTemplate;
            set
            {
                if (value == _invertTemplate) return;
                _invertTemplate = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Getter Properties

        public string TemplateName => string.IsNullOrWhiteSpace(TemplatePath) ? "" : TemplatePath.Split('\\').Last();

        public System.Drawing.Color BackgroundColor => System.Drawing.Color.FromArgb(SelectedColor.R, SelectedColor.G, SelectedColor.B);

        #endregion Getter Properties

        #region OpenFileDialog properties

        public string Filter { get; set; }
        public string Title { get; set; } = "Select Template";

        public bool DialogResult { get; protected set; }

        #endregion OpenFileDialog properties

        #region Command Properties

        public ICommand RefreshGridCommand { get; private set; }
        public ICommand RedrawGridCommand { get; private set; }
        public ICommand GenerateImageCommand { get; private set; }
        public ICommand SelectTemplateCommand { get; private set; }

        #endregion Command Properties

        public GenerateImageViewModel()
        {
            Initialize();
        }

        #region Initialization

        private void Initialize()
        {
            _colorGenManager = ColorGenManager.Instance;
            _genImageManager = GenImageManager.Instance;

            SelectedParameters = new ObservableDictionary<int, ParameterBase>();

            ScalingModes = new List<BitmapScalingMode>
            {
                BitmapScalingMode.NearestNeighbor,
                BitmapScalingMode.Linear,
                BitmapScalingMode.Fant
            };
            AllGenTypes = (EnGenType[])Enum.GetValues(typeof(EnGenType));

            _width = _colorGenManager.CanvasWidth;
            _height = _colorGenManager.CanvasHeight;
            _chunkHeight = 100;
            _chunkWidth = 100;

            InitializeCommands();
            InitializePoints();

            RaisePropertyChanged(nameof(Width));
            RaisePropertyChanged(nameof(Height));
        }

        private void InitializeCommands()
        {
            RefreshGridCommand = new RelayCommand(RefreshGrid);
            RedrawGridCommand = new RelayCommand(RedrawGrid);
            GenerateImageCommand = new RelayCommand(GenerateImage);
            SelectTemplateCommand = new RelayCommand(SelectTemplate);
        }

        private void InitializePoints()
        {
            RefreshGrid();
        }

        #endregion Initialization

        #region Commands

        public void RefreshGrid()
        {
            UpdateGrid(EnPointUpdateType.Refresh);
        }

        public void RedrawGrid()
        {
            Random rand = new Random();

            _seed = rand.Next(0, 1000000);

            UpdateGrid(EnPointUpdateType.Draw);
        }

        public void GenerateImage()
        {
            GenImage genImage = new GenImage(DateTime.Now);

            using (Bitmap image = new Bitmap(Width, Height))
            {
                Bitmap mask = null;

                try
                {
                    mask = CreateMask();
                }
                catch (Exception ex)
                {
                    // ignored
                }

                UseTemplate = UseTemplate && mask != null;

                /*PixelData[][][] chunks = GetChunks(ChunkWidth, ChunkHeight, Points);

                Parallel.ForEach(chunks, chunk =>
                {
                    foreach (PixelData[] column in chunk)
                    {
                        foreach (PixelData point in column)
                        {
                            SetPixelColor(image, mask, point);
                        }
                    }
                });*/

                foreach (PixelData[] column in Points)
                {
                    foreach (PixelData point in column)
                    {
                        SetPixelColor(image, mask, point);
                    }
                }

                Bitmap finalImage = Utils.Transparent2Color(image, BackgroundColor);

                finalImage.Save(genImage.SourceFilePath, ImageFormat.Png);

                Utils.ResizeImage(finalImage, 0.5).Save(genImage.ThumbnailFilePath, ImageFormat.Png);
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                _genImageManager.AddImage(genImage);
            });

            CurrentImage = genImage;
        }

        public void SelectTemplate()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Template Images(*.png; *.jpg)|*.png; *.jpg",
                InitialDirectory = Constants.TemplateDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                TemplatePath = openFileDialog.FileName;
            }

            try
            {
                Bitmap template = new Bitmap(TemplatePath);
                OriginalMaskWidth = template.Width;
                OriginalMaskHeight = template.Height;
            }
            catch (Exception ex)
            {
                //ignored
            }
        }

        #endregion Commands

        #region Private Methods

        private static T[][][] GetChunks<T>(int chunkWidth, int chunkHeight, IReadOnlyList<T[]> collection)
        {
            if (chunkWidth == 0 ||
                chunkHeight == 0) return new T[0][][];

            int collectionWidth = collection.Count;
            int collectionHeight = collection.FirstOrDefault()?.Length ?? 0;

            if (collectionWidth == 0 ||
                collectionHeight == 0) return new T[0][][];

            List<T[][]> chunkList = new List<T[][]>();

            int yCount = collectionHeight;
            int lastChunkEndY = 0;

            while (yCount > 0)
            {
                int xCount = collectionWidth;
                int lastChunkEndX = 0;

                int currentChunkHeight = Math.Min(chunkHeight, yCount);
                yCount -= currentChunkHeight;

                while (xCount > 0)
                {
                    int currentChunkWidth = Math.Min(chunkWidth, xCount);
                    xCount -= currentChunkWidth;

                    T[][] xx = new T[currentChunkWidth][];

                    for (int i = 0; i < currentChunkWidth; i++)
                    {
                        T[] yy = new T[currentChunkHeight];

                        for (int j = 0; j < currentChunkHeight; j++)
                        {
                            yy[j] = collection[lastChunkEndX + i][lastChunkEndY + j];
                        }

                        xx[i] = yy;
                    }

                    chunkList.Add(xx);

                    lastChunkEndX += currentChunkWidth;
                }

                lastChunkEndY += currentChunkHeight;
            }

            return chunkList.ToArray();

            // Loop x until xCount == 0
            // Take chunkWidth from xCount
            // Take Y from Y count (first loop only)
            // Add new chunk
            // Reset xCount and loop
        }

        private void SetPointColor(PixelData pixel, Random rng = null)
        {
            int x = pixel.CoordX;
            int y = pixel.CoordY;

            if (rng == null)
            {
                rng = new Random(_seed);
            }

            (int r, int g, int b) color = (0, 0, 0);

            switch (GenType)
            {
                case 0:

                    color.r = y % 3 == 0 ? 255 : 124;
                    color.g = x % 2 == 0 ? 255 : 124;
                    color.b = x % 9 == 0 || y % 8 == 0 ? 255 : 0;

                    break;

                default:

                    SelectedColorGen.SetCanvasSize();

                    if (RandomizeParameters)
                    {
                        SelectedColorGen.RandomizeParameters(_seed);
                    }

                    color = SelectedColorGen.GeneratePixel(x, y, rng, pixel);

                    break;
            }

            pixel.ColorRed = color.r;
            pixel.ColorBlue = color.g;
            pixel.ColorGreen = color.b;
        }

        private void SetPixelColor(Bitmap image, Bitmap template, PixelData point)
        {
            if (UseTemplate)
            {
                System.Drawing.Color templatePixel = template.GetPixel(point.CoordX, point.CoordY);

                bool isWhite = templatePixel.R == 255 &&
                              templatePixel.G == 255 &&
                              templatePixel.B == 255;

                if (!isWhite)
                {
                    int transparencyValue = templatePixel.A < 255
                        ? templatePixel.A
                        : 255 - new[] { templatePixel.R, templatePixel.G, templatePixel.B }.Max();

                    System.Drawing.Color color = System.Drawing.Color.FromArgb
                    (
                        transparencyValue,
                        point.ColorRed,
                        point.ColorGreen,
                        point.ColorBlue
                    );

                    image.SetPixel(point.CoordX, point.CoordY, color);
                }
                else
                {
                    image.SetPixel(point.CoordX, point.CoordY, BackgroundColor);
                }
            }
            else
            {
                System.Drawing.Color color = System.Drawing.Color.FromArgb
                (
                    point.ColorRed,
                    point.ColorGreen,
                    point.ColorBlue
                );

                image.SetPixel(point.CoordX, point.CoordY, color);
            }
        }

        private void UpdateGrid(EnPointUpdateType pointUpdateType)
        {
            CellsLoaded = 0;
            CellsToLoad = Width * Height;

            Task.Run(() => UpdatePoints(pointUpdateType));
        }

        public void UpdatePoints(EnPointUpdateType pointUpdateType)
        {
            LoadingCells = true;

            switch (pointUpdateType)
            {
                case EnPointUpdateType.Refresh:

                    PixelData[][] points = new PixelData[Width][];

                    Parallel.For(0, Width, x =>
                    {
                        PixelData[] column = new PixelData[Height];

                        for (int y = 0; y < Height; y++)
                        {
                            PixelData point = new PixelData
                            {
                                CoordX = x,
                                CoordY = y,
                            };

                            column[y] = point;

                            CellsLoaded++;
                        }

                        points[x] = column;
                    });

                    Points = points;

                    break;

                case EnPointUpdateType.Draw:

                    _colorGenManager.ClearPoints();

                    Random rng = new Random(_seed);

                    if (SelectedColorGen != null)
                    {
                        PixelData[][][] chunks = GetChunks(ChunkWidth, ChunkHeight, Points);

                        Parallel.ForEach(chunks, chunk =>
                        {
                            foreach (PixelData[] column in chunk)
                            {
                                foreach (PixelData point in column)
                                {
                                    if (GenType == EnGenType.BitsPieces)
                                    {
                                        SetPointColor(point, rng);
                                    }
                                    else
                                    {
                                        SetPointColor(point);
                                    }

                                    CellsLoaded++;
                                }
                            }
                        });
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(pointUpdateType), pointUpdateType, null);
            }

            LoadingCells = false;

            if (pointUpdateType == EnPointUpdateType.Draw)
            {
                GenerateImage();
            }
        }

        private Bitmap CreateMask()
        {
            Bitmap template = null;
            Bitmap mask = null;

            try
            {
                template = UseTemplate ? new Bitmap(TemplatePath) : null;
            }
            catch (Exception ex)
            {
                UseTemplate = false;
            }

            if (!UseTemplate)
            {
                return mask;
            }

            template = Utils.ResizeImage(template, MaskWidth, MaskHeight);

            mask = new Bitmap(Width, Height);

            // Center Mask
            int xOffset = (Width - MaskWidth) / 2;
            int yOffset = (Height - MaskHeight) / 2;

            if (InvertTemplate)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        mask.SetPixel(x, y, System.Drawing.Color.Black);
                    }
                }

                template = InvertMask(template);
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x <= xOffset ||
                        y <= yOffset ||
                        x >= MaskWidth + xOffset ||
                        y >= MaskHeight + yOffset) continue;

                    System.Drawing.Color templatePixel = template.GetPixel(x - xOffset, y - yOffset);
                    mask.SetPixel(x, y, templatePixel);
                }
            }

            Utils.Transparent2Color(mask, System.Drawing.Color.Black);

            return mask;
        }

        private Bitmap InvertMask(Bitmap template)
        {
            Bitmap invertBitmap = new Bitmap(template.Width, template.Height);

            for (int x = 0; x < template.Width; x++)
            {
                for (int y = 0; y < template.Height; y++)
                {
                    System.Drawing.Color templatePixel = template.GetPixel(x, y);

                    System.Drawing.Color invertedColor = System.Drawing.Color.FromArgb
                        (
                            Math.Abs(templatePixel.R - 255),
                            Math.Abs(templatePixel.G - 255),
                            Math.Abs(templatePixel.B - 255)
                        );

                    invertBitmap.SetPixel(x, y, invertedColor);
                }
            }

            return invertBitmap;
        }

        #endregion Private Methods
    }
}