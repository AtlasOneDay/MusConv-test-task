using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_task
{
    public class SongInfo
    {
        private string songName, artistName, writers, songDuration;
        public SongInfo(string songName, string artistName, string writers, string songDuration)
        {
            this.songName = songName;
            this.artistName = artistName;
            this.writers = writers;
            this.songDuration = songDuration;
        }
        public string SongName
        {
            get { return songName; }
            set { songName = value; }
        }
        public string ArtistName
        {
            get { return artistName; }
            set { artistName = value; }
        }
        public string Writers
        {
            get { return writers; }
            set { writers = value; }
        }
        public string SongDuration
        {
            get { return songDuration; }
            set { SongDuration = value; }
        }

    }
}
