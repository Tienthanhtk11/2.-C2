using System;
using System.IO;

namespace App
{
    public static class Log
    {
        public static void Logging(string ex, string filename)
        {
            StreamWriter sw = null;
            try
            {
                //Error,
                string s = @"C:\\SMS_LOG\";
                sw = new StreamWriter(s + "\\" + filename + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt", true);
                //sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine($"{DateTime.Now.ToString()} : {ex}");
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
