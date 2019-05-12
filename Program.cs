using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Osu_Map_Recovery
{
    class Program
    {
        private static string sourcedir;
        private static string outdir;

        private static Dictionary<int, string> beatmapsAuthor = new Dictionary<int, string>();
        private static Dictionary<int, string> beatmapsTitle = new Dictionary<int, string>();
        static void Main(string[] args)
        {
            Console.Title = "Osu Map Recovery";
            
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine("Please enter the current osu song directory");
                Console.Write("> ");
                string input = Console.ReadLine();
                if (checkOsuDir(input))
                {
                    sourcedir = input;
                    valid = true;
                }
            }

            bool valid1 = false;
            while (!valid1)
            {
                Console.WriteLine("\nPlease enter the path for the output file");
                Console.Write("> ");
                string input = Console.ReadLine();
                if (checkOutDir(input))
                {
                    outdir = input;
                    valid1 = true;
                }
            }

            Console.WriteLine("Grabbing songs...");

            string[] songs = Directory.GetDirectories(sourcedir);
            
            Console.WriteLine("All directories have been grabbed");

            foreach (string str in songs)
            {
                string fullstring = str;
                fullstring = fullstring.Remove(0, sourcedir.Count() + 1);

                string[] splitID = fullstring.Split(new char[0]);
                int ID;
                Int32.TryParse(splitID[0], out ID);

                string author = "";
                string title = "";
                bool pastHyphen = false;
                for (int i = 1; i <= splitID.GetUpperBound(0); i++)
                {
                    if (pastHyphen == false && splitID[i] == "-")
                    {
                        pastHyphen = true;
                        i++;
                    }

                    if (!pastHyphen)
                        author += splitID[i];
                    else
                        title += splitID[i];
                }

                if (!beatmapsAuthor.ContainsKey(ID) && !beatmapsTitle.ContainsKey(ID))
                {
                    beatmapsAuthor.Add(ID, author);
                    beatmapsTitle.Add(ID, title);
                    Console.WriteLine("ID: " + ID + "\nAuthor: " + author + "\nTitle: " + title+ "\n");
                }
            }
            Console.WriteLine("All data has been loaded");

            writeHTML();

            System.Diagnostics.Process.Start(outdir + @"\osu_songs.html");
        }

        static void writeHTML()
        {
            if (!File.Exists(outdir + @"\osu_songs.html"))
            {
                Console.WriteLine("Starting to write html...");
                StreamWriter stream = new StreamWriter(outdir + @"\osu_songs.html", true, System.Text.Encoding.UTF8);

                stream.WriteLine("<html>" + stream.NewLine + @"<head>");
                stream.WriteLine("<style>");
                stream.Write("* {" + stream.NewLine + @"    font-family: sans-serif;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("table{" + stream.NewLine + "    width: 60%; " + stream.NewLine + "    border-collapse: collapse;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("table, th, td{" + stream.NewLine + @"    border: 1px solid black;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("th{" + stream.NewLine + @"    background-color: #BBB;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("tr:nth-child(odd){" + stream.NewLine + "    background-color: #DDD;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("</style>" + stream.NewLine);
                stream.WriteLine("<title>Osu Map Recovery</title>");
                stream.WriteLine("</head>");
                stream.WriteLine("<body>");
                stream.WriteLine("<table>");
                stream.Write("  <tr>" + stream.NewLine + "    <th>Link</th>" + stream.NewLine + "    <th>Title</th>" + stream.NewLine + "    <th>Author</th>" + stream.NewLine + "  </tr>" + stream.NewLine);
                
                foreach (KeyValuePair<int, string> pair in beatmapsAuthor)
                {
                    string title = "";
                    beatmapsTitle.TryGetValue(pair.Key, out title);

                    stream.WriteLine("  <tr>");
                    stream.WriteLine("    <td>" + stream.NewLine + @"      <a href='https://osu.ppy.sh/s/" + pair.Key + "'>" + pair.Key + "</a>" + stream.NewLine + "    </td>");
                    stream.WriteLine("    <td>" + stream.NewLine + "      " + title + stream.NewLine + "    </td>");
                    stream.WriteLine("    <td>" + stream.NewLine + "      " + pair.Value + stream.NewLine + "    </td>");
                    stream.WriteLine("  </tr>");
                }
                stream.WriteLine("</table>");
                stream.WriteLine("</body>");
                stream.WriteLine("</html>");

                stream.Flush();
                stream.Close();
            }
            else
            {
                File.Delete(outdir + @"\osu_maps.html");
                writeHTML();
            }
        }
        static bool checkOsuDir(string path)
        {
            if (Directory.Exists(path))
            {
                if (File.Exists(Directory.GetParent(path) + @"\osu!.exe"))
                {
                    return true;
                }
                Console.WriteLine("\nDirectory exists, but is not an osu! directory");
                return false;
            }
            return false;
        }

        static bool checkOutDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine("The directory does not exist yet.\nWould you like to create it? [y/n]");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                        checkOutDir(path);
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("An IO Exception has occured when trying to create the directory.");
                        return false;
                    }
                }
            }
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.GetAccessControl(path);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("It appears as though you do not have write permission for this folder");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unknown exception has occured. Please report the console output as well as the following:\n\n" + ex.ToString());
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                return true;
            }
            return true;
        }
    }
}
