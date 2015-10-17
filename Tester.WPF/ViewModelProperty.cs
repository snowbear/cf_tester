using System;
using Tester.Models;

namespace Tester
{
	public class ViewModelProperty<T> : ModelBase
	{
		public const string ValuePropertyName = "Value";

		private Action _onChanged;

		private T _value;

		public T Value
		{
			get { return _value; }
			set
			{
				if (Equals(value, _value)) return;
				_value = value;
				OnPropertyChanged(ValuePropertyName);
				if (_onChanged != null)
					_onChanged();
			}
		}

		public void OnChanged(Action action)
		{
			_onChanged += action;
		}

		public void BindTo(ViewModelProperty<T> other)
		{
			Value = other.Value;
			other.OnChanged(() => Value = other.Value);
			OnChanged(() => other.Value = Value);
		}
	}
}