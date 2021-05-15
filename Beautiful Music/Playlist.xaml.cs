using BeautifulMusic.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static BeautifulMusic.SqlDataAccess;

namespace BeautifulMusic
{
    public partial class Playlist : UserControl
    {
        public Playlist()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                CurrentPlayListView.ItemsSource = TrackModels;
            }
            Playlists = GetPlayLists();
            PlayListsListView.ItemsSource = Playlists;
        }

        private void button_Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PlayListsListView.SelectedItem is PlaylistModel playlist)
                {
                    TrackModels = GetTracksByPlayList(playlist.Id);
                    CurrentPlayListView.ItemsSource = TrackModels;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void button_Play_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPlayListView.SelectedItem is TrackModel track)
            {
                SelectedTrack = track;
                PlayListGrid.Children.Clear();
                PlayListGrid.Children.Add(new Player());
            }
        }
        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            PlayListGrid.Children.Add(new PlayDialog());
        }

        private void button_Clear_Click(object sender, RoutedEventArgs e)
        {
            TrackModels.Clear();
            CurrentPlayListView.ItemsSource = TrackModels;
        }

        private void button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PlayListsListView.SelectedItem is PlaylistModel model)
                {
                    if (model.UserId != CurrentUser.Id)
                        throw new Exception("Запрещено удаление");
                    DeletePlayList(model.Id);
                    Playlists.Remove(model);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
