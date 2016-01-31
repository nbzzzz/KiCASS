using System.Collections.Generic;
using System.Windows;
using log4net.Config;
using Microsoft.Kinect;
using LaptopOrchestra.Kinect.ViewModel;
using LaptopOrchestra.Kinect.Model;

[assembly: XmlConfigurator(Watch = true)]

namespace LaptopOrchestra.Kinect
{
    /// <summary>
    ///     Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        ///     Configuration Items selected
        /// </summary>
        public Dictionary<JointType, bool> ConfigurationFlags;

        /// <summary>
        ///      Kinect 2.0 object that publishes new data
        /// </summary>
        public KinectProcessor KinectProcessor;

        /// <summary>
        ///     Manages the active connections to KiCASS
        /// </summary>
        public SessionManager SessionManager;

        public BackgroundThread BackgroundThread;

        public UDPReceiver udpRec;

        /// <summary>
        ///     Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;

            // Initialize logger
            XmlConfigurator.Configure();
            Logger.Debug("App Startup");

            ConfigurationFlags = new Dictionary<JointType, bool>();
            KinectProcessor = new KinectProcessor();
			SessionManager = new SessionManager();
            BackgroundThread = new BackgroundThread();

            //var udpReceiver = new UDPReceiver(8080, SessionManager, KinectProcessor);
            udpRec = new UDPReceiver(8080, SessionManager, KinectProcessor);

            // Initialize main GUI
            MainWindow window = new MainWindow();
            var viewModel = new MainWindowViewModel(SessionManager, KinectProcessor);
            window.DataContext = viewModel;
            MainWindow.Show();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            Logger.Info("Closing Connections");
            SessionManager.CloseAllConnections();
            Logger.Info("Shut down Kinect");
            KinectProcessor.StopKinect();
            Logger.Info("KiCASS Exited Succesfully");
            udpRec.Close();
        }
    }
}