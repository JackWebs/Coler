using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using coler.Annotations;
using coler.BusinessLogic;
using coler.BusinessLogic.Manager;
using coler.Globals;
using coler.Model.ColorGen;
using coler.Model.ColorGen.Interface;
using coler.Model.Enum;
using coler.Model.GenImage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Color = System.Windows.Media.Color;
using PixelData = coler.Model.PixelData;

namespace coler.ViewModel
{
    public class GenerateImageViewModel : ViewModelBase
    {
        #region Fields

        private ColorGenManager _colorGenManager;
        private GenImageManager _genImageManager;

        public ColorGen1 ColorGen1 { get; set; }

        private int _seed;

        public enum EnPointUpdateType
        {
            Refresh,
            Draw
        };

        #region Backing Fields

        private PixelData[][] _points;

        private double _xParameter = 1;
        private double _yParameter = 1;

        private int _width;
        private int _height;
        private int _genType;

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

        #endregion

        #endregion

        #region Properties

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

        public double XParameter
        {
            get => _xParameter;
            set
            {
                if (Math.Abs(value - _xParameter) < Constants.DoubleEqualityTolerance) return;
                _xParameter = value;
                RaisePropertyChanged();
            }
        }

        public double YParameter
        {
            get => _yParameter;
            set
            {
                if (Math.Abs(value - _yParameter) < Constants.DoubleEqualityTolerance) return;
                _yParameter = value;
                RaisePropertyChanged();
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

                _colorGenManager.CanvasWidth = _height;

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

                var scale = (double)_maskWidth / OriginalMaskWidth;

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

                var scale = (double)_maskHeight / OriginalMaskHeight;

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

        public int GenType
        {
            get => _genType;
            set
            {
                if (value == _genType) return;
                _genType = value;
                RaisePropertyChanged();
            }
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

        #endregion

        #region Getter Properties

        public string TemplateName => string.IsNullOrWhiteSpace(TemplatePath) ? "" : TemplatePath.Split('\\').Last();

        public System.Drawing.Color BackgroundColor => System.Drawing.Color.FromArgb(SelectedColor.R, SelectedColor.G, SelectedColor.B);

        #endregion

        #region OpenFileDialog properties

        public string Filter { get; set; }
        public string Title { get; set; } = "Select Template";

        public bool DialogResult { get; protected set; }

        #endregion

        #region Command Properties

        public ICommand RefreshGridCommand { get; private set; }
        public ICommand RedrawGridCommand { get; private set; }
        public ICommand GenerateImageCommand { get; private set; }
        public ICommand SelectTemplateCommand { get; private set; }

        #endregion

        public GenerateImageViewModel()
        {
            Initialize();
        }

        #region Initialization

        private void Initialize()
        {
            _colorGenManager = ColorGenManager.Instance;
            _genImageManager = GenImageManager.Instance;

            ColorGen1 = new ColorGen1();

            ScalingModes = new List<BitmapScalingMode>
            {
                BitmapScalingMode.NearestNeighbor,
                BitmapScalingMode.Linear,
                BitmapScalingMode.Fant
            };

            _width = _colorGenManager.CanvasWidth;
            _height = _colorGenManager.CanvasHeight;

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

        #endregion

        #region Commands

        public void RefreshGrid()
        {
            UpdateGrid(EnPointUpdateType.Refresh);
        }

        public void RedrawGrid()
        {
            var rand = new Random();

            _seed = rand.Next(0, 1000000);

            UpdateGrid(EnPointUpdateType.Draw);
        }

        public void GenerateImage()
        {
            var genImage = new GenImage(DateTime.Now);

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

                foreach (var column in Points)
                {
                    foreach (var point in column)
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

        #endregion

        #region Private Methods

        private void SetPointColor(PixelData pixel, IColorGen colorGen, Random rng = null)
        {
            var x = pixel.CoordX;
            var y = pixel.CoordY;

            if (rng == null)
            {
                rng = new Random(_seed);
            }

            (int r, int g, int b) color = (0,0,0);

            switch (GenType)
            {
                case 0:

                    color.r = y % 3 == 0 ? 255 : 124;
                    color.g = x % 2 == 0 ? 255 : 124;
                    color.b = x % 9 == 0 || y % 8 == 0 ? 255 : 0;

                    break;

                default:

                    colorGen.SetCanvasSize();

                    if (RandomizeParameters)
                    {
                        colorGen.RandomizeParameters(rng);
                    }

                    color = colorGen.GeneratePixel(x, y);

                    break;
            }

            /*if (GenType == 0)
            {
                
            }
            else
            {
                

                colorGreen = SelectedColorGen.GenerateColor(x, y, GreenParameter, EnColor.Green, rng, point);
                colorRed = SelectedColorGen.GenerateColor(x, y, RedParameter, EnColor.Red, rng, point);
                colorBlue = SelectedColorGen.GenerateColor(x, y, BlueParameter, EnColor.Blue, rng, point);
            }*/

            pixel.ColorRed = color.r;
            pixel.ColorBlue = color.g;
            pixel.ColorGreen = color.b;
        }

        private void SetPixelColor(Bitmap image, Bitmap template, PixelData point)
        {
            if (UseTemplate)
            {
                var templatePixel = template.GetPixel(point.CoordX, point.CoordY);

                var isWhite = templatePixel.R == 255 &&
                              templatePixel.G == 255 &&
                              templatePixel.B == 255;

                if (!isWhite)
                {
                    var transparencyValue = templatePixel.A < 255
                        ? templatePixel.A
                        : 255 - new[] { templatePixel.R, templatePixel.G, templatePixel.B }.Max();

                    var color = System.Drawing.Color.FromArgb
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
                var color = System.Drawing.Color.FromArgb
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

                    var points = new PixelData[Width][];

                    Parallel.For(0, Width, x =>
                    {
                        var column = new PixelData[Height];

                        for (var y = 0; y < Height; y++)
                        {
                            var point = new PixelData
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

                    var rng = new Random(_seed);
                    var colorGen = _colorGenManager.GetColorGen(GenType);

                    if (colorGen != null)
                    {
                        Parallel.ForEach(Points, column =>
                        {
                            foreach (var point in column)
                            {
                                if (GenType == 5)
                                {
                                    SetPointColor(point, colorGen, rng);
                                }
                                else
                                {
                                    SetPointColor(point, colorGen);
                                }

                                CellsLoaded++;
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
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        mask.SetPixel(x, y, System.Drawing.Color.Black);
                    }
                }

                template = InvertMask(template);
            }

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (x <= xOffset || 
                        y <= yOffset || 
                        x >= MaskWidth + xOffset || 
                        y >= MaskHeight + yOffset) continue;

                    var templatePixel = template.GetPixel(x - xOffset, y - yOffset);
                    mask.SetPixel(x, y, templatePixel);
                }
            }

            Utils.Transparent2Color(mask, System.Drawing.Color.Black);

            return mask;
        }

        private Bitmap InvertMask(Bitmap template)
        {
            var invertBitmap = new Bitmap(template.Width, template.Height);

            for (var x = 0; x < template.Width; x++)
            {
                for (var y = 0; y < template.Height; y++)
                {
                    var templatePixel = template.GetPixel(x, y);

                    var invertedColor = System.Drawing.Color.FromArgb
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

        #endregion

        #region Events



        #endregion
    }
}