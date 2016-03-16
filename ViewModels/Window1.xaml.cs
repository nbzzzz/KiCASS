//some code from 
//www.stackoverflow.com/questions/23168068/need-simple-working-example-of-setting-wpf-mvvm-combobox-itemssource-based-on-se
/*
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LaptopOrchestra.Kinect.ViewModel
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            //Set the data context of the window
            DataContext = new TestVM();
        }
    }

    public class TestVM : INotifyPropertyChanged
    {

        #region Class attributes

        protected static string[] firstComboValues = new string[] { "Choice_1", "Choice_2" };

        protected static string[][] secondComboValues =
            new string[][] {
                new string[] { "value_1_1", "value_1_2", "value_1_3" },
                new string[] { "value_2_1", "value_2_2", "value_2_3" }
            };


        #endregion

        #region Public Properties

        #region FirstSelectedValue

        protected string m_FirstSelectedValue;

        /// <summary>
        ///  
        /// </summary>
        public string FirstSelectedValue
        {
            get { return m_FirstSelectedValue; }
            set
            {
                if (m_FirstSelectedValue != value)
                {
                    m_FirstSelectedValue = value;
                    UpdateSecondComboValues();
                    NotifyPropertyChanged("FirstSelectedValue");
                }
            }
        }

        #endregion

        #region SecondSelectedValue

        protected string m_SecondSelectedValue;

        /// <summary>
        ///  
        /// </summary>
        public string SecondSelectedValue
        {
            get { return m_SecondSelectedValue; }
            set
            {
                if (m_SecondSelectedValue != value)
                {
                    m_SecondSelectedValue = value;
                    NotifyPropertyChanged("SecondSelectedValue");
                }
            }
        }

        #endregion

        #region FirstComboValues

        protected ObservableCollection<string> m_FirstComboValues;

        /// <summary>
        ///  
        /// </summary>
        public ObservableCollection<string> FirstComboValues
        {
            get { return m_FirstComboValues; }
            set
            {
                if (m_FirstComboValues != value)
                {
                    m_FirstComboValues = value;
                    NotifyPropertyChanged("FirstComboValues");
                }
            }
        }

        #endregion

        #region SecondComboValues

        protected ObservableCollection<string> m_SecondComboValues;

        /// <summary>
        ///  
        /// </summary>
        public ObservableCollection<string> SecondComboValues
        {
            get { return m_SecondComboValues; }
            set
            {
                if (m_SecondComboValues != value)
                {
                    m_SecondComboValues = value;
                    NotifyPropertyChanged("SecondComboValues");
                }
            }
        }

        #endregion

        #endregion

        public TestVM()
        {
            FirstComboValues = new ObservableCollection<string>(firstComboValues);
        }

        /// <summary>
        /// Update the collection of values for the second combo box
        /// </summary>
        protected void UpdateSecondComboValues()
        {
            int firstComboChoice;
            for (firstComboChoice = 0; firstComboChoice < firstComboValues.Length; firstComboChoice++)
            {
                if (firstComboValues[firstComboChoice] == FirstSelectedValue)
                    break;
            }


            if (firstComboChoice == firstComboValues.Length)// just in case of a bug
                SecondComboValues = null;
            else
                SecondComboValues = new ObservableCollection<string>(secondComboValues[firstComboChoice]);

        }


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}


*/