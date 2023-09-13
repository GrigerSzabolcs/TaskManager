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
using TaskManagerWpf.Models;

namespace TaskManagerWpf
{
    /// <summary>
    /// Interaction logic for ActivityCreatorWindow.xaml
    /// </summary>
    public partial class ActivityCreatorWindow : Window
    {
        public Activity Activity { get => (this.DataContext as Activity); }
        public ActivityCreatorWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Activity.Title==null)
                this.DialogResult = false;
            else
                this.DialogResult = true;
        }

    }
}
