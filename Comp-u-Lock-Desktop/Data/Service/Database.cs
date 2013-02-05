using System;
using System.IO;
using Newtonsoft.Json;
using System.Security.AccessControl;

namespace Data.Service
{
    public class Database
    {
        public FileStream Db;
        public Uri FilePath;


        public Database(string file)
        {
            FilePath = new Uri(Environment.CurrentDirectory + file);
            Db = !File.Exists(FilePath.AbsolutePath) ? File.Create(FilePath.AbsolutePath) : File.Open(FilePath.AbsolutePath, FileMode.Open);
        }

        public void Write(string json)
        {
            
            Console.WriteLine("File written successfully: {0}", FilePath.AbsolutePath);
        }

        public void Load()
        {
            using (var sr = new StreamReader(Db))
            {
                Console.WriteLine(sr.ReadLine());
            }
        }
    }
}
