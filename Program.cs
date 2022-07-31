using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace discord_link_downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath, folderPath, fileText, extension, fileName;
            string[] urls;

            int iExtension;

            Random random = new Random();

            Regex rg = new Regex("\"https://cdn.discordapp.com/attachments/.*?\""); //|(href=\".*?(.mov)\")|(href=\".*?(.webm)\")
            MatchCollection matches;

            Console.WriteLine("Please input the path of the file.");
            filePath = Console.ReadLine();

            Console.WriteLine("\nPlease input the path of the folder you'd like to save everything to.");
            folderPath = Console.ReadLine();

            Console.Clear();

            fileText = File.ReadAllText(filePath);

            matches = rg.Matches(fileText);

            urls = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                urls[i] = matches[i].ToString();
                urls[i] = urls[i].Trim('"');

                iExtension = urls[i].LastIndexOf('.');
                extension = "";

                for (int i2 = iExtension; i2 < urls[i].Length; i2++)
                {
                    extension += urls[i][i2];
                }

                fileName = random.Next() + extension;

                while (File.Exists(folderPath + "\\" + fileName))
                {
                    fileName = random.Next() + extension;
                }

                Console.WriteLine(urls[i]);

                using (var client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile(urls[i], folderPath + "\\" + fileName);
                    }
                    catch (WebException deadURL)
                    {
                        if (File.Exists(folderPath + "\\" + fileName))
                        {
                            System.IO.File.Delete(folderPath + "\\" + fileName);
                        }
                    }
                }
                
                
            }

            Console.Clear();

            Console.WriteLine("That should be it!");

            Console.ReadLine();
        }
    }
}
