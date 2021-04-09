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

        public List<List<SongInfo>> ScrapeSongsData(string wikipediaPage)
        {
            var web = new HtmlWeb();
            try
            {

                var doc = web.Load(wikipediaPage);

                string artistName = doc.DocumentNode.SelectSingleNode("//*[@class='contributor']").InnerText;

                List<List<SongInfo>> listOfTables = new List<List<SongInfo>>();

                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//*[@class='tracklist']"))
                {
                    List<SongInfo> tableOfSongs = new List<SongInfo>();
                    HtmlNode tbody = table.SelectSingleNode("tbody");
                    HtmlNodeCollection rows = tbody.SelectNodes("tr");                  //find rows of table
                    tableOfSongs = ScraperUtility.GetSongInfos(rows, artistName);
                    listOfTables.Add(tableOfSongs);
                }
                return listOfTables;
            }
            catch (UriFormatException)
            {
                return null;
            }
        }
      

    }
}
