//------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows;
using log4net.Config;
using Microsoft.Kinect;

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


            // TODO: Add a flag to start up the simulator if desired -- should be a flag passable somehow @ app startup
            /*KinectSimulator kinectSimulator = new KinectSimulator(this.queue);
            kinectSimulator.Top = 200;
            kinectSimulator.Left = 600;
            kinectSimulator.Show();*/

            var kinectProcessor = new KinectProcessor();

            new DataSubscriber(ConfigurationFlags, kinectProcessor);


            // Initialize main GUI
            var configurationTool = new ConfigurationTool(ConfigurationFlags, kinectProcessor)
            {
                Top = 200,
                Left = 500
            };
            configurationTool.Show();
        }
    }
}