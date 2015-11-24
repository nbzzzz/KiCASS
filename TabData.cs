using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LaptopOrchestra.Kinect
{
    public class TabData
    {

        private string _header;
        private int _height;
        private int _width;
        private int _itemListWidth;
        private ItemsControl _items;
        private Image _image;
        private Canvas _canvas;


        public TabData(string header, int height, int width, int itemListWidth, ItemsControl items, Image image, Canvas canvas)
        {

            _header = header;
            _height = height;
            _width = width;
            _itemListWidth = itemListWidth;
            _items = items;
            _image = image;
            _canvas = canvas;    

        }
        public TabData(string header, ItemsControl items, Image image, Canvas canvas)
        {
            _header = header;
            _height = 1080;
            _width = 1920;
            _itemListWidth = 200;
            _items = items;
            _image = image;
            _canvas = canvas;
        }

        public string Header
        {
            get { return _header; }
        }

    }

    public class TabList : ObservableCollection<TabData>
    {

        public TabList()
            : base()
        {  

        }

    }
}
