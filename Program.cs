using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ReviewWindows
{
    internal class Program
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rectangle rect);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
           // ShowWindow(handle, SW_HIDE);

            for (;;)
            {
                try
                {
                    GetStatus();
                }
                catch (Exception ex)
                {
                }
                System.Threading.Thread.Sleep(600000);
            }
        }

        public static void GetStatus()
        {
            Process[] processes1 = Process.GetProcessesByName("mstsc");

            List<Process> processes = new List<Process>(processes1);

            Process[] processes2 = Process.GetProcessesByName("iexplore");

            foreach (var item in processes2)
            {
                processes.Add(item);
            }


            //ProcessStartInfo startInfo = new ProcessStartInfo("mstsc.exe");
            // startInfo.WindowStyle

            //Process[] processes442 = Process.GetProcesses();
            string log = string.Empty;
            string newFileName = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Logs\\client_Review" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".csv";
            foreach (Process p in processes)
            {
                IntPtr windowHandle = p.MainWindowHandle;
                Rectangle rect;
                GetWindowRect(windowHandle, out rect);

                Console.Write("X: " + rect.Location.X.ToString() + " - ");
                Console.Write("Y: " + rect.Location.Y.ToString() + " - ");
                Console.Write(p.MainWindowTitle); Console.WriteLine();

                log = string.Format("{0};{1};{2};{3};{4}{5}", rect.Location.X.ToString(), rect.Location.Y.ToString(), p.MainWindowTitle.ToString(), DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine);

                if (!File.Exists(newFileName))
                {
                    string clientHeader = "Position X" + "," + "Position Y" + "," + "WindowTitle" + "," + "Date" + "," + "Time" + Environment.NewLine;

                    File.WriteAllText(newFileName, clientHeader);
                }

                File.AppendAllText(newFileName, log);

              
            }
            Console.WriteLine("--------------------------------");
        }
    }
}