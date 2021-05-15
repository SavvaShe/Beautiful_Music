using BeautifulMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static BeautifulMusic.SqlDataAccess;

namespace BeautifulMusic
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TrackModels = new System.Collections.ObjectModel.ObservableCollection<TrackModel>();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
        private void buttonFechar_Click(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
        private void ButtonHidePage_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        { WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal; }
        private void grid_MouseDown(object sender, MouseButtonEventArgs e) { DragMove(); }

        private void listViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
        }


        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            moveCursorMenu(index);

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Promotion());
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Mediateka());
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Enter());
                    break;
                case 3:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Searchall());
                    break;
                case 4:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Mymusic());
                    break;
                case 5:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Playlist());
                    break;
                default:
                    break;
            }
        }

        private void moveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }

        public void ShowPlaylist() { ListViewMenu.SelectedIndex = 5; }
    }
}