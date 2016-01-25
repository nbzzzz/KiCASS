using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect.Input;

namespace LaptopOrchestra.Kinect
{
	public class ConfigFlags
	{
		private Dictionary<JointType, bool> _configFlags;
        private Dictionary<HandType, bool> _handStateFlags;
        private List<Tuple<JointType, JointType>> _distanceFlags;
		private List<Tuple<JointType, JointType>> _vectorFlags;


		public Dictionary<JointType, bool> JointFlags
		{
			get { return _configFlags; }
			set { _configFlags = value; }
		}

		public List<Tuple<JointType, JointType>> DistanceFlags
		{
			get { return _distanceFlags; }
			set { _distanceFlags = value; }
		}

		public List<Tuple<JointType, JointType>> VectorFlags
		{
			get { return _vectorFlags; }
			set { _vectorFlags = value; }
		}

        public Dictionary<HandType, bool> HandStateFlag
		{
			get { return _handStateFlags; }
			set { _handStateFlags = value; }
		}

		public ConfigFlags()
		{
			_configFlags = InitJointFlags(_configFlags);
			_distanceFlags = new List<Tuple<JointType, JointType>>();
			_vectorFlags = new List<Tuple<JointType, JointType>>();
		    _handStateFlags = InitHandFlags(_handStateFlags); ; 
        }

		private Dictionary<JointType, bool> InitJointFlags(Dictionary<JointType, bool> flags)
		{
			var jointTypes = Enum.GetValues(typeof(JointType));

			flags = new Dictionary<JointType, bool>();

			foreach (JointType jt in jointTypes)
			{
				flags[jt] = false;
			}

			return flags;
		}

        private Dictionary<HandType, bool> InitHandFlags(Dictionary<HandType, bool> flags)
        {
            var handTypes = Enum.GetValues(typeof(JointType));

            flags = new Dictionary<HandType, bool>();

            foreach (HandType ht in handTypes)
            {
                flags[ht] = false;
            }

            return flags;
        }

        public bool IfAnyConfig()
		{
			return _configFlags.Any(x => x.Value == true) || _handStateFlags.Any(x => x.Value == true) || _distanceFlags.Any() || _vectorFlags.Any();
        }
	}
}
