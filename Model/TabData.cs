using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LaptopOrchestra.Kinect
{
    public class TabData
    {
        #region properties

        private string _header;
        private int _height;
        private int _width;
        private int _itemListWidth;
        private List<String> _items;
        private Dictionary<JointType, bool> _displayFlags;
        private bool _active;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public TabData(string header, int height, int width, int itemListWidth, List<String> items, Dictionary<JointType, bool> displayFlags)
        {

            _header = header;
            _height = height;
            _width = width;
            _itemListWidth = itemListWidth;
            _items = items;
            _displayFlags = displayFlags;

        }
        public TabData(string header, List<String> items, Dictionary<JointType, bool> displayFlags, bool active)
        {
            _header = header;
            _height = 1080;
            _width = 1920;
            _itemListWidth = 200;
            _items = items;
            _displayFlags = displayFlags;
            _active = active;
        }

        #endregion

        #region functions

        public string Header
        {
            get { return _header; }
        }

        public List<String> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        public Dictionary<JointType,bool> displayFlags
        {
            get { return _displayFlags; }
            set { _displayFlags = value; }
        }
        
        public int Height
        {
            get
            {
                return _height;
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }
        }

        public int ItemListWidth
        {
            get
            {
                return _itemListWidth;
            }
        }

        public bool Active
        {
            get
            {
                return _active;
            }

            set
            {
                _active = value;
            }
        }

        #endregion
    }

    public class TabList : ObservableCollection<TabData>
    {
        private static ObservableCollection<TabData> _tabList;
        public TabList()            
        {
            _tabList = new ObservableCollection<TabData>();

        }
        public ObservableCollection<TabData> getTabs()
        {
            return this;
        }
        public int getIndex(TabData tabData)
        {
            return IndexOf(tabData);
        }        
    }
}
