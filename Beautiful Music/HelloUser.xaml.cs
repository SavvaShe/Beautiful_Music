using System.Windows.Controls;
using static BeautifulMusic.SqlDataAccess;

namespace BeautifulMusic
{
    public partial class HelloUser : UserControl
    {
        public HelloUser()
        {
            InitializeComponent();
            tbUser.Text = $"Привет {CurrentUser.Login}";
        }
    }
}
