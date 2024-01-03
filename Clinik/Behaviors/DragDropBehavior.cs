using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Clinik.Behaviors
{
    public static class DragDropBehavior
    {
        public static readonly DependencyProperty DragCommandProperty =
            DependencyProperty.RegisterAttached("DragCommand", typeof(ICommand), typeof(DragDropBehavior), new PropertyMetadata(null));

        public static ICommand GetDragCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DragCommandProperty);
        }

        public static void SetDragCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DragCommandProperty, value);
        }

        public static void SetIsDragSource(UIElement element, bool value)
        {
            element.PreviewMouseLeftButtonDown += Element_PreviewMouseLeftButtonDown;
        }

        private static void Element_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as UIElement;
            if (element != null)
            {
                DataObject data = new DataObject(typeof(UIElement), element);
                DragDrop.DoDragDrop(element, data, DragDropEffects.Move);
            }
        }
    }
}
