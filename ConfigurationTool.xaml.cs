using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Kinect;

namespace LaptopOrchestra.Kinect
{

    public partial class ConfigurationTool : Window
    {
        // BEGIN: KinectProcessor vars
        Mode _mode = Mode.Color;

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;
        Queue<IDictionary<JointType, Joint>> queue;

        Dictionary<JointType, bool> configurationFlags;

        bool _displayBody = false;
        // END: KinectProcess vars

        /// <summary>
        /// Default constructor-- set up RelayCommands.
        /// </summary>
        public ConfigurationTool(Queue<IDictionary<JointType, Joint>> queue, Dictionary<JointType, bool> configurationFlags) // TODO: remove arg and place into KinectProcessor
        {
            InitializeComponent();
            this.configurationFlags = configurationFlags;

            var jointTypes = Enum.GetValues(typeof(JointType));

            foreach (JointType jt in jointTypes) {
                lvJoints.Items.Add(jt);
                configurationFlags[jt] = false;
            }
            
            this.queue = queue;
            startKinect();

        }

        private void lvJoints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes)
            {
                configurationFlags[jt] = false;
            }

            foreach ( var item in lvJoints.SelectedItems)
            {
                JointType jt = (JointType)Enum.Parse(typeof(JointType), item.ToString(), true);
                configurationFlags[jt] = true;
            }

        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes)
            {
                configurationFlags[jt] = true;
            }
            lvJoints.SelectAll();
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes)
            {
                configurationFlags[jt] = false;
            }
            lvJoints.UnselectAll();
        }



        // TODO: Start of KinectProcessor -- must be moved into its own class once GUI can be sorted

        #region Event handlers

        private void startKinect()
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                _mode = Mode.Color;

                _displayBody = true;

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Color)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Depth
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Depth)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Infrared
            using (var frame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Infrared)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    canvas.Children.Clear();

                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null && body.IsTracked)
                        {
                            IDictionary<JointType, Joint> newJoint = new Dictionary<JointType, Joint>();

                            foreach (var joint in body.Joints)
                            {
                                newJoint.Add(joint.Value.JointType, joint.Value);
                            }

                            this.queue.Enqueue(newJoint);

                            if (body.IsTracked)
                            {
                                // Draw skeleton.
                                if (_displayBody)
                                {
                                    canvas.DrawSkeleton(body, configurationFlags);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Color;
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Depth;
        }

        private void Infrared_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Infrared;
        }

        private void Body_Click(object sender, RoutedEventArgs e)
        {
            _displayBody = !_displayBody;
        }

        #endregion

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var ip = IP.Text;
            try {
                var port = int.Parse(Port.Text);
                UDP.ConfigureIpAndPort(ip, port);
            }
            catch
            {

            }
        }
    }

    public enum Mode
    {
        Color,
        Depth,
        Infrared
    }


    public class RelayCommand<T> : ICommand
    {
        #region Fields

        readonly Action<T> _execute = null;
        readonly Predicate<T> _canExecute = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
        /// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        ///<summary>
        ///Defines the method that determines whether the command can execute in its current state.
        ///</summary>
        ///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        ///<returns>
        ///true if this command can be executed; otherwise, false.
        ///</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        ///<summary>
        ///Occurs when changes occur that affect whether or not the command should execute.
        ///</summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        ///<summary>
        ///Defines the method to be called when the command is invoked.
        ///</summary>
        ///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion
    }

}
