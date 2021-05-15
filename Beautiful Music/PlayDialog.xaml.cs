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
    public partial class PlayDialog : UserControl
    {
       public PlayDialog()
        {
            InitializeComponent();
        }
        public void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            string name = textboxListName.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                PlaylistModel model = new PlaylistModel { Name = name, UserId = CurrentUser.Id };
                SaveNewPlayList(model);
                if (!Playlists.Any(x => x.Id == model.Id))
                    Playlists.Add(model);
                var p =  (MainWindow)((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)this.Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent;
                p.ShowPlaylist();
                PlayListGrid.Children.Clear();
                //PlayListGrid.Children.Add(new Playlist());
            }
        }

    }
}
