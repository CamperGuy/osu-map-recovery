using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
            // Console Setup
            Console.OutputEncoding = Encoding.Unicode;
            Console.SetWindowSize(80, 23);
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;

            Console.Title = "Osu Map Recovery";
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("||                                                                           ||");
            Console.WriteLine("||                           Made by Joel Helbling                           ||");
            Console.WriteLine("||               https://github.com/camperguy/osu-map-recovery               ||");
            Console.WriteLine("||                                                                           ||");
            Console.WriteLine("-------------------------------------------------------------------------------");
            System.Threading.Thread.Sleep(2000);

            // Request and validate the beatmap directory
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine("\nPlease enter the current osu beatmap directory");
                Console.Write("> ");
                string input = Console.ReadLine();
                if (checkOsuDir(input))
                {
                    sourcedir = input;
                    valid = true;
                }
            }

            // Request and validate the output directory
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

            Console.WriteLine("\nGrabbing beatmaps...");

            string[] beatmaps = Directory.GetDirectories(sourcedir);
            
            Console.WriteLine("[x] All beatmaps have been grabbed!");

            Console.WriteLine("\nLoading beatmaps...");
            foreach (string str in beatmaps)
            {
                // Format the directory path
                string fullstring = str;
                fullstring = fullstring.Remove(0, sourcedir.Count() + 1);

                // Extract the Beatmap ID
                string[] splitID = fullstring.Split(new char[0]);
                int ID;
                Int32.TryParse(splitID[0], out ID);

                // Extract Author and Title
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
                        author += splitID[i] + " ";
                    else
                        title += splitID[i] + " ";
                }

                // Avoid duplicates
                if (!beatmapsAuthor.ContainsKey(ID) && !beatmapsTitle.ContainsKey(ID))
                {
                    // Add data to the Dictionary/HashMap
                    beatmapsAuthor.Add(ID, author);
                    beatmapsTitle.Add(ID, title);
                }
            }
            Console.WriteLine("[x] All beatmaps have been loaded!");

            Console.WriteLine("\nStarting to write HTML");
            writeHTML();
            Console.WriteLine("[x] HTML file has been written!");

            System.Diagnostics.Process.Start(outdir + @"\osu_beatmaps.html");

            System.Threading.Thread.Sleep(5000);
            Environment.Exit(0);
        }

        static void writeHTML()
        {
            if (!File.Exists(outdir + @"\osu_beatmaps.html"))
            {
                
                StreamWriter stream = new StreamWriter(outdir + @"\osu_beatmaps.html", true, System.Text.Encoding.UTF8);

                // Head
                stream.WriteLine("<html>" + stream.NewLine + @"<head>");
                stream.WriteLine("<title>Osu Map Recovery</title>");
                stream.WriteLine("<style>");
                // Set up CSS
                stream.Write("* {" + stream.NewLine + @"    font-family: sans-serif;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("table{" + stream.NewLine + "    width: 60%; " + stream.NewLine + "    border-collapse: collapse;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("table, th, td{" + stream.NewLine + @"    border: 1px solid black;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("th{" + stream.NewLine + @"    background-color: #BBB;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("tr:nth-child(odd){" + stream.NewLine + "    background-color: #DDD;" + stream.NewLine + "}" + stream.NewLine);
                stream.Write("</style>" + stream.NewLine);
                stream.WriteLine("</head>");
                // Body / Table
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
                Console.WriteLine("[!] Overwriting osu_beatmaps.html");
                File.Delete(outdir + @"\osu_beatmaps.html");
                writeHTML();
            }
        }
        static bool checkOsuDir(string path)
        {
            if (Directory.Exists(path))
            {
                // Confirm that the user has given the correct directory
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
                // Ask the user if they would like to try to create their directory
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
                // Confirm that we have write access to this folder
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
