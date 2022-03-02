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

namespace Yue_mp3Player
{
    /// <summary>
    /// Interaction logic for Playing_Tag.xaml
    /// </summary>
    public partial class Playing_Tag : UserControl
    {
        

        public TagLib.File currentFile;
        public Playing_Tag()
        {
            InitializeComponent();
            
        }

        public void readTag()
        {
                var title = currentFile.Tag.Title;
                var artist = currentFile.Tag.AlbumArtists;
                var album = currentFile.Tag.Album;
                var year = currentFile.Tag.Year;
                if (title != null)
                {
                    titleBox.Text = title.ToString();
                }
                else {
                    titleBox.Text = "";
                };
                if (artist != null)
                {
                    artistsBox.Text = string.Join(" ", artist);
                }
                else
                {
                    artistsBox.Text = "";
                }
                if (album != null)
                {
                    albumYearBox.Text = album.ToString();
                }
                else {
                    albumYearBox.Text = "";
                }
                if (year != 0) { 
                    albumYearBox.Text += "(" + year.ToString() + ")";
                }
        }

        public String title { set { titleBox.Text = value; } }
        public String artist { set { artistsBox.Text = value; } }
        public String albumYear { set { albumYearBox.Text = value; } }

    }
}
