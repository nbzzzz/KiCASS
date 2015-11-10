//------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace LaptopOrchestra.Kinect
{
    using Microsoft.Kinect;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// Queue to communicate between Kinect Data Producer and Data Comsumer
        /// </summary>
        private Queue<IDictionary<JointType,Joint>> queue = null;
        
        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        void App_Startup(object sender, StartupEventArgs e)
        {
            // Initialize our queue to pass data from Kinect Thread to Communications Thread
            this.queue = new Queue<IDictionary<JointType, Joint>>();



            // Initialize main GUI
            ConfigurationTool configurationTool = new ConfigurationTool(this.queue);
            configurationTool.Top = 200;
            configurationTool.Left = 500;
            configurationTool.Show();



            // TODO: Add a flag to start up the simulator if desired -- should be a flag passable somehow @ app startup
            /*KinectSimulator kinectSimulator = new KinectSimulator(this.queue);
            kinectSimulator.Top = 200;
            kinectSimulator.Left = 600;
            kinectSimulator.Show();*/

            DataConsumer dataConsumer = new DataConsumer(this.queue, configurationTool);

            Thread consumer = new Thread(dataConsumer.consume);

            consumer.Start();
        }
    }
}
