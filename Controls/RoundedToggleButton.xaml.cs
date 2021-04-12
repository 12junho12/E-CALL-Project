using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CargoLampTest.Controls
{
    public partial class RoundedToggleButton : UserControl
    {
        public RoundedToggleButton()
        {
            InitializeComponent();
        }

        [Category("Appearance")]
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RoundedToggleButton), new PropertyMetadata(new CornerRadius()));

    }
}
