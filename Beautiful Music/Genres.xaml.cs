using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
    public partial class Genres : UserControl
    {
        private List<string> genres = new List<string>();
        public Genres()
        {
            InitializeComponent();

            var genres = GetGenres();
            foreach (var item in genres)
            {
                GenresListView.Items.Add(item.Name);
            }
        }
        private void button_Genres_Click(object sender, RoutedEventArgs e)
        {
            Genresgrid.Children.Clear();
            Genresgrid.Children.Add(new Author());
        }
    }
}
