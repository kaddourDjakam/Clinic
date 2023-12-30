
using Clinik.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clinik.ViewModel.textbox_changed
{
    public static class TextBoxChangedCmd
    {

        public static readonly DependencyProperty TextChangedCommandProperty =
        DependencyProperty.RegisterAttached("TextChangedCommand", typeof(RelayCommand), typeof(TextBoxChangedCmd), new PropertyMetadata(null, OnTextChangedCommandPropertyChanged));

        public static readonly DependencyProperty EnterKeyDownCommandProperty =
            DependencyProperty.RegisterAttached("EnterKeyDownCommand", typeof(RelayCommand), typeof(TextBoxChangedCmd), new PropertyMetadata(null, OnEnterKeyDownCommandPropertyChanged));

        public static RelayCommand GetTextChangedCommand(DependencyObject obj)
        {
            return (RelayCommand)obj.GetValue(TextChangedCommandProperty);
        }

        public static void SetTextChangedCommand(DependencyObject obj, RelayCommand value)
        {
            obj.SetValue(TextChangedCommandProperty, value);
        }

        public static RelayCommand GetEnterKeyDownCommand(DependencyObject obj)
        {
            return (RelayCommand)obj.GetValue(EnterKeyDownCommandProperty);
        }

        public static void SetEnterKeyDownCommand(DependencyObject obj, RelayCommand value)
        {
            obj.SetValue(EnterKeyDownCommandProperty, value);
        }

        private static void OnTextChangedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if (e.NewValue is RelayCommand command)
                {
                    textBox.TextChanged += (sender, args) =>
                    {
                        command.Execute(null);
                    };
                }
                else if (e.OldValue is RelayCommand oldCommand)
                {
                    textBox.TextChanged -= (sender, args) =>
                    {
                        oldCommand.Execute(null);
                    };
                }
            }
        }

        private static void OnEnterKeyDownCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if (e.NewValue is RelayCommand command)
                {
                    textBox.PreviewKeyDown += (sender, args) =>
                    {
                        if (args.Key == Key.Enter)
                        {
                            command.Execute(null);
                            args.Handled = true;
                        }
                    };
                }
                else if (e.OldValue is RelayCommand oldCommand)
                {
                    textBox.PreviewKeyDown -= (sender, args) =>
                    {
                        if (args.Key == Key.Enter)
                        {
                            oldCommand.Execute(null);
                            args.Handled = true;
                        }
                    };
                }
            }
        }

    }
    
}
