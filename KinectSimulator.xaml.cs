using System.Collections.Generic;
using System.Windows;
using Microsoft.Kinect;
using System.IO;
using System.Windows.Controls;


namespace LaptopOrchestra.Kinect
{


    /// <summary>
    /// Interaction logic for KinectSimulator
    /// </summary>
    public partial class KinectSimulator : Window
    {

        /// <summary>
        /// Queue to communicate between Kinect Simulator and Data Comsumer
        /// </summary>
        private readonly Queue<IReadOnlyDictionary<JointType, Joint>> _queue;

        /// <summary>
        /// List of kinect simulator data
        /// </summary>
        private readonly List<KinectSimulatorData> _simulatorDataList;

        public KinectSimulator(Queue<IReadOnlyDictionary<JointType, Joint>> queue)
        {
            // Set the communication _queue
            this._queue = queue;

            // Initialize our simulate data list
            _simulatorDataList = new List<KinectSimulatorData>();

            // Parse our Kinect Simulator Data csv 
            string[] allLines = File.ReadAllLines(@"SimulatorData\shoot.csv");

            foreach (var line in allLines){
                _simulatorDataList.Add(new KinectSimulatorData(line.Split(' ')));
            }

            InitializeComponent();
        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid != null) grid.ItemsSource = _simulatorDataList;
        }

        private void flushToQueue_Click(object sender, RoutedEventArgs e)
        {
            foreach (var simulatorData in _simulatorDataList){
                _queue.Enqueue(simulatorData.toKinectData());
            }
        }


    }
}