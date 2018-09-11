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
using System.Windows.Shapes;
using MemoryGameExample.ViewModels;
using System.ComponentModel;

namespace MemoryGameExample
{
    /// <summary>
    /// Interaction logic for HighscoreDialog.xaml
    /// </summary>
    public partial class HighscoreDialog : Window
    {
        public HighscoreDialog()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            this.DataContext = new HighscoreDialogViewModel(this);
        }

      
    }
}
