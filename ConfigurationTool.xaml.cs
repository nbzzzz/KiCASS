using Microsoft.Kinect;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Input;
using System;
using LaptopOrchestra.Kinect;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LaptopOrchestra.Kinect
{
    public class ListJoint
    {        
        public JointType jointType { get; set; }
        public Boolean send { get; set; }
    }
    public class DataClass
    {
        private static ObservableCollection<ListJoint> _JointsCollection;

        public DataClass()
        {
            _JointsCollection = new ObservableCollection<ListJoint>();
        }

        public ObservableCollection<ListJoint> JointsCollection
        {
            get { return _JointsCollection; }
            set { _JointsCollection = value; }
        }

        public ObservableCollection<ListJoint> GetJoints()
        {
            var JointTypes = Enum.GetValues(typeof(JointType)).Cast<JointType>(); ;

            foreach (var jt in JointTypes)
            {

                JointType jointType = new JointType();
                jointType = jt;

                ListJoint listJoint = new ListJoint();
                listJoint.jointType = jointType;
                listJoint.send = false;
                JointsCollection.Add(listJoint);
            }

            return JointsCollection;
        }
    }

    public partial class ConfigurationTool : Window
    {
        // BEGIN: KinectProcessor vars
        Mode _mode = Mode.Color;

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;
        Queue<IDictionary<JointType, Joint>> queue;

        bool _displayBody = false;
        // END: KinectProcess vars

        public ICommand MoveRightCommand
        {
            get;
            set;
        }

        public ICommand MoveLeftCommand
        {
            get;
            set;
        }

        public ICommand MoveAllRightCommand
        {
            get;
            set;
        }

        public ICommand MoveAllLeftCommand
        {
            get;
            set;
        }

        public static DependencyProperty RightHeaderProperty =
            DependencyProperty.Register("RightHeader", typeof(string), typeof(ConfigurationTool));

        public string RightHeader
        {
            get { return (string)GetValue(RightHeaderProperty); }
            set { SetValue(RightHeaderProperty, value); }
        }

        public static DependencyProperty LeftHeaderProperty =
            DependencyProperty.Register("LeftHeader", typeof(string), typeof(ConfigurationTool));

        public string LeftHeader
        {
            get { return (string)GetValue(LeftHeaderProperty); }
            set { SetValue(LeftHeaderProperty, value); }
        }

        /// <summary>
        /// Default constructor-- set up RelayCommands.
        /// </summary>
        public ConfigurationTool(Queue<IDictionary<JointType, Joint>> queue) // TODO: remove arg and place into KinectProcessor
        {
            LeftHeader = "Joints";
            RightHeader = "OSC Joints";

            
            MoveRightCommand = new RelayCommand<ListJoint>((o) => OnMoveRight(o), (o) => o != null);
            MoveLeftCommand = new RelayCommand<ListJoint>((o) => OnMoveLeft(o), (o) => o != null);
            MoveAllRightCommand = new RelayCommand<ListCollectionView>((o) => OnMoveAllRight((ListCollectionView)o), (o) => ((ListCollectionView)o).Count > 0);
            MoveAllLeftCommand = new RelayCommand<ListCollectionView>((o) => OnMoveAllLeft((ListCollectionView)o), (o) => ((ListCollectionView)o).Count > 0);

            this.queue = queue;
            startKinect();
            InitializeComponent();
        }

        public ObservableCollection<ListJoint> getJointList()
        {
            ObjectDataProvider resource = (ObjectDataProvider)this.Resources["Joints"];

            return (ObservableCollection<ListJoint>)resource.Data;
            
        }

        /// <summary>
        /// Make this selected joint sent
        /// </summary>
        private void OnMoveRight(ListJoint joint)
        {
            joint.send = true;
            RefreshViews();
        }

        /// <summary>
        /// Make this selected joint not send.
        /// </summary>
        private void OnMoveLeft(ListJoint joint)
        {
            joint.send = false;
            RefreshViews();
        }

        /// <summary>
        /// Make all Joints send
        /// </summary>
        private void OnMoveAllRight(ListCollectionView joints)
        {
            foreach (ListJoint j in joints.SourceCollection)
                j.send = true;
            RefreshViews();
        }

        /// <summary>
        /// Make all joints not send
        /// </summary>
        private void OnMoveAllLeft(ListCollectionView joints)
        {
            foreach (ListJoint j in joints.SourceCollection)
                j.send = false;
            RefreshViews();
        }

        /// <summary>
        /// Filters out any non sending joints
        /// </summary>
        private void sendFilter(object sender, FilterEventArgs e)
        {
            ListJoint joint = e.Item as ListJoint;
            e.Accepted = joint.send == true;
        }

        /// <summary>
        /// Filter to list all joints
        /// </summary>
        private void listFilter(object sender, FilterEventArgs e)
        {
            ListJoint joint = e.Item as ListJoint;
            e.Accepted = joint.send == false;
        }

        /// <summary>
        /// Refresh the collection view sources.
        /// </summary>
        private void RefreshViews()
        {
            foreach (object resource in Resources.Values)
            {
                CollectionViewSource cvs = resource as CollectionViewSource;
                if (cvs != null)
                    cvs.View.Refresh();
            }
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
                        if (body != null)
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
                                    canvas.DrawSkeleton(body);
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