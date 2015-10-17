using System.Windows.Media;
using Tester.Models;

namespace Tester.ViewModels
{
	public class StateViewModel : ModelBase
	{
		private Brush _color;

		public Brush Color
		{
			get { return _color; }
			private set
			{
				if (Equals(value, _color)) return;
				_color = value;
				OnPropertyChanged("Color");
			}
		}

        private StateType _stateType;
        public StateType StateType
        {
            get { return _stateType; }
            set
            {
                if (Equals(_stateType, value)) return;
                _stateType = value;
                OnPropertyChanged("StateType");
            }
        }

		public StateViewModel()
		{
			SetSuccess();
		}

		public void SetSuccess()
		{
			Color = Brushes.LimeGreen;
			StateType = StateType.Success;
		}

		public void SetError()
		{
			Color = Brushes.Red;
			StateType = StateType.Error;
		}

		public void SetInconclusive()
		{
			Color = Brushes.Yellow;
			StateType = StateType.Inconclusive;
		}
	}
}