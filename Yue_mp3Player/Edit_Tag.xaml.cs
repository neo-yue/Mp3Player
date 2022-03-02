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
    /// Interaction logic for Edit_Tag.xaml
    /// </summary>
    public partial class Edit_Tag : UserControl
    {

        public TagLib.File currentFile;
        //public Playing_Tag playing=new Playing_Tag();
        public Edit_Tag()
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
                else
                {
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
                    albumBox.Text = album.ToString();
                }
                else
                {
                    albumBox.Text = "";
                }
                if (year != 0)
                {
                    yearBox.Text += year.ToString();
                }
                else
                {
                    yearBox.Text = "";
                }

        }
    

        private void save_Click(object sender, RoutedEventArgs e)           //Save the tag information of the file
        {
            currentFile.Tag.Title = titleBox.Text;                  
            currentFile.Tag.Album = albumBox.Text;
            currentFile.Tag.AlbumArtists = artistsBox.Text.Split(" ");
            try                                                            //Prompts the user when the year is not a number
            {
                if (!string.IsNullOrEmpty(yearBox.Text))                    //Allow  year value to be empty
                    currentFile.Tag.Year = (uint)int.Parse(yearBox.Text);
                else {
                    currentFile.Tag.Year = 0; 
                }

            }
            catch (FormatException ex)
            {
                if(currentFile.Tag.Year.ToString()!="0")                    
                    yearBox.Text = currentFile.Tag.Year.ToString();         //If the year entered is in the wrong format,
                                                                            //the textbox displays the previous content
                else
                {
                    yearBox.Text = "";
                }
                Console.WriteLine(ex.Message);
                MessageBox.Show("Years can only be numbers");
            }
            finally {
                currentFile.Save();
              
            }
        }


        public String title()  { return titleBox.Text; }
        public String artists() { return artistsBox.Text; }
        public String album() { return albumBox.Text; }
        public String year() { return yearBox.Text; }
    }
}
