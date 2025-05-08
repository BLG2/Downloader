using System.Windows;
using System.Windows.Input;

namespace Downloader.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region DRAG/DROP Application
        private void MoveWindowOnHold(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
                {
                    this.Opacity = 0.7;
                    this.DragMove();
                }
                this.Opacity = 1;
            }
            catch { }
        }
        #endregion
    }
}
