using BeautifulMusic.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static BeautifulMusic.SqlDataAccess;

namespace BeautifulMusic
{
    /// <summary>
    /// Логика взаимодействия для UserControl4.xaml
    /// </summary>
    public partial class Mymusic : UserControl
    {
        public Mymusic()
        {
            InitializeComponent();
            PlayListView.ItemsSource = MyTrackModels;
        }
        private void button_Play_Click(object sender, RoutedEventArgs e)
        {
            if (PlayListView.SelectedItem is TrackModel track)
            {
                SelectedTrack = track;
                TrackModels.Add(track);
                Searcallhgrid.Children.Clear();
                Searcallhgrid.Children.Add(new Player());
            }
        }
    }
}
