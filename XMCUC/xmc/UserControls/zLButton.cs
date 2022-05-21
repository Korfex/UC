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
    public partial class zLButton : Control
    {
        public zLButton()
        {
            LayoutUpdated += (a, b) => UpdateFormattedText();
            LayoutUpdated += (a, b) => InvalidateVisual();
        }

        public event RoutedEventHandler Click;

        private static void OnValueChanged(object sender, DependencyPropertyChangedEventArgs e) => ((zLButton)sender).InvalidateVisual();
        private static void OnTextChanged(object sender, DependencyPropertyChangedEventArgs e) => ((zLButton)sender).UpdateFormattedText();
        
        public void UpdateFormattedText() => FormattedText = new FormattedText(Text, CultureInfo.CurrentUICulture, FlowDirection, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground);

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));
            drawingContext.PushOpacity(HoveredLevel);
            drawingContext.DrawRectangle(HoveredBackground, null, new Rect(0, 0, ActualWidth, ActualHeight));
            drawingContext.PushOpacity(MDLevel);
            drawingContext.DrawRectangle(MDBackground, null, new Rect(0, 0, ActualWidth, ActualHeight));
            drawingContext.Pop();
            drawingContext.PushOpacity(1);
            drawingContext.DrawText(FormattedText, new Point((ActualWidth - FormattedText.Width) / 2, (ActualHeight - FormattedText.Height) / 2));
            base.OnRender(drawingContext);
        }

        public static readonly DependencyProperty
            HoveredBackgroundProperty = DependencyProperty.Register("HoveredBackground", typeof(Brush), typeof(zLButton), new PropertyMetadata(Brushes.Gray, new PropertyChangedCallback(OnValueChanged))),
            MDBackgroundProperty = DependencyProperty.Register("MDBackground", typeof(Brush), typeof(zLButton), new PropertyMetadata(Brushes.Gold, new PropertyChangedCallback(OnValueChanged))),
            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(zLButton), new PropertyMetadata((string)null, new PropertyChangedCallback(OnTextChanged)));

        private static readonly DependencyProperty
            HoveredLevelProp = DependencyProperty.Register("HoveredLevel", typeof(double), typeof(zLButton), new PropertyMetadata(0d, new PropertyChangedCallback(OnValueChanged))),
            MDLevelProp = DependencyProperty.Register("MDLevel", typeof(double), typeof(zLButton), new PropertyMetadata(0d, new PropertyChangedCallback(OnValueChanged))),
            FormattedTextProp = DependencyProperty.Register("FormattedText", typeof(FormattedText), typeof(zLButton), new PropertyMetadata((FormattedText)null, new PropertyChangedCallback(OnValueChanged)));

        private FormattedText FormattedText
        {
            get => (FormattedText)GetValue(FormattedTextProp);
            set => SetValue(FormattedTextProp, value);
        }

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

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
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
