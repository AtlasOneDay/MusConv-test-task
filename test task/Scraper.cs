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
                bool album = doc.DocumentNode.SelectSingleNode("//*[@class='infobox-header description']").InnerText.Contains("Studio album");
                bool single = doc.DocumentNode.SelectSingleNode("//*[@class='infobox-header description']").InnerText.Contains("Single");
                if (album)
                {
                    return ScrapeIf_AlbumPage(wikipediaPage);
                }
                else if (single)
                {
                    return ScrapeIf_SinglePage(wikipediaPage);
                }
                else
                    return null;
            }
            catch (System.Net.WebException)
            {
                return null;
            }
            catch(UriFormatException)
            {
                return null;
            }
        }
        private List<List<SongInfo>> ScrapeIf_AlbumPage(string wikipediaPage)
        {
            List<List<SongInfo>> listOfTables = new List<List<SongInfo>>();
            var web = new HtmlWeb();      
            var doc = web.Load(wikipediaPage);   
            string artistName = doc.DocumentNode.SelectSingleNode("//*[@class='contributor']").InnerText;            

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//*[@class='tracklist']"))
            {
                List<SongInfo> tableOfSongs = new List<SongInfo>();
                HtmlNode tbody = table.SelectSingleNode("tbody");
                HtmlNodeCollection rows = tbody.SelectNodes("tr");                  //find rows of table
                tableOfSongs = ScraperUtility.GetSongInfos_Album(rows, artistName);
                listOfTables.Add(tableOfSongs);
            }
            return listOfTables;
        }
        private List<List<SongInfo>> ScrapeIf_SinglePage(string wikipediaPage)
        {
            List<List<SongInfo>> listOfTables = new List<List<SongInfo>>();
            List<SongInfo> tableOfSongs = new List<SongInfo>();
            var web = new HtmlWeb();
            var doc = web.Load(wikipediaPage);
            string artistName = doc.DocumentNode.SelectSingleNode("//*[@class='infobox-header description']").SelectNodes("a").LastOrDefault().Attributes["title"].Value;

            var divWithSongs = doc.DocumentNode.SelectSingleNode("//*[@class='mw-parser-output']");
            HtmlNodeCollection listOfRows_ol = divWithSongs.SelectNodes("ol");
            List<HtmlNode> listOfRows_li = new List<HtmlNode>();
            foreach(HtmlNode ol in listOfRows_ol)
            {
                HtmlNodeCollection rowsInsideOl = ol.SelectNodes("li");
                foreach(HtmlNode row in rowsInsideOl)
                {
                    listOfRows_li.Add(row);
                }
            }
            tableOfSongs = ScraperUtility.GetSongInfos_Single(listOfRows_li, artistName);
            listOfTables.Add(tableOfSongs);
            return listOfTables;
            
        }


    }
}
