using POD.Models;
using POD.Toolbox;
using POD.ViewModels;
using POD.Windows;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace POD
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Directory.CreateDirectory(Environment.CurrentDirectory + "/Data");
            Directory.CreateDirectory(Environment.CurrentDirectory + "/Data/Images");

            DataContext = new MainViewModel();

        }

        // Private Methods
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ToggleMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CTRL_PersonalObjectList.Height = this.ActualHeight - 76;
            CTRL_ItemInfoScrollViewer.Height = this.ActualHeight - 86;
            //CTRL_ItemImageList.Height = this.ActualHeight - 210;
            //CTRL_ActiveImageDisplay.MaxHeight = this.ActualHeight - 300;
            //CTRL_ActiveImageDisplay.MaxWidth = this.ActualWidth - 632;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(((sender as Image).DataContext as ItemImage).FullFilePath);
            }
            catch (Exception ex)
            {
                YesNoDialog question = new YesNoDialog(ex.Message + "\nUnlink?");
                question.ShowDialog();
                if (question.Answer == true)
                {
                    Configuration.MainModelRef.ActiveCard.ItemImages.Remove(((sender as Image).DataContext as ItemImage));
                }
            }
        }
    }
}
