//------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
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
        public Queue<IDictionary<JointType,Joint>> queue = null;

        /// <summary>
        /// Configuration Items selected
        /// </summary>
        public Dictionary<JointType, bool> ConfigurationFlags = null;

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
			queue = new Queue<IDictionary<JointType, Joint>>();

            ConfigurationFlags = new Dictionary<JointType, bool>();

            // Initialize main GUI
            var configurationTool = new ConfigurationTool(this.queue, ConfigurationFlags)
            {
                Top = 200,
                Left = 500
            };
            configurationTool.Show();

            // TODO: Add a flag to start up the simulator if desired -- should be a flag passable somehow @ app startup
            /*KinectSimulator kinectSimulator = new KinectSimulator(this.queue);
            kinectSimulator.Top = 200;
            kinectSimulator.Left = 600;
            kinectSimulator.Show();*/

            var dataConsumer = new DataConsumer(this.queue, ConfigurationFlags);
            var consumer = new Thread(dataConsumer.Consume);

            consumer.Start();
        }
    }
}
