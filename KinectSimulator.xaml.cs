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
        private Queue<IReadOnlyDictionary<JointType, Joint>> queue = null;

        /// <summary>
        /// List of kinect simulator data
        /// </summary>
        public List<KinectSimulatorData> simulatorDataList = null;

        public KinectSimulator(Queue<IReadOnlyDictionary<JointType, Joint>> queue)
        {
            // Set the communication queue
            this.queue = queue;

            // Initialize our simulate data list
            simulatorDataList = new List<KinectSimulatorData>();

            // Parse our Kinect Simulator Data csv 
            string[] allLines = File.ReadAllLines(@"C:\Projects\CapstoneKinectLaptopOrchestra\SimulatorData\shoot.csv");

            foreach (var line in allLines){
                simulatorDataList.Add(new KinectSimulatorData(line.Split(' ')));
            }

            InitializeComponent();
        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as DataGrid;
            grid.ItemsSource = simulatorDataList;

        }

        private void flushToQueue_Click(object sender, RoutedEventArgs e)
        {
            foreach (var simulatorData in simulatorDataList){
                queue.Enqueue(simulatorData.toKinectData());
            }
        }


    }
}