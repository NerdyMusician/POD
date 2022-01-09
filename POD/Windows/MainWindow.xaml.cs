using POD.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace POD
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            CTRL_ItemInfoScrollViewer.Height = this.ActualHeight - 160;
            CTRL_ItemImageList.Height = this.ActualHeight - 210;
            CTRL_ActiveImageDisplay.MaxHeight = this.ActualHeight - 300;
            CTRL_ActiveImageDisplay.MaxWidth = this.ActualWidth - 632;
        }
    }
}
