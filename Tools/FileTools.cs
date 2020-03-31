using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tools
{
    static public class FileTools
    {
        // static readonly string Directory = @"Tests";
        static public void Clear()
        {
            if (File.Exists("log.log"))
                File.Create("log.log").Close();
        }
        static public void Log(String Message, bool IsAddEnd = true)
        {
            try
            {
                if (!File.Exists("log.log"))
                    File.Create("log.log").Close();

                StreamWriter w = new StreamWriter("log.log", IsAddEnd);
                w.WriteLine(DateTime.Now + ":" + DateTime.Now.Millisecond + "\t\t" + Message);
                w.Close();
            }
            catch { }
        }
        static public void Log(String Message, String Path)
        {
            try
            {
                if (!File.Exists(Path))
                    File.Create(Path).Close();

                StreamWriter w = new StreamWriter(Path, true);
                w.WriteLine(DateTime.Now + ":" + DateTime.Now.Millisecond + "\t\t" + Message);
                w.Close();
            }
            catch { }
        }
        static public void Save(Object ob, String Path)
        {
            try
            {
                BinaryFormatter b = new BinaryFormatter();
                FileStream f = new FileStream(Path, FileMode.Create);
                b.Serialize(f, ob);
                f.Close();
            }
            catch { }
        }
        static public void Save(Object ob)
        {
            try
            {
                int m = 0;
                string s;
                BinaryFormatter b = new BinaryFormatter();
                if (!Directory.Exists("Saves")) Directory.CreateDirectory("Saves");
                while (true)
                {
                    s = "Saves//" + DateTime.Today.ToShortDateString() + m + ".sav";
                    if (File.Exists(s))
                        m++;
                    else break;
                }
                FileStream f = new FileStream(s, FileMode.Create);
                b.Serialize(f, ob);
                f.Close();
            }
            catch { }
        }

    }
}
