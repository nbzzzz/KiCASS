using Microsoft.Kinect;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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
        public ConfigurationTool()
        {
            LeftHeader = "Joints";
            RightHeader = "OSC Joints";

            
            MoveRightCommand = new RelayCommand<ListJoint>((o) => OnMoveRight(o), (o) => o != null);
            MoveLeftCommand = new RelayCommand<ListJoint>((o) => OnMoveLeft(o), (o) => o != null);
            MoveAllRightCommand = new RelayCommand<ListCollectionView>((o) => OnMoveAllRight((ListCollectionView)o), (o) => ((ListCollectionView)o).Count > 0);
            MoveAllLeftCommand = new RelayCommand<ListCollectionView>((o) => OnMoveAllLeft((ListCollectionView)o), (o) => ((ListCollectionView)o).Count > 0);
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
