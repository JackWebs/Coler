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
using coler.Globals;
using coler.Model.ColorGen;
using coler.Model.Enum;
using coler.Model.GenImage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Color = System.Windows.Media.Color;
using PixelData = coler.Model.PixelData;

namespace coler.ViewModel
{
    public class GenerateImageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        private ColorGenManager _colorGenManager;
        private GenImageManager _genImageManager;

        private int _seed;

        #region Backing Fields

        private List<PixelData> _points;

        private double _cellSize = 2;

        private int _width = 1920;
        private int _height = 1080;
        private double _xParameter = 1;
        private double _yParameter = 1;
        private int _genType;

        private bool _randomizeParameters;
        private int _redParameter;
        private int _greenParameter;
        private int _blueParameter;
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

        #endregion

        #endregion

        #region Properties

        public List<PixelData> Points
        {
            get => _points;
            set
            {
                if (Equals(value, _points)) return;
                _points = value;
                OnPropertyChanged();
            }
        }

        public IColorGenFunction SelectedColorGen { get; set; }

        public BitmapScalingMode SelectedScalingMode
        {
            get => _selectedScalingMode;
            set
            {
                if (value == _selectedScalingMode) return;
                _selectedScalingMode = value;
                OnPropertyChanged();
            }
        }

        public List<BitmapScalingMode> ScalingModes
        {
            get => _scalingModes;
            set
            {
                if (Equals(value, _scalingModes)) return;
                _scalingModes = value;
                OnPropertyChanged();
            }
        }

        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (value.Equals(_selectedColor)) return;
                _selectedColor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        public GenImage CurrentImage
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public string TemplatePath
        {
            get => _templatePath;
            set
            {
                if (value == _templatePath) return;
                _templatePath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TemplateName));
            }
        }

        public string LastCreatedImagePath { get; set; }

        public double CellSize
        {
            get => _cellSize;
            set
            {
                if (value.Equals(_cellSize)) return;
                _cellSize = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public int MaskWidth
        {
            get => _maskWidth;
            set
            {
                if (value == _maskWidth) return;
                _maskWidth = value;
                OnPropertyChanged();
            }
        }

        public int MaskHeight
        {
            get => _maskHeight;
            set
            {
                if (value == _maskHeight) return;
                _maskHeight = value;
                OnPropertyChanged();
            }
        }

        public double XParameter
        {
            get => _xParameter;
            set
            {
                if (value == _xParameter) return;
                _xParameter = value;
                OnPropertyChanged();
            }
        }

        public double YParameter
        {
            get => _yParameter;
            set
            {
                if (value == _yParameter) return;
                _yParameter = value;
                OnPropertyChanged();
            }
        }

        public int GenType
        {
            get => _genType;
            set
            {
                if (value == _genType) return;
                _genType = value;
                OnPropertyChanged();
            }
        }

        public int RedParameter
        {
            get => _redParameter;
            set
            {
                if (value == _redParameter) return;
                _redParameter = value;
                OnPropertyChanged();
            }
        }

        public int GreenParameter
        {
            get => _greenParameter;
            set
            {
                if (value == _greenParameter) return;
                _greenParameter = value;
                OnPropertyChanged();
            }
        }

        public int BlueParameter
        {
            get => _blueParameter;
            set
            {
                if (value == _blueParameter) return;
                _blueParameter = value;
                OnPropertyChanged();
            }
        }

        public int CellsLoaded
        {
            get => _cellsLoaded;
            set
            {
                if (value == _cellsLoaded) return;
                _cellsLoaded = value;
                OnPropertyChanged();
            }
        }

        public int CellsToLoad
        {
            get => _cellsToLoad;
            set
            {
                if (value == _cellsToLoad) return;
                _cellsToLoad = value;
                OnPropertyChanged();
            }
        }

        public bool RandomizeParameters
        {
            get => _randomizeParameters;
            set
            {
                if (value == _randomizeParameters) return;
                _randomizeParameters = value;
                OnPropertyChanged();
            }
        }

        public bool LoadingCells
        {
            get => _loadingCells;
            set
            {
                if (value == _loadingCells) return;
                _loadingCells = value;
                OnPropertyChanged();
            }
        }

        public bool UseTemplate
        {
            get => _useTemplate;
            set
            {
                if (value == _useTemplate) return;
                _useTemplate = value;
                OnPropertyChanged();
            }
        }

        public bool InvertTemplate
        {
            get => _invertTemplate;
            set
            {
                if (value == _invertTemplate) return;
                _invertTemplate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Getter Properties

        public string TemplateName => string.IsNullOrWhiteSpace(TemplatePath) ? "" : TemplatePath.Split('\\').Last();

        public double ActualImageWidth => Width * CellSize;

        public System.Drawing.Color BackgroundColor => System.Drawing.Color.FromArgb(SelectedColor.R, SelectedColor.G, SelectedColor.B);

        #endregion

        #region OpenFileDialog properties

        public string Filter { get; set; }
        public string Title { get; set; } = "Select Template";

        public bool DialogResult { get; protected set; }

        //protected IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();

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

            ScalingModes = new List<BitmapScalingMode>
            {
                BitmapScalingMode.NearestNeighbor,
                BitmapScalingMode.Linear,
                BitmapScalingMode.Fant
            };

            Points = _colorGenManager.Points;

            InitializeCommands();
            InitializePoints();
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
            Points.Clear();

            var points = new List<PixelData>();

            UpdateGenType();

            SelectedColorGen?.UpdateDimensions(Width, Height);
            SelectedColorGen?.UpdateParameters(XParameter, YParameter);

            CellsLoaded = 0;
            CellsToLoad = Width * Height;

            Task.Run(() =>
            {
                LoadingCells = true;

                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        var point = new PixelData
                        {
                            CoordX = x,
                            CoordY = y,
                        };

                        points.Add(point);

                        CellsLoaded++;
                    }
                }

                Points.AddRange(points);

                OnPropertyChanged(nameof(ActualImageWidth));

                LoadingCells = false;
            });
        }

        public void RedrawGrid()
        {
            var rand = new Random();

            _seed = rand.Next(0, 1000000);

            UpdateGenType();

            SelectedColorGen?.UpdateDimensions(Width, Height);
            SelectedColorGen?.UpdateParameters(XParameter, YParameter);

            CellsLoaded = 0;
            CellsToLoad = Width * Height;

            Task.Run(() =>
            {
                LoadingCells = true;

                foreach (var point in Points)
                {
                    SetPointColor(point);

                    CellsLoaded++;
                }

                LoadingCells = false;

                GenerateImage();
            });
        }

        public void GenerateImage()
        {
            var dateTimeNow = DateTime.Now;
            var fileName = dateTimeNow.ToString("yyyy-MM-dd hh-mm-ss") + ".png";

            var genImage = new GenImage
            {
                DateCreated = dateTimeNow,
                SourceFilePath = Path.Combine(FilePaths.BufferDirectory, fileName),
                ThumbnailFilePath = Path.Combine(FilePaths.ThumbnailDirectory, fileName),     
            };

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

                foreach (var point in Points)
                {
                    SetPixelColor(image, mask, point);
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
                InitialDirectory = FilePaths.TemplateDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                TemplatePath = openFileDialog.FileName;
            }

            try
            {
                Bitmap template = new Bitmap(TemplatePath);
                MaskWidth = template.Width;
                MaskHeight = template.Height;
            }
            catch (Exception ex)
            {
                //ignored
            }
        }

        #endregion

        #region Private Methods

        private void SetPointColor(PixelData point)
        {
            var x = point.CoordX;
            var y = point.CoordY;

            var rng = new Random(_seed);

            int colorRed = 0;
            int colorGreen = 0;
            int colorBlue = 0;

            if (GenType == 0)
            {
                colorRed = y % 3 == 0 ? 255 : 124;
                colorGreen = x % 2 == 0 ? 255 : 124;
                colorBlue = x % 9 == 0 || y % 8 == 0 ? 255 : 0;
            }
            else
            {
                if (RandomizeParameters)
                {
                    SetParameters(SelectedColorGen.ColorParameters);
                }

                colorGreen = SelectedColorGen.GenerateColor(x, y, GreenParameter, EnColor.Green, rng, point);
                colorRed = SelectedColorGen.GenerateColor(x, y, RedParameter, EnColor.Red, rng, point);
                colorBlue = SelectedColorGen.GenerateColor(x, y, BlueParameter, EnColor.Blue, rng, point);
            }

            point.ColorRed = colorRed;
            point.ColorBlue = colorBlue;
            point.ColorGreen = colorGreen;
        }

        private void SetParameters(int[] parameters)
        {
            var rng = new Random(_seed);

            RedParameter = rng.Next(parameters.First(), parameters.Last());
            GreenParameter = rng.Next(parameters.First(), parameters.Last());
            BlueParameter = rng.Next(parameters.First(), parameters.Last());
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

        private void UpdateGenType()
        {
            SelectedColorGen = _colorGenManager.GetColorGen(GenType);
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
                    if (x > xOffset &&
                        y > yOffset &&
                        x < MaskWidth + xOffset &&
                        y < MaskHeight + yOffset)
                    {
                        var templatePixel = template.GetPixel(x - xOffset, y - yOffset);
                        mask.SetPixel(x, y, templatePixel);
                    }
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

        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}