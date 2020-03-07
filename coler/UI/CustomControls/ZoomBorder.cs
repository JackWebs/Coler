using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace coler.UI.CustomControls
{
    public class ZoomBorder : Border
    {
        #region Fields

        private UIElement _child;
        private Point _origin;
        private Point _start;
        private Point _lastImageTransform;

        #endregion Fields

        #region Properties

        public Point LastImageTransform
        {
            get => _lastImageTransform;
            private set
            {
                _lastImageTransform = value;
                OnImageTransformChanged();
            }
        }

        #endregion Properties

        #region Event Handlers

        public event EventHandler ImageTransformChanged;

        public static RoutedEvent DoubleClickEvent =
            EventManager.RegisterRoutedEvent("DoubleClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZoomBorder));

        public event RoutedEventHandler DoubleClick
        {
            add => AddHandler(DoubleClickEvent, value);
            remove => RemoveHandler(DoubleClickEvent, value);
        }

        #endregion Event Handlers

        private TranslateTransform GetTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is TranslateTransform);
        }

        private ScaleTransform GetScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is ScaleTransform);
        }

        public override UIElement Child
        {
            get => base.Child;
            set
            {
                if (value != null && value != Child)
                    Initialize(value);
                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            _child = element;
            if (_child == null) return;

            LastImageTransform = new Point(0, 0);

            TransformGroup group = new TransformGroup();
            ScaleTransform st = new ScaleTransform();
            TranslateTransform tt = new TranslateTransform();

            group.Children.Add(st);
            group.Children.Add(tt);

            _child.RenderTransform = group;
            _child.RenderTransformOrigin = new Point(0.0, 0.0);

            MouseWheel += Child_MouseWheel;
            MouseLeftButtonDown += Child_MouseLeftButtonDown;
            MouseLeftButtonUp += Child_MouseLeftButtonUp;
            MouseMove += Child_MouseMove;
            PreviewMouseRightButtonDown += Child_PreviewMouseRightButtonDown;
        }

        public void Reset()
        {
            if (_child == null) return;

            // reset zoom
            ScaleTransform st = GetScaleTransform(_child);
            st.ScaleX = 1.0;
            st.ScaleY = 1.0;

            // reset pan
            TranslateTransform tt = GetTranslateTransform(_child);
            tt.X = 0.0;
            tt.Y = 0.0;
        }

        public void Zoom(double zoom, Point relative)
        {
            if (_child == null) return;

            ScaleTransform st = GetScaleTransform(_child);
            TranslateTransform tt = GetTranslateTransform(_child);

            if (!(zoom > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
                return;

            double absoluteX = relative.X * st.ScaleX + tt.X;
            double absoluteY = relative.Y * st.ScaleY + tt.Y;

            st.ScaleX += zoom;
            st.ScaleY += zoom;

            tt.X = absoluteX - relative.X * st.ScaleX;
            tt.Y = absoluteY - relative.Y * st.ScaleY;
        }

        #region Child Events

        private void Child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoom = e.Delta > 0 ? .2 : -.2;
            Point relative = e.GetPosition(_child);

            Zoom(zoom, relative);
        }

        private void Child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_child == null) return;

            TranslateTransform tt = GetTranslateTransform(_child);
            _start = e.GetPosition(this);
            _origin = new Point(tt.X, tt.Y);
            Cursor = Cursors.Hand;
            _child.CaptureMouse();
        }

        private void Child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_child == null) return;

            _child.ReleaseMouseCapture();
            Cursor = Cursors.Arrow;
        }

        private void Child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Reset();
        }

        private void Child_MouseMove(object sender, MouseEventArgs e)
        {
            if (_child == null) return;
            if (!_child.IsMouseCaptured) return;

            TranslateTransform tt = GetTranslateTransform(_child);
            Vector v = _start - e.GetPosition(this);
            tt.X = _origin.X - v.X;
            tt.Y = _origin.Y - v.Y;

            LastImageTransform = new Point(tt.X, tt.Y);
        }

        #endregion Child Events

        #region General Events

        public void OnImageTransformChanged()
        {
            ImageTransformChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDoubleClick()
        {
            RoutedEventArgs args = new RoutedEventArgs(DoubleClickEvent, this);

            RaiseEvent(args);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (e.ClickCount == 2)
            {
                OnDoubleClick();
            }
        }

        #endregion General Events
    }
}