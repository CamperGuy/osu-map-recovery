using System;

namespace Osu_Map_Recovery
{
    public class Beatmap
    {
        public int Mapid { get; private set; }
        public string Filename { get; private set; }
        public string Author { get; private set; }
        public string Title { get; private set; }
        public string URL { get; private set; }

        public Beatmap(int id, string filename)
        {
            Mapid = id;
            Filename = filename;
            URL = "https://osu.ppy.sh/b/" + Mapid;
            GetMapDetails();
        }

        private void GetMapDetails()
        {
            // https://osu.ppy.sh/help/wiki/osu!_File_Formats/Osu_(file_format)
        }
    }
}
