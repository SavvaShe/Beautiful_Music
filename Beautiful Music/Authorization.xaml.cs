using BeautifulMusic.Models;
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
using static BeautifulMusic.SqlDataAccess;

namespace BeautifulMusic
{
    /// <summary>
    /// Логика взаимодействия для UserControlAutoris.xaml
    /// </summary>
    public partial class Authorization : UserControl
    {


        public Authorization()
        {
            InitializeComponent();
            btnOut.Visibility = CurrentUser == null ? Visibility.Hidden : Visibility.Visible;
        }

        private void button_Enterback_Click(object sender, RoutedEventArgs e)
        {
            autorizgrid.Children.Clear();
            autorizgrid.Children.Add(new Enter());
        }

        public void Button_Enter_Click(object sender, RoutedEventArgs e)
        {
            string login = textboxlogin.Text.Trim();
            string pass = passbox.Password.Trim();

            if (login.Length < 5)
            {
                textboxlogin.ToolTip = "это поле введено не корректно!";
                textboxlogin.Background = Brushes.LightBlue;
            }
            if (pass.Length < 5)
            {
                { passbox.ToolTip = "это поле введено не корректно!"; passbox.Background = Brushes.LightBlue; }
                return;
            }
            UserModel user = new UserModel { Login = login, Password = pass };
            if (!IsUserByPaswordFound(user))
            {
                { passbox.ToolTip = "логин или пароль введено не корректно!"; passbox.Background = Brushes.LightBlue; }
                return;
            }
            CurrentUser = user;
            TrackModels.Clear();
            autorizgrid.Children.Clear();
            autorizgrid.Children.Add(new HelloUser());
        }

        private void button_Out_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = null;
            btnOut.Visibility = Visibility.Hidden;
            TrackModels.Clear();
        }
    }
}