using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Microsoft.Kinect;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace LaptopOrchestra.Kinect
{
    /// <summary>
    /// Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Queue to communicate between Kinect Data Producer and Data Comsumer
        /// </summary>
        private Queue<IReadOnlyDictionary<JointType,Joint>> queue = null;
        
        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        void App_Startup(object sender, StartupEventArgs e)
        {
			// Initialize logger
			log4net.Config.XmlConfigurator.Configure();
			Logger.Debug("App Startup");

			// Initialize our queue to pass data from Kinect Thread to Communications Thread
			this.queue = new Queue<IReadOnlyDictionary<JointType, Joint>>();

//            MainWindow mainWindow = new MainWindow(this.queue);
//            mainWindow.Top = 200;
//            mainWindow.Left = 500;
//            mainWindow.Show();

            ConfigurationTool configurationTool = new ConfigurationTool();
            configurationTool.Top = 200;
            configurationTool.Left = 500;
            configurationTool.Show();
            

            KinectSimulator kinectSimulator = new KinectSimulator(this.queue);
            kinectSimulator.Top = 200;
            kinectSimulator.Left = 600;
            kinectSimulator.Show();

            DataConsumer dataConsumer = new DataConsumer(this.queue, configurationTool);

            Thread consumer = new Thread(dataConsumer.consume);

            consumer.Start();
        }
    }
}
