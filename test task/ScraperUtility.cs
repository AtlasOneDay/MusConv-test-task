using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Diagnostics;

namespace test_task
{
    class ScraperUtility
    {
        public static List<SongInfo> GetSongInfos_Album(HtmlNodeCollection rows,string artistName)
        {
            List<SongInfo> tableOfSongs = new List<SongInfo>();
            foreach (HtmlNode row in rows) //inside every single row of all tracklist tables
            {                             
                string songName = GetSongNameFromRow_Album(row, rows);
                string songWriters = GetSongWritersFromRow_Album(row, rows);
                string songDuration = GetSongDurationFromRow_Album(row, rows);
                //artist always going to be the same
                if (ValidatedForTable(row, rows) == true)
                {
                    tableOfSongs.Add(new SongInfo(songName, artistName, songWriters, songDuration));
                }
            }
            return tableOfSongs;
        }
        public static List<SongInfo> GetSongInfos_Single(List<HtmlNode> rows, string artistName)
        {
            List<SongInfo> tableOfSongs = new List<SongInfo>();
            foreach(HtmlNode row in rows)
            {
                string songName = GetSongNameFromRow_Single(row, rows);
                string songWriters = GetSongWritersFromRow_Single(row, rows);
                string songDuration = GetSongDurationFromRow_Single(row, rows);
                tableOfSongs.Add(new SongInfo(songName, artistName, songWriters, songDuration));
            }
            return tableOfSongs;
        }
        private static string GetSongNameFromRow_Album(HtmlNode row, HtmlNodeCollection rows)
        {
            if (ValidateRowForSongNameExtraction(row,rows) == true)   //dont get first and last row of names (because its "title" and "totallength")
            {
                int iter = 0;                                       //when iter = 0 - its song number, iter = 1 - song title, iter = 2 - writers, iter = 3 - song duration
                foreach (HtmlNode cell in row.SelectNodes("th|td"))
                {
                    if (iter == 1)
                    {
                        return cell.InnerText;
                    }
                    iter++;
                }
            }
            return "Song was not found";

        }
        private static string GetSongWritersFromRow_Album(HtmlNode row, HtmlNodeCollection rows)
        {
            if (ValidateRowForWritersExtraction(row, rows) == true)
            {
                int iter = 0;
                HtmlNodeCollection cells = row.SelectNodes("th|td");
                foreach (HtmlNode cell in cells)
                {
                    if (cells.Count == 4)
                    {
                        if (iter == 2)
                        {
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
                            return "Writer wasn't specified";
                        }
                    iter++;
                }
            }
            return "Writer was not found";
        }
        private static string GetSongDurationFromRow_Album(HtmlNode row, HtmlNodeCollection rows)
        {
            if (ValidateROwForDurationExtraction(row,rows) == true)
            {
                int iter = 0;
                HtmlNodeCollection cells = row.SelectNodes("th|td");
                foreach (HtmlNode cell in cells)
                {
                    if (cells.Count == 4)
                    {
                        if (iter == 3)
                        {
                            return cell.InnerText;
                        }
                    }
                    else if (cells.Count == 3)
                    {
                        if (iter == 2)
                        {
                            return cell.InnerText;
                        }
                    }
                    iter++;
                }
            }
            return "Duration was not found";
        }
        private static string GetSongNameFromRow_Single(HtmlNode row, List<HtmlNode> rows)
        {
            string rowContents = row.InnerText;
            int dashIndex = rowContents.IndexOf("–");
            if (dashIndex > 0) 
            {
                rowContents = rowContents.Substring(0, dashIndex);
            }
            int bracketIndex = rowContents.IndexOf(")");
            if(bracketIndex > 0)
            {
                rowContents = rowContents.Substring(1,bracketIndex);
            }
            
            return rowContents;
        }
        private static string GetSongWritersFromRow_Single(HtmlNode row, List<HtmlNode> rows)
        {
            return "Writer wasn't specified";
        }
        private static string GetSongDurationFromRow_Single(HtmlNode row, List<HtmlNode> rows)
        {
            string rowContents = row.InnerText;
            int dashIndex = rowContents.IndexOf("–");
            if (dashIndex > 0)
            {
                rowContents = rowContents.Substring(dashIndex + 2);
            }
            int minusIndex = rowContents.IndexOf("-");
            if(minusIndex > 0)
            {
                rowContents = rowContents.Substring(minusIndex + 1);
            }
            return rowContents;
        }
        private static bool ValidatedForTable(HtmlNode row, HtmlNodeCollection listOfRows)
        {
            bool RowIsCalledTotalLength = row.InnerText.Contains("Total length"); //recognize if last row is needed
            if (row == listOfRows[0] || ((row == listOfRows[listOfRows.Count - 1]) && RowIsCalledTotalLength))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool ValidateROwForDurationExtraction(HtmlNode row, HtmlNodeCollection listOfRows)
        {
            if (row != listOfRows[0]) //dont get first row (because its just word "Writers")
            {
                return true;
            }
            return false;
        }
        private static bool ValidateRowForWritersExtraction(HtmlNode row, HtmlNodeCollection listOfRows)
        {
            if (row != listOfRows[0]) //dont get first row (because its just word "Writers")
            {
                return true;
            }
            return false;
        }
        private static bool ValidateRowForSongNameExtraction(HtmlNode row, HtmlNodeCollection listOfRows)
        {
            int firstRow_Title = 0;                          //unwanted rows
            int lastRow_TotalLength = listOfRows.Count - 1;  //
            bool RowIsCalledTotalLength = row.InnerText.Contains("Total length");
            if (row == listOfRows[firstRow_Title] || ((row == listOfRows[lastRow_TotalLength]) && RowIsCalledTotalLength))   //dont get first and last row of names (because its "title" and "totallength")
            {
                return false;
            }
            else
            {
                return true;
            }
        }



    }
}
