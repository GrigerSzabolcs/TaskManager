using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace TaskManagerWpf
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tb_password.Password != tb_password2.Password)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else if(tb_username.Text == "" || tb_password.Password == "" || tb_firstname.Text == "" || tb_lastname.Text == "" || tb_email.Text == "")
            {
                MessageBox.Show("Please fill out all the fields");
            }
            else
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5235");
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
                var response = await client.PutAsJsonAsync<RegisterViewModel>("auth", new RegisterViewModel()
                {
                    UserName = tb_username.Text,
                    Password = tb_password.Password,
                    FirstName = tb_firstname.Text,
                    LastName = tb_lastname.Text,
                    Email = tb_email.Text
                });
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Registration succesful", "Info", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    this.DialogResult = true;
                }
            }
        }
    }

    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

    }
}
