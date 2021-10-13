using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace CPU_Usage_Console
{
    class Program
    {
        // https://stackoverflow.com/questions/51193103/how-to-get-total-cpu-usage-all-processes-c/51194100

        static readonly string gFileName = "cpu_usage.csv";
        static int gIdleTime = 1 * 1000 * 10; // 10 sec
        static void Main(string[] args)
        {

            var cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            Thread.Sleep(1000);
            var firstCall = cpuUsage.NextValue();
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");

            List<string> listInfo = new List<string>();

            for (int i = 0; i < 2; i++)
            {
                // 外層兩次等於兩分鐘，這邊可自行規劃，改成while(1)這樣一職跑，透過定時的資訊處理後再看是否要結束迴圈
                for (int j = 0; j < 6; j++)
                {
                    // 10秒鐘取一次cpu資訊，跑六次就是一分鐘
                    Thread.Sleep(gIdleTime);
                    currentTime = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");
                    string strUsage = String.Format("{0}\t{1}%", currentTime, cpuUsage.NextValue());
                    listInfo.Add(strUsage);
                    Console.WriteLine(strUsage);
                }

                if (!File.Exists(gFileName))
                {
                    listInfo.Insert(0, "Time \t CPU Usage%");
                }

                File.AppendAllLines(gFileName, listInfo);

                listInfo.Clear();

                // TODO: 可以判斷幾分鐘後呼叫函數做sorting以及trigger notification的動作
            }

            Console.Read();
           
        }
    }
}
