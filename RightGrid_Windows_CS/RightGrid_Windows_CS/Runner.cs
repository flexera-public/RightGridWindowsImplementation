using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;

namespace WinRightGrid
{
    class Runner
    {
        public static void run(string input_file, string output_file) 
        {
            string executable_string = ConfigurationManager.AppSettings["Runnable_Application"].Replace("@input_file", " " + input_file).Replace("@output_file", " " + output_file);
            Console.WriteLine(executable_string);
        }
    }
}
