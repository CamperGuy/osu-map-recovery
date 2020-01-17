using System;
using System.Collections.Generic;
using System.IO;

namespace Osu_Map_Recovery
{
    public class Parser
    {
        public Dictionary<short, Beatmap> Maps;

        public Parser()
        {
            Maps = new Dictionary<short, Beatmap>();
        }

        public bool ConfirmBeatmapDirectory(string path)
        {
            try
            {
                Directory.Exists(path);

                
                foreach(string filename in Directory.GetFiles(path, "*.osu", SearchOption.AllDirectories))
                {
                    // do things here
                }
                
                else
                    return false;
            }
            catch (IOException)
            {
                return false;

            }

            return true;
        }
        
    }
}
