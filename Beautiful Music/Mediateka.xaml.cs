using BeautifulMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static BeautifulMusic.SqlDataAccess;

namespace BeautifulMusic
{
    /// <summary>
    /// Логика взаимодействия для UserControl3.xaml
    /// </summary>
    public partial class Mediateka : UserControl
    {
        List<AlbumModel> albums;
        List<ArtistModel> artists;
        List<GenreModel> genres;
        List<TrackModel> tracks;

        public Mediateka()
        {
            albums = new List<AlbumModel>();
            artists = new List<ArtistModel>();
            genres = new List<GenreModel>();
            tracks = new List<TrackModel>();
            if (CurrentUser!=null)
                fillListViews();
        }

        private void fillListViews()
        {
            InitializeComponent();
            genres = GetGenres();
            artists = GetArtist();
            albums = GetAlbums();
            tracks = GetTracks();
            GenresListView.ItemsSource = genres;
            ArtistsListView.ItemsSource = artists;
            AlbumsListView.ItemsSource = albums;
            TraksListView.ItemsSource = tracks;
        }

        private void button_Album_Click(object sender, RoutedEventArgs e)
        {
            AlbumModel album = AlbumsListView.SelectedItem as AlbumModel;
            if (album != null)
            {
                tracks = GetTracksByAlbum(album.Id);
                TraksListView.ItemsSource = tracks;
            }
        }
        private void button_Artist_Click(object sender, RoutedEventArgs e)
        {
            ArtistModel artist = ArtistsListView.SelectedItem as ArtistModel;
            if (artist != null)
            {
                albums = GetAlbumsByArtist(artist.Id);
                AlbumsListView.ItemsSource = albums;
                tracks = GetTracksByArtist(artist.Id);
                TraksListView.ItemsSource = tracks;
            }
        }
        private void button_Play_Click(object sender, RoutedEventArgs e)
        {
            if (TraksListView.SelectedItem is TrackModel track)
            {
                SelectedTrack = track;
                TrackModels.Add(track);
                MediatekaGrid.Children.Clear();
                MediatekaGrid.Children.Add(new Player());
            }
        }
        private void button_Load_Click(object sender, RoutedEventArgs e)
        {
            foreach (TrackModel track in TraksListView.Items)
                TrackModels.Add(track);
        }

        private void button_Genres_Click(object sender, RoutedEventArgs e)
        {
            GenreModel genre = GenresListView.SelectedItem as GenreModel;
            if (genre != null)
            {
                artists = GetArtistByGenre(genre.Id);
                ArtistsListView.ItemsSource = artists;
                albums = GetAlbumsByGenre(genre.Id);
                AlbumsListView.ItemsSource = albums;
                tracks = GetTracksByGenre(genre.Id);
                TraksListView.ItemsSource = tracks;
            }
        }
    }
}
