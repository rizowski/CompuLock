using System;
using System.IO;

namespace Data.Service
{
    class Database
    {
        public FileStream Db;
        public Database(string file)
        {
            Db = !File.Exists(file) ? File.Create(file) : File.Open(file,FileMode.Open);
        }

        public void Save()
        {
            
        }

        public void Write(string json)
        {
            using (var s = new StreamWriter(Db))
            {
                s.Write(json);
            }
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
