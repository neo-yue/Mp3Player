using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace Yue_mp3Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TagLib.File currentFile;

        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private bool openFile=false;


        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();    //Execute timer_tick every 1 second  
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        


        private void timer_Tick(object sender, EventArgs e)  
        {
            //When the read file is not empty and has not finished playing. and the user does not drag the slider bar
            //Get music playback progress
            if ((myMediaPlayer.Source != null) && (myMediaPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = myMediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = myMediaPlayer.Position.TotalSeconds;
            }
        }



        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = openFile;         //If you open a file, you can edit the tag
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Edit_Tag.Visibility == Visibility.Visible)                  //Hide the edit window on the second click
            {

                Edit_Tag.Visibility = Visibility.Hidden;
            }
            else
            {
                                                                            //show the edit window
                Playing_Tag.Visibility = Visibility.Hidden;
                Edit_Tag.Visibility = Visibility.Visible;
            }
        }

        private void CurrentTag_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = openFile;         //If you open a file, you can show the tag
        }

        private void CurrentTag_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            if (Playing_Tag.Visibility == Visibility.Visible)
            {                                                                     //Hide the current tag window on the second click

                Playing_Tag.Visibility = Visibility.Hidden;
            }
            else
            {
                Playing_Tag.Visibility = Visibility.Visible;                        //show current file tag
                Edit_Tag.Visibility = Visibility.Hidden;
                syncTextbox();
            }
            

        }





        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;                //can open file.
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //MP3 format file filter
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
            
            if (openFileDialog.ShowDialog() == true)
              
                //Set the source of the media player element.
                myMediaPlayer.Source = new Uri(openFileDialog.FileName);

            //The filename property stores the full path that was selected
            //currentFile = TagLib.File.Create(openFileDialog.FileName);
            if (openFileDialog.FileName!="") {
                try                                         //Alert the user if you open a non-MP3 file (when the filter is turned off)
                {
                    Playing_Tag.currentFile = TagLib.File.Create(openFileDialog.FileName);
                    Playing_Tag.readTag();

                    Edit_Tag.currentFile = TagLib.File.Create(openFileDialog.FileName);
                    Edit_Tag.readTag();
                } catch (Exception ex) { 
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("The player can only open MP3 files");
                }

                openFile = true;
            }
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (myMediaPlayer != null) && (myMediaPlayer.Source != null); //Play music when player and file are available
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaPlayer.Play();                           //Play music
            mediaPlayerIsPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;            //Pause music while playing
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaPlayer.Pause();                        //Pause music  
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying; //Stop music while playing
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaPlayer.Stop();               //Stop music
            mediaPlayerIsPlaying = false;           
        }

       

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e) {
            System.Diagnostics.Process.GetCurrentProcess().Kill();//close the programe

        }
        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;            //When the user drags the slider bar,  userIsDraggingSlider is set to true
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;    //When the user finishes dragging the slider,  userIsDraggingSlider is set to false
            myMediaPlayer.Position = TimeSpan.FromSeconds(sliProgress.Value); //Set the file playing progress
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TBlockPosition.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");//Displays the playback time
        }

        private void syncTextbox()
        {                                                       //Synchronize the modified data to the current tag
            
                Playing_Tag.title = Edit_Tag.title();
                Playing_Tag.artist = Edit_Tag.artists();

            if (Edit_Tag.year() != "" )
            {
                Playing_Tag.albumYear = Edit_Tag.album() + "(" + Edit_Tag.year() + ")";
            }
            else
            {
                Playing_Tag.albumYear = Edit_Tag.album();
            } 
           
            
        }

    }
}
