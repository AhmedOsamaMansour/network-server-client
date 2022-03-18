using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            //Start server
            //change direction if you change path
            string path = @"F:\c\Project Network\Project\Template[2021-2022]\HTTPServer\bin\Debug\redirectionRule.txt";   
            // 1) Make server object on port 1000
            Server serv = new Server(1000,path);
            // 2) Start Server
           serv.StartServer();
        }
        static void CreateRedirectionRulesFile()
        {
          // TODO: Create file named redirectionRules.txt
             StreamWriter fw = new StreamWriter("redirectionRule.txt");
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            fw.WriteLine("/aboutus.html,/aboutus2.html");
            fw.Flush();
            fw.Dispose();

        }
         
    }
}
