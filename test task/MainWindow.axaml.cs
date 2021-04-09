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
        private ListBox ListBox_listOfSongs;  //initialize controls
        private TextBox TextBox_wikipediaLinkField;
        private string wikipediaLink;

        Scraper scraper;
        List<List<SongInfo>> listOfTablesWithSongs = new List<List<SongInfo>>();
        List<SongInfo> listOfSongs = new List<SongInfo>();
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
            wikipediaLink = TextBox_wikipediaLinkField.Text; //get link from user textbox
            listOfTablesWithSongs = scraper.ScrapeSongsData(wikipediaLink);
            if (listOfTablesWithSongs == null)                                //i know that one shouldn't return null but didn't know how to implement it otherwise
            {
                List<string> exceptionMessage = new List<string>() { "LINK FIELD IS EMPTY!" };
                ListBox_listOfSongs.Items = exceptionMessage;
            }
            else
            {
                listOfSongs = MainWindowUtility.GetSongsOutOfTables(listOfTablesWithSongs);
                ListBox_listOfSongs.Items = MainWindowUtility.GetStringSongsData(listOfSongs);
            }
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ListBox_listOfSongs = this.FindControl<ListBox>("listOfSongs");    //find control of itemsrepeater which shows list of songs
            TextBox_wikipediaLinkField = this.FindControl<TextBox>("wikipediaLinkField");       
        }       
    }
}
