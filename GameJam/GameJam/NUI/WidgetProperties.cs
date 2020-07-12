using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace GameJam.NUI
{
    interface IWidgetProperty
    {
        bool IsCorrectType(Type type);
    }

    public abstract class WidgetProperty<T> : IWidgetProperty
    {
        public abstract T Value
        {
            get;
        }

        public bool IsCorrectType(Type type)
        {
            return type.IsEquivalentTo(typeof(T));
        }
    }

    public class FixedValue<T> : WidgetProperty<T>
    {
        public FixedValue(T value)
        {
            _value = value;
        }

        private T _value;
        public override T Value => _value;
    }

    public class RelativeValue<T> : WidgetProperty<T> where T : IComparable<T>
    {
        public RelativeValue(Widget widget, string prop, float percentage) {
            PropertiesReference = new WeakReference<WidgetProperties>(widget.Properties);
            PropertyName = prop;
            Percentage = percentage;
        }

        public string PropertyName
        {
            get;
            private set;
        }
        public float Percentage
        {
            get;
            private set;
        }
        public WeakReference<WidgetProperties> PropertiesReference
        {
            get;
            private set;
        }

        public T ComputeRelativeValue()
        {
            WidgetProperties properties;
            PropertiesReference.TryGetTarget(out properties);
            if(properties == null)
            {
                throw new Exception("Invalid properties reference.");
            }
            return properties.GetProperty<T>(PropertyName).Value * (dynamic)Percentage;
        }

        public override T Value => ComputeRelativeValue();
    }

    public class AspectRatioValue<T> : RelativeValue<T> where T : IComparable<T>
    {
        public enum ComputeThisFromOther
        {
            Width,
            Height
        }

        public AspectRatioValue(Widget widget, float aspect, ComputeThisFromOther computeFromOther) // aspect = w/h
            : base(widget, computeFromOther == ComputeThisFromOther.Height ? "width" : "height", computeFromOther == ComputeThisFromOther.Height ? 1 / aspect : aspect)
        {
        }
    }

    public class ComputedValue<T> : WidgetProperty<T> where T : IComparable<T>
    {
        Func<T> _computer;

        public ComputedValue(Func<T> computer)
        {
            if(computer == null)
            {
                throw new ArgumentNullException();
            }

            _computer = computer;
        }

        public override T Value => _computer();
    }

    public class WidgetProperties
    {
        private struct WidgetKeyPair
        {
            public readonly Widget Widget;
            public readonly string Key;

            public WidgetKeyPair(Widget widget, string key)
            {
                Widget = widget;
                Key = key;
            }
        }

        private Dictionary<string, IWidgetProperty> _properties = new Dictionary<string, IWidgetProperty>();
        private Dictionary<string, string[]> _proxyProperties = new Dictionary<string, string[]>();
        private Dictionary<string, WidgetKeyPair> _childPassThroughProperties = new Dictionary<string, WidgetKeyPair>();
        private Dictionary<string, Action> _propertyReactions = new Dictionary<string, Action>();
        private Dictionary<string, bool> _readOnlyProperties = new Dictionary<string, bool>();
        public bool Locked
        {
            get;
            private set;
        } = false;

        public bool ComputePropertiesOnSet
        {
            get;
            internal set;
        } = true;

        public Action ComputePropertiesDelegate
        {
            get;
            private set;
        }

        public WidgetProperties(Action computePropertiesDelegate = null)
        {
            ComputePropertiesDelegate = computePropertiesDelegate;
        }

        public WidgetProperty<T> GetProperty<T>(string key)
        {
            if(_childPassThroughProperties.ContainsKey(key))
            {
                return GetPropertyThroughChildPassThrough<T>(key);
            }
            if (!_properties[key].IsCorrectType(typeof(T)))
            {
                throw new Exception("Attempted to access property with incorrect type.");
            }
            return (WidgetProperty<T>)_properties[key];
        }
        public void SetProperty<T>(string key, WidgetProperty<T> prop)
        {
            SetProperty<T>(key, prop, false);
        }
        public void SetProperty<T>(string key, WidgetProperty<T> prop, bool setAsReadOnly)
        {
            if (_proxyProperties.ContainsKey(key))
            {
                SetPropertiesThroughProxy(key, prop);
                return;
            }
            if(_childPassThroughProperties.ContainsKey(key))
            {
                SetPropertyThroughChildPassThrough(key, prop);
                return;
            }

            if (_properties.ContainsKey(key) && !_properties[key].IsCorrectType(typeof(T)))
            {
                throw new Exception("Attempted to set property with incorrect type.");
            }
            if (Locked && !_properties.ContainsKey(key))
            {
                throw new Exception("Attempted to add new property of locked widget properties.");
            }
            if (_readOnlyProperties.ContainsKey(key) && _readOnlyProperties[key])
            {
                throw new Exception("Attempted to set widget property marked as read-only.");
            }
            _properties[key] = prop;
            if (setAsReadOnly)
            {
                _readOnlyProperties.Add(key, true);
            }
            if (ComputePropertiesOnSet)
            {
                ComputePropertiesDelegate?.Invoke();
            }
        }

        public void SetPropertyAsProxy(string key, string[] props)
        {
            if (Locked)
            {
                throw new Exception("Attempted to add new property of locked widget properties.");
            }
            _proxyProperties.Add(key, props);
        }
        private void SetPropertiesThroughProxy<T>(string proxyKey, WidgetProperty<T> prop)
        {
            foreach (string key in _proxyProperties[proxyKey])
            {
                SetProperty(key, prop);
            }
        }

        public void SetPropertyAsChildPassThrough<T>(string key, Widget widget)
        {
            SetPropertyAsChildPassThrough<T>(key, widget, key);
        }
        public void SetPropertyAsChildPassThrough<T>(string key, Widget widget, string childKey)
        {
            if(Locked)
            {
                throw new Exception("Attempted to add new property of locked widget properties.");
            }
            _childPassThroughProperties.Add(key, new WidgetKeyPair(widget, childKey));
        }
        private WidgetProperty<T> GetPropertyThroughChildPassThrough<T>(string key)
        {
            return _childPassThroughProperties[key].Widget.Properties.GetProperty<T>(_childPassThroughProperties[key].Key);
        }
        private void SetPropertyThroughChildPassThrough<T>(string key, WidgetProperty<T> prop)
        {
            _childPassThroughProperties[key].Widget.Properties.SetProperty(_childPassThroughProperties[key].Key, prop);
        }

        public void SetPropertyReaction(string key, Action action)
        {
            if(!_properties.ContainsKey(key))
            {
                throw new Exception("Attempted to add new reaction to property that doesn't exist.");
            }
            _propertyReactions[key] = action;
        }

        internal void Lock()
        {
            Locked = true;
        }
    }
}
