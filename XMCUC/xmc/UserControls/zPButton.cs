using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static xmc.uc.zGlobals;


namespace xmc.uc
{
    public class zPButton : Control
    {
        public zPButton()
        {
        }

        public event RoutedEventHandler Click;

        private static void OnValueChanged(object sender, DependencyPropertyChangedEventArgs e) => ((zPButton)sender).InvalidateVisual();

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));
            drawingContext.PushOpacity(HoveredLevel);
            drawingContext.DrawRectangle(HoveredBackground, null, new Rect(0, 0, ActualWidth, ActualHeight));
            drawingContext.Pop();
            drawingContext.PushOpacity(MDLevel);
            drawingContext.DrawRectangle(MDBackground, null, new Rect(0, 0, ActualWidth, ActualHeight));
            drawingContext.Pop();
            if (Data == null)
                goto end;
            var p = new Pen(Foreground, PathThickness);
            var j = Data.GetRenderBounds(p);
            drawingContext.PushTransform(new TranslateTransform((ActualWidth - j.Size.Width) / 2, (ActualHeight - j.Size.Height) / 2));
            drawingContext.DrawGeometry(null, p, Data);
end:
            base.OnRender(drawingContext);
        }


        public static readonly DependencyProperty
            HoveredBackgroundProperty = DependencyProperty.Register("HoveredBackground", typeof(Brush), typeof(zPButton), new PropertyMetadata(Brushes.Gray, new PropertyChangedCallback(OnValueChanged))),
            MDBackgroundProperty = DependencyProperty.Register("MDBackground", typeof(Brush), typeof(zPButton), new PropertyMetadata(Brushes.Gold, new PropertyChangedCallback(OnValueChanged))),
            DataProperty = DependencyProperty.Register("Data", typeof(Geometry), typeof(zPButton), new PropertyMetadata((Geometry)null, new PropertyChangedCallback(OnValueChanged))),
            PathThicknessProperty = DependencyProperty.Register("PathThickness", typeof(double), typeof(zPButton), new PropertyMetadata(0d, new PropertyChangedCallback(OnValueChanged)));

        private static readonly DependencyProperty
            HoveredLevelProp = DependencyProperty.Register("HoveredLevel", typeof(double), typeof(zPButton), new PropertyMetadata(0d, new PropertyChangedCallback(OnValueChanged))),
            MDLevelProp = DependencyProperty.Register("MDLevel", typeof(double), typeof(zPButton), new PropertyMetadata(0d, new PropertyChangedCallback(OnValueChanged)));

        private double HoveredLevel
        {
            get => (double)GetValue(HoveredLevelProp);
            set => SetValue(HoveredLevelProp, value);
        }

        private double MDLevel
        {
            get => (double)GetValue(MDLevelProp);
            set => SetValue(MDLevelProp, value);
        }

        public Brush HoveredBackground
        {
            get => (Brush)GetValue(HoveredBackgroundProperty);
            set => SetValue(HoveredBackgroundProperty, value);
        }

        public Brush MDBackground
        {
            get => (Brush)GetValue(MDBackgroundProperty);
            set => SetValue(MDBackgroundProperty, value);
        }

        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public double PathThickness
        {
            get => (double)GetValue(PathThicknessProperty);
            set => SetValue(PathThicknessProperty, value);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            BeginAnimation(MDLevelProp, new DoubleAnimation(1, animationSpeed));
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            BeginAnimation(MDLevelProp, new DoubleAnimation(0, animationSpeed));
            if (Click != null)
                Click(this, e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            BeginAnimation(HoveredLevelProp, new DoubleAnimation(1, animationSpeed));
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            BeginAnimation(HoveredLevelProp, new DoubleAnimation(0, animationSpeed));
            base.OnMouseLeave(e);
        }
    }
}
