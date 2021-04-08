using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Avalonia.Controls;
using HtmlAgilityPack;

namespace test_task
{
    public class Scraper
    {
        private ObservableCollection<SongInfo> _entries = new ObservableCollection<SongInfo>();
        public ObservableCollection<SongInfo> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }

        public List<List<SongInfo>> ScrapeSongsData(string wikipediaPage)
        {
            var web = new HtmlWeb();
            var doc = web.Load(wikipediaPage);

            List<List<SongInfo>> listOfTables = new List<List<SongInfo>>();


            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//*[@class='tracklist']"))
            {
                List<SongInfo> tableOfSongs = new List<SongInfo>();
                Debug.Print("table");
                HtmlNode tbody = table.SelectSingleNode("tbody");
                HtmlNodeCollection rows = tbody.SelectNodes("tr");


                foreach (HtmlNode row in rows) //inside every single row of all tracklist tables
                {

                    string artistName = doc.DocumentNode.SelectSingleNode("//*[@class='contributor']").InnerText; //artist always going to be the same
                    string songName = "";
                    string songWriters = "";
                    string songDuration = "";

                    if (row == rows[0] || row == rows[rows.Count - 1])   //dont get first and last row of names (because its "title" and "totallength")
                    {
                        //skip
                    }
                    else
                    {
                        songName = GetSongNameFromRow(row);
                    }
                    if (row != rows[0]) //dont get first row (because its just word "Writers")
                    {
                        songWriters = GetSongWritersFromRow(row);
                        songDuration = GetSongDurationFromRow(row);
                    }

                    if (row == rows[0] || row == rows[rows.Count - 1])
                    {
                        //skip
                    }
                    else
                    {
                        tableOfSongs.Add(new SongInfo(songName, artistName, songWriters, songDuration));
                    }
                }

                listOfTables.Add(tableOfSongs);

            }



            return listOfTables;
        }
        private string GetSongNameFromRow(HtmlNode row)
        {
            int iter = 0;                                       //when iter = 0 - its song number, iter = 1 - song title, iter = 2 - writers, iter = 3 - song duration
            foreach (HtmlNode cell in row.SelectNodes("th|td"))
            {
                if (iter == 1)
                {
                    Debug.Print(cell.InnerText);
                    return cell.InnerText;
                }
                iter++;

            }
            return "Song was not found";

        }
        private string GetSongWritersFromRow(HtmlNode row)
        {
            int iter = 0;
            HtmlNodeCollection cells = row.SelectNodes("th|td");
            foreach (HtmlNode cell in cells)
            {
                if (cells.Count == 4)
                {
                    if (iter == 2)
                    {
                        Debug.Print(cell.InnerText);
                        if (cell.InnerText == "&#160;")
                        {
                            return "Writer wasn't specified";
                        }
                        else
                        {
                            return cell.InnerText;
                        }
                    }
                }
                else if (cells.Count == 3)
                    if (iter == 2)
                    {
                        Debug.Print("Writer wasn't specified");    //&#160 means empty space
                        return "Writer wasn't specified";
                    }
                iter++;
            }
            return "Writer was not found";
        }
        private string GetSongDurationFromRow(HtmlNode row) //if number of cells is 3 , it means that row has no "writers value,which means we take not iter = 3, but iter = 2"
        {
            int iter = 0;
            HtmlNodeCollection cells = row.SelectNodes("th|td");
            foreach (HtmlNode cell in cells)
            {
                if (cells.Count == 4)
                {
                    if (iter == 3)
                    {
                        Debug.Print(cell.InnerText);
                        return cell.InnerText;
                    }
                }
                else if (cells.Count == 3)
                {
                    if (iter == 2)
                    {
                        Debug.Print(cell.InnerText);
                        return cell.InnerText;
                    }
                }
                iter++;
            }
            return "Duration was not found";
        }

    }
}
