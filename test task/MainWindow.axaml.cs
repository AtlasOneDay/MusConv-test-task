using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Diagnostics;

namespace test_task
{
    public class MainWindow : Window
    {
        Scraper scraper; 
        private ItemsRepeater listOfSongs;
        private TextBox wikipediaLinkField;
        private string wikipediaLink;

        List<SongInfo> Songs = new List<SongInfo>();
        List<string> INFOstrings = new List<string>();
        List<List<SongInfo>> listOfTablesWithSongs = new List<List<SongInfo>>();

        public MainWindow()
        {
            InitializeComponent();
            scraper = new Scraper();
            DataContext = scraper;
#if DEBUG
            this.AttachDevTools();          
#endif
        }
        public void buttonGetMusicData_Click(object sender, RoutedEventArgs e)
        {
            wikipediaLink = wikipediaLinkField.Text;

            listOfTablesWithSongs = scraper.ScrapeSongsData(wikipediaLink);
            if (listOfTablesWithSongs == null)
            {
                wikipediaLinkField.Text = "INVALID LINK";
            }
            else
            {


                foreach (List<SongInfo> table in listOfTablesWithSongs)
                {
                    foreach (SongInfo song in table)
                    {
                        Songs.Add(song);
                    }
                }

                for (int i = 0; i < Songs.Count - 1; i++)
                {

                    INFOstrings.Add("Song Name: " + Songs[i].SongName + "\n "
                        + "Artist Name: " + Songs[i].ArtistName + " "
                        + "Writers: " + Songs[i].Writers + " "
                        + "Duration: " + Songs[i].SongDuration);
                }
                listOfSongs.Items = INFOstrings;
            }


        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            listOfSongs = this.FindControl<ItemsRepeater>("listOfSongs"); //find control of itemsrepeater which shows list of songs
            wikipediaLinkField = this.FindControl<TextBox>("wikipediaLinkField");
            

            
        }

        
    }
}
