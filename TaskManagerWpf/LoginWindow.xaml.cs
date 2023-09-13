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
using TaskManagerWpf.Models;

namespace TaskManagerWpf
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5235");
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

            var response = await client.PostAsJsonAsync<LoginViewModel>("auth", new LoginViewModel()
            {
                UserName = tb_username.Text,
                Password = tb_password.Password
            });
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = await response.Content.ReadAsAsync<TokenModel>();
                token.Expiration = token.Expiration.ToLocalTime();
                MainWindow mw = new MainWindow(token);
                mw.ShowDialog();
                //mw.Show();
                //this.Close();
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RegisterWindow rw = new RegisterWindow();
            rw.ShowDialog();
        }
    }

    internal class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
