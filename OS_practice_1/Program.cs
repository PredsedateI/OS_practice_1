using System;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.IO.Compression;

namespace OS_practice_1
{
    class PLACEnTIME
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public short Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            First();
            Second();
            Third();
            Fourth();
            Fifth();
        }
        static void First()
        {
            Console.WriteLine("1.");
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine("Название: {0}", drive.Name);
                Console.WriteLine("Тип диска: {0}", drive.DriveType);
                if (drive.IsReady)
                {
                    Console.WriteLine("Метка тома: {0}", drive.VolumeLabel);
                    Console.WriteLine("Ёмкость: {0} байт ({1:F} Гбайт)", drive.TotalSize, drive.TotalSize / 1073741824.0);
                    Console.WriteLine("Свободно: {0} байт ({1:F} Гбайт)", drive.TotalFreeSpace, drive.TotalFreeSpace / 1073741824.0);
                    Console.WriteLine("Доступно для текущего пользователя: {0} байт ({1:F} Гбайт)", drive.AvailableFreeSpace, drive.AvailableFreeSpace / 1073741824.0);
                    Console.WriteLine("Файловая система: {0}", drive.DriveFormat);
                }
                Console.WriteLine();
            }
        }
        static void Second()
        {
            Console.WriteLine("2.");
            string path = "TXTfile.txt";
            string str = Console.ReadLine();

            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write(str);
            }

            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            FileInfo fileInf = new FileInfo(path);
            fileInf.Delete();
            Console.WriteLine();
        }
        static void Third()
        {
            Console.WriteLine("3.");
            string path = "JSONfile.json";
            PLACEnTIME rf = new PLACEnTIME { Latitude = 55.755800, Longitude = 37.617683, Day = 25, Month = "December", Year = 1992 };
            string json = JsonSerializer.Serialize<PLACEnTIME>(rf);

            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write(json);
            }

            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            File.Delete(path);
            Console.WriteLine();
        }
        static void Fourth()
        {
            Console.WriteLine("4.");
            string path = "XMLfile.xml";
            XmlDocument xd = new XmlDocument();
            xd.Load("XMLfile.xml");

            XmlElement rootElem = xd.DocumentElement;
            XmlElement userElem = xd.CreateElement("user");
            XmlElement nameElem = xd.CreateElement("name");
            XmlElement companyElem = xd.CreateElement("company");
            XmlElement ageElem = xd.CreateElement("age");

            Console.Write("Введите имя: ");
            XmlText nameText = xd.CreateTextNode(Console.ReadLine());
            Console.Write("Введите название компании: ");
            XmlText companyText = xd.CreateTextNode(Console.ReadLine());
            Console.Write("Введите возраст: ");
            XmlText ageText = xd.CreateTextNode(Console.ReadLine());

            nameElem.AppendChild(nameText);
            companyElem.AppendChild(companyText);
            ageElem.AppendChild(ageText);
            userElem.AppendChild(nameElem);
            userElem.AppendChild(companyElem);
            userElem.AppendChild(ageElem);
            rootElem.AppendChild(userElem);
            xd.Save("XMLfile.xml");

            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            File.Delete(path);
            Console.WriteLine();
        }
        static void Fifth()
        {
            Console.WriteLine("5.");
            string zipPath = "ZIPfile.zip";
            using (FileStream file = new FileStream(zipPath, FileMode.OpenOrCreate))
            {
                using ZipArchive zipfile = new ZipArchive(file, ZipArchiveMode.Update);

                string path = "";
                while (!File.Exists(path))
                {
                    Console.Write("Введите путь к файлу для добавление в архив: ");
                    path = Console.ReadLine();
                }

                zipfile.CreateEntryFromFile(path, path.Substring(path.LastIndexOf('\\') + 1));

                zipfile.ExtractToDirectory("fromZIPfile");

                path = "fromZIPfile\\" + path.Substring(path.LastIndexOf('\\') + 1);
                FileInfo fileInf = new FileInfo(path);
                Console.WriteLine("Имя файла: {0}\nВремя создания: {1:F}\nРазмер: {2} байт", fileInf.Name, fileInf.CreationTime, fileInf.Length);
            }

            Directory.Delete("fromZIPfile", true);
            File.Delete(zipPath);
            Console.WriteLine();
        }
    }
}
