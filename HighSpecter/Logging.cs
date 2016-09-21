using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace HighSpecter
{
    public static class Logging
    {
        private static bool _started = false;
        private static FileStream _file;
        private static StreamWriter _writer;
        
        private static StreamReader _reader;
        private static String _log = "";
            
        private static readonly string FolderPath = Logic.StaticPath + @"Logs";
        private static readonly string FilePath = Logic.StaticPath + @"Logs/Log " + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString()
            + "." + DateTime.Now.Second.ToString() + ".csv";

        private static Task _writeToDisk; 

        private static void Begin(string toAdd)
        {
            lock (_log)
            {
                if (_started)
                {
                    Add("tried initialise the log again? \n");
                    _log += "tried initialise the log again? \n"; 
                }
                else
                {
                    if (System.IO.Directory.Exists(FolderPath))
                    {
                        //if folder exists do nothign
                    }
                    else
                    { System.IO.Directory.CreateDirectory(FolderPath); }

                    _started = true;


                    _file = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 1024, true);
                    _writer = new StreamWriter(_file, Encoding.Unicode);
                    _reader = new StreamReader(_file, Encoding.Unicode);
                }
            }
            _writeToDisk = new Task(() =>
            {
                Add(toAdd);
            });
            _writeToDisk.Start(); 

        }

        public static void Add(string toAdd)
        {
            if (!_started) //if the log file is not started
            {
                Begin(toAdd);
            }
            else
            {
                String towrite = DateTime.Now.ToShortTimeString() + ":" + DateTime.Now.Second.ToString()
                    + "," + toAdd + Environment.NewLine; 
                toAdd = DateTime.Now.ToShortTimeString() +":" + DateTime.Now.Second.ToString() + ", " + toAdd + "\n"; 

                lock (_log)
                {
                    _log += toAdd;
                }

                lock (Logic.Status)
                {
                    Logic.Status.LogChanges += toAdd;
                }

                _writeToDisk.ContinueWith((continuation) =>
                {
                    Write(towrite);
                });
            }
        }

        public static String GetFullLog()
        {
            string temp = ""; 
            temp +=_log;            
            return temp;
        }

        private static void Write(string text)
        {
            _writer.Write(text);
            _writer.Flush();
        }

    }
}
