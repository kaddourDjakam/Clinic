using Clinik.View.ServicesView;
using Clinik.ViewModel.ServicesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Clinik.Services
{
    public static class MessageService
    {
        public static void ShowSuccessMessage(string message, Window parentWindow)
        {
            ShowMessageBox(message, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A3E4D7")), parentWindow);
        }

        public static void ShowErrorMessage(string message, Window parentWindow)
        {
            
            ShowMessageBox(message, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF665C")), parentWindow);
        }

        private static void ShowMessageBox(string message, Brush backgroundColor, Window parentWindow)
        {
            var messageBox = new CustomMessageBox()
            {
                DataContext = new CustomMessageBoxViewModel(message, backgroundColor),
            };

            var popup = new Popup
            {
                Child = messageBox,
                IsOpen = true,
                StaysOpen = false
            };

            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            var x = screenWidth - messageBox.ActualWidth - 10; // Adjust 10 for margin
            var y = screenHeight - messageBox.ActualHeight - 10; // Adjust 10 for margin

            popup.HorizontalOffset = x;
            popup.VerticalOffset = y;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += (sender, args) =>
            {
                popup.IsOpen = false;
                timer.Stop();
            };
            timer.Start();
        }
    }
}
