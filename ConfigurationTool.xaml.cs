using System.Windows;

namespace LaptopOrchestra.Kinect
{
    /// <summary>
    /// Interaction logic for ConfigurationTool.xaml
    /// </summary>
    public partial class ConfigurationTool : Window
    {

        /// <summary>
        /// Flags to indidcate which data set to send to OSC
        /// </summary>
        private ConfigurationFlags configurationFlags = new ConfigurationFlags();

        public ConfigurationTool(ConfigurationFlags configurationFlags)
        {
            this.configurationFlags = configurationFlags;
            InitializeComponent();
        }

        private void LeftHandChecked(object sender, RoutedEventArgs e)
        {
            configurationFlags.leftHand = true;
        }

        private void LeftHandUnchecked(object sender, RoutedEventArgs e)
        {
            configurationFlags.leftHand = false;
        }

        private void RightHandChecked(object sender, RoutedEventArgs e)
        {
            configurationFlags.rightHand = true;
        }

        private void RightHandUnchecked(object sender, RoutedEventArgs e)
        {
            configurationFlags.rightHand = false;
        }
    }
}
