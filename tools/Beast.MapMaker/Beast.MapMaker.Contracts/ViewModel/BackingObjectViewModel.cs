using GalaSoft.MvvmLight;
using System;
using System.Linq.Expressions;

namespace Beast.MapMaker.ViewModel
{
    public abstract class BackingObjectViewModel<TObject> : ViewModelBase
    {
        private TObject _backingObject;
        private const string PropertyNameBackingObject = "BackingObject";
        public TObject BackingObject
        {
            get { return _backingObject; }
            set
            {
                _backingObject = value;

                RaisePropertyChanged(PropertyNameBackingObject);
                OnBackingObjectChanged();
            }
        }

        protected BackingObjectViewModel()
        {
        }

        protected BackingObjectViewModel(TObject obj)
        {
            BackingObject = obj;
        }

        protected virtual void OnBackingObjectChanged()
        {
        }

        protected T GetPropertyValue<T>(Expression<Func<TObject, T>> expression)
        {
            if (BackingObject == null)
                return default(T);

            var property = ReflectionExtensions.GetProperty(expression);
            if (property == null)
                return default(T);

            return (T)property.GetValue(BackingObject);
        }

        protected void SetPropertyValue<T>(Expression<Func<TObject, T>> expression, T value)
        {
            if (BackingObject == null)
                return;

            var property = ReflectionExtensions.GetProperty(expression);
            if (property == null)
                return;

            property.SetValue(BackingObject, value);
        }
    }
}
