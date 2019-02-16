using Merge_Tool.Resource;
using System.ComponentModel;
using System.Collections.Generic;            //INotifyPropertyChanged

namespace Merge_Tool.Models
{
    public class ConfigFile: NotifyObject
    {
        private string _configFileName;
        public string ConfigFileName
        {
            get { return _configFileName; }
            set
            {
                _configFileName = value;
                RaisePropertyChanged("ConfigFileName");
            }
        }


        private string _configFileDate;
        public string ConfigFileDate
        {
            get { return _configFileDate; }
            set
            {
                _configFileDate = value;
                RaisePropertyChanged("ConfigFileDate");
            }
        }

        private Model _model;
        public Model Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged("Model");
            }
        }

        private Diagcode []_diagcode;
        public Diagcode []Diagcode
        {
            get { return _diagcode; }
            set
            {
                _diagcode = value;
                RaisePropertyChanged("Diagcode");
            }
        }

    }

    public class Model
    {
        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
            }
        }
    }

    public class Diagcode
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        private string _isSwitch;
        public string IsSwitch
        {
            get { return _isSwitch; }
            set
            {
                _isSwitch = value;
            }
        }

        private Location _location;
        public Location Location
        {
            get { return _location; }
            set
            {
                _location = value;
            }
        }

        private Step[] _step;
        public Step[] Step
        {
            get { return _step; }
            set
            {
                _step = value;
            }
        }

    }

    public class Location
    {
        private string _x;
        public string X
        {
            get { return _x; }
            set
            {
                _x = value;
            }
        }

        private string _y;
        public string Y
        {
            get { return _y; }
            set
            {
                _y = value;
            }
        }
    }

    public class Step
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
            }
        }

        private string _startWaitSecond;
        public string StartWaitSecond
        {
            get { return _startWaitSecond; }
            set
            {
                _startWaitSecond = value;
            }
        }

        private string _measurementSecond;
        public string MeasurementSecond
        {
            get { return _measurementSecond; }
            set
            {
                _measurementSecond = value;
            }
        }

        private Standerd _standerd;
        public Standerd Standerd
        {
            get { return _standerd; }
            set
            {
                _standerd = value;
            }
        }
    }

    public class Standerd
    {
        private string _min;
        public string Min
        {
            get { return _min; }
            set
            {
                _min = value;
            }
        }

        private string _max;
        public string Max
        {
            get { return _max; }
            set
            {
                _max = value;
            }
        }

        private string _variable;
        public string Variable
        {
            get { return _variable; }
            set
            {
                _variable = value;
            }
        }

        private string _isBeChange;
        public string IsBeChange
        {
            get { return _isBeChange; }
            set
            {
                _isBeChange = value;
            }
        }

    }
}
