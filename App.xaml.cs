//------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;
using log4net.Config;
using Microsoft.Kinect;
using System.Windows.Forms;
using System.ComponentModel;

[assembly: XmlConfigurator(Watch = true)]

namespace LaptopOrchestra.Kinect
{
    /// <summary>
    ///     Interaction logic for App
    /// </summary>
    public partial class App : System.Windows.Application
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


        /// <summary>
        ///     GUI for managing kinect data
        /// </summary>
        public ConfigurationTool configurationTool;
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

			var udpReceiver = new UDPReceiver(8080, SessionManager, KinectProcessor);

            // Initialize main GUI
            configurationTool = new ConfigurationTool(SessionManager, KinectProcessor)
            {
                Top = 0,
                Left = 0
            };
            configurationTool.Show();
            System.Windows.Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
        }        

        public static bool CloseCancel()
        {
            const string message = "Are you sure that you would like to close KICASS?";
            const string caption = "Cancel Installer";
            var result = System.Windows.Forms.MessageBox.Show(message, caption,
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                return true;
            else
                return false;
        }

        public void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (CloseCancel())
            {
                Logger.Info("Closing Connections");
                SessionManager.CloseAllConnections();
                Logger.Info("Shut down Kinect");
                KinectProcessor.StopKinect();
                Logger.Info("Kill the update thread in the GUI");
                configurationTool.KillUpdateThread();
                Logger.Info("KiCASS Exited Succesfully");
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}