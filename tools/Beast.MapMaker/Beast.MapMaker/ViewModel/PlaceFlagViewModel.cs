using Beast.Mapping;
using GalaSoft.MvvmLight;

namespace Beast.MapMaker.ViewModel
{
    public class PlaceFlagViewModel : BackingObjectViewModel<IPlace>
    {
        private int _value;
        private const string PropertyNameValue = "Value";
        public int Value    
        {
            get { return _value; }
            set
            {
                if (value == _value)
                    return;

                _value = value;

                RaisePropertyChanged(PropertyNameValue);
            }
        }

        private string _name;
        private const string PropertyNameName = "Name";
        public string Name  
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;

                _name = value;

                RaisePropertyChanged(PropertyNameName);
            }
        }

        private const string PropertyNameIsSelected = "IsSelected";
        public bool IsSelected  
        {
            get
            {
                if (BackingObject != null)
                    return BackingObject.HasFlag(Value);
                return false;
            }
            set
            {
                if (BackingObject != null)
                {
                    if (value)
                        BackingObject.Flags |= Value;
                    else
                        BackingObject.Flags &= ~Value;
                }

                RaisePropertyChanged(PropertyNameIsSelected);
            }
        }

        protected override void OnBackingObjectChanged()
        {
            RaisePropertyChanged(PropertyNameIsSelected);
        }
    }
}
