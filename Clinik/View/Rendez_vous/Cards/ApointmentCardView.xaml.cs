﻿using Clinik.ViewModel.Rendez_vous.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clinik.View.Rendez_vous.Cards
{
    /// <summary>
    /// Interaction logic for ApointmentCardView.xaml
    /// </summary>
    public partial class ApointmentCardView : UserControl
    {
        private bool isDragging;
        private Point startPoint;
        public ApointmentCardView()
        {
            InitializeComponent();
        }


        public ScrollViewer ParentScrollViewer { get; set; }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
                isDragging = true;
                startPoint = e.GetPosition(null);
            
           
        }

        private void Border_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = startPoint - mousePos;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                     Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    // Start the drag-and-drop operation
                    StartDrag(sender as Border, e);
                }
            }
        }
        private void SecondScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                ApointmentCardViewModel viewModel = border.DataContext as ApointmentCardViewModel;

                if (viewModel != null)
                {
                    DragDrop.DoDragDrop(border, viewModel, DragDropEffects.Move);
                }
            }
        }
        private void StartDrag(Border border, MouseEventArgs e)
        {
            isDragging = false;

            DataObject data = new DataObject(typeof(ApointmentCardView), this);
            DragDrop.DoDragDrop(border, data, DragDropEffects.Move);
        }


    }
}
