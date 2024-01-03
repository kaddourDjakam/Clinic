using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Clinik.ViewModel.ServicesViewModel
{
    class CustomMessageBoxViewModel : ViewModelBase
    {
		private string _Message; 

        public string Message
        {
			get { return _Message; }
			set { _Message = value; OnPropertyChanged(nameof(Message)); }
		}
        private Brush _BackgroundColor;

        public Brush BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; OnPropertyChanged(nameof(BackgroundColor)); }
        }

        public CustomMessageBoxViewModel(string message, Brush backgroundColor)
        {
            Message = message;
            BackgroundColor = backgroundColor;
        }

    }
}
