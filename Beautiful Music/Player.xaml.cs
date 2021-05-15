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
    /// <summary>
    /// Логика взаимодействия для UserControlPlayer.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        private int currentSong = 0;
        MediaPlayer player = new MediaPlayer();

        private bool isPlay = false;
        private bool isLoaded = false;

        public Player()
        {
            InitializeComponent();

            SongName.Text = SelectedTrack.Name;
        }

        private void prev_Click(object sender, RoutedEventArgs e)
        {
            if (currentSong == 0)
            {
                 currentSong = TrackModels.Count - 1;
            }
            else
            {
                currentSong--;
            }
            changeSong();
        }

        private void playPause_Click(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                isLoaded = true;
                player.Open(new Uri(Environment.CurrentDirectory + "/Audio/" + SelectedTrack.Name.Trim() + ".mp3"));
            }

            if (isPlay)
            {
                MiddleIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                isPlay = false;
                player.Pause();
            }
            else
            {
                MiddleIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                isPlay = true;
                player.Play();
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (currentSong == TrackModels.Count-1)
            {
                currentSong = 0;
            }
            else
            {
                currentSong++;
            }

            changeSong();
        }

        private void changeSong()
        {
            MiddleIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            SelectedTrack = TrackModels[currentSong];
            SongName.Text = SelectedTrack.Name;
            isPlay = true;
            isLoaded = true;
            player.Open(new Uri(Environment.CurrentDirectory + "/Audio/" + SelectedTrack.Name.Trim() + ".mp3"));
            player.Play();
        }
    }
}
