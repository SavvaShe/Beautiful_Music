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
    /// Логика взаимодействия для UserControl6.xaml
    /// </summary>
    public partial class Enter : UserControl
    {
       public Enter()
        {
            InitializeComponent();
            btnOut.Visibility = CurrentUser == null ? Visibility.Hidden : Visibility.Visible;
        }
        public void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = textboxlogin.Text.Trim();
            string pass = passbox.Password.Trim();
            string pass_2 = passbox_2.Password.Trim();
            string email = textboxemail.Text.ToLower().Trim();

            if(login.Length<5)
            { textboxlogin.ToolTip = "это поле введено не корректно!"; textboxlogin.Background = Brushes.LightBlue; }
            else if(pass.Length<5)
            { passbox.ToolTip = "это поле введено не корректно!"; passbox.Background = Brushes.LightBlue; }
            else if (pass!=pass_2)
            { passbox_2.ToolTip = "это поле введено не корректно!"; passbox_2.Background = Brushes.LightBlue; }
            else if (email.Length<5 || !email.Contains("@") || !email.Contains("."))
            { textboxemail.ToolTip = "это поле введено не корректно!"; textboxemail.Background = Brushes.LightBlue; }
            else
            {
                try
                {
                    textboxlogin.ToolTip = "";
                    textboxlogin.Background = Brushes.Transparent;
                    passbox.ToolTip = "";
                    passbox.Background = Brushes.Transparent;
                    passbox_2.ToolTip = "";
                    passbox_2.Background = Brushes.Transparent;
                    textboxemail.ToolTip = "";
                    textboxemail.Background = Brushes.Transparent;
                    var user = new UserModel {Login=login ,Password= pass,Email= email,RoleId=1};
                    AddUser(user);
                    Entergrid.Children.Clear();
                    Entergrid.Children.Add(new Authorization());
                }
                catch (DuplicateWaitObjectException ex)
                {
                    textboxemail.ToolTip = ex.Message;
                     textboxemail.Background = Brushes.LightBlue;
                    return;
                }
                catch{}
            }
        }
        private void button_Autorizback_Click(object sender, RoutedEventArgs e)
        {

            Entergrid.Children.Clear();
            Entergrid.Children.Add(new Authorization());
        }

        private void button_Out_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = null;
            btnOut.Visibility = Visibility.Hidden;
            TrackModels.Clear();
        }
    }
}
