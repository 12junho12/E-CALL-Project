

using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace CargoLampTest.Controls
{
    public partial class LedControl1 : CheckBox
    {
        static LedControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LedControl1), new FrameworkPropertyMetadata(typeof(LedControl1)));
        }

        public static readonly DependencyProperty OnColorProperty =
            DependencyProperty.Register("OnColor", typeof(Brush), typeof(LedControl1), new PropertyMetadata(Brushes.Green));

        public Brush OnColor
        {
            get { return (Brush)GetValue(OnColorProperty); }
            set { SetValue(OnColorProperty, value); }
        }

        public static readonly DependencyProperty OffColorProperty =
            DependencyProperty.Register("OffColor", typeof(Brush), typeof(LedControl1), new PropertyMetadata(Brushes.Red));

        public Brush OffColor
        {
            get { return (Brush)GetValue(OffColorProperty); }
            set { SetValue(OffColorProperty, value); }
        }
    }

}