using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_task
{
    class MainWindowUtility
    {
        public static List<SongInfo> GetSongsOutOfTables(List<List<SongInfo>> listOfTablesWithSongs)
        {
            List<SongInfo> Songs = new List<SongInfo>();
            foreach (List<SongInfo> table in listOfTablesWithSongs)
            {
                foreach (SongInfo song in table)
                {
                    Songs.Add(song);
                }
            }
            return Songs;
        }
        public static List<string> GetStringSongsData(List<SongInfo> listOfSongs)
        {
            List<string> INFOstrings = new List<string>();
            for(int i = 0; i < listOfSongs.Count; i++)
            {

                INFOstrings.Add("Song Name: " + listOfSongs[i].SongName + " \n"
                    + "Artist Name: " + listOfSongs[i].ArtistName + " \n"
                    + "Writers: " + listOfSongs[i].Writers + " \n"
                    + "Duration: " + listOfSongs[i].SongDuration);
            }
            return INFOstrings;
        }
    }
}
