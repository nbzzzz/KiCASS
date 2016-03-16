using System.Windows;
using log4net.Config;
using LaptopOrchestra.Kinect.ViewModel;
using System;

namespace LaptopOrchestra.Kinect
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize logger
            XmlConfigurator.Configure();
            Logger.Debug("App Startup");
            
            //Create the main window view.
            MainWindow window = new MainWindow();

            // Create the ViewModel to which the main window binds.
            MainWindowViewModel viewModel = new MainWindowViewModel();

            // When the ViewModel asks to be closed,  close the window.
            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                window.Close();
            };
            viewModel.RequestClose += handler;

            // Allow all controls in the window to bind to the ViewModel by
            //setting the DataContext, which propagates down the element tree.
            window.DataContext = viewModel;

            //Show the main window.
            window.Show();
        }
    }
}