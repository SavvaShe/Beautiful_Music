using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static BeautifulMusic.SqlDataAccess;

namespace BeautifulMusic
{
    /// <summary>
    /// Логика взаимодействия для UserControlMusic.xaml
    /// </summary>
    public partial class Author : UserControl
    {
        private List<string> autors = new List<string>();

        public Author()
        {
            InitializeComponent();

            var autors = GetArtist();
            foreach (var item in autors)
                AutorsListView.Items.Add(item.Name);
        }
        private void button_Music_Click(object sender, RoutedEventArgs e)
        {
            musicgrid.Children.Clear();
            musicgrid.Children.Add(new Player());
        }
    }
}
