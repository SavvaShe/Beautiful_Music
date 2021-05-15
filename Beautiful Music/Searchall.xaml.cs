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
    /// </summary>
    public partial class Searchall : UserControl
    {
        public Searchall()
        {
            InitializeComponent();
        }
        public void Button_Search_click(object sender, RoutedEventArgs e)
        {
            string search = textboxsearch.Text.Trim();
            if (string.IsNullOrEmpty(search))
            {
                textboxsearch.ToolTip = "это поле введено не корректно!";
                textboxsearch.Background = Brushes.LightBlue;
                return;
            }
            MyTrackModels = SearchTracs(search);
            if (!MyTrackModels.Any())
            {
                textboxsearch.ToolTip = "композиции не найдены";
                textboxsearch.Background = Brushes.LightBlue;
                return;
            }
            textboxsearch.ToolTip = "";
            textboxsearch.Background = Brushes.Transparent;
            Searcallhgrid.Children.Clear();
            Searcallhgrid.Children.Add(new Mymusic());
        }
    }
}
