using MahApps.Metro.Controls;
using System.Windows;

namespace Beast.MapMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for NewDungeonDialog.xaml
    /// </summary>
    public partial class NewDungeonDialog : MetroWindow
    {
        public NewDungeonDialog()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
