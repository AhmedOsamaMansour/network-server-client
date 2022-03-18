using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        string[] Request3line;
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //throw new NotImplementedException();
            bool result= true;
            //TODO: parse the receivedRequest using the \r\n delimeter
            string[] str = {"\r\n"};
            Request3line = requestString.Split(str,StringSplitOptions.None);
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (Request3line.Length<3) { //3 line request , header , blank line
                return false;
            }
            requestLines = Request3line[0].Split(' ');
            // Parse Request line
            result &=ParseRequestLine(); 
            // Validate blank line exists
            result &=ValidateBlankLine();
            // Load header lines into HeaderLines dictionary
            result &=LoadHeaderLines();
            return result;           
        }

        private bool ParseRequestLine()
        {
            //throw new NotImplementedException();
            // uri , method , http ver
            if (requestLines.Length < 3) { 
                return false;
            }
            else {
                    if (requestLines[2] == "HTTP/1.1") { httpVersion = HTTPVersion.HTTP11; }
                    else { return false; }

                    }
                    switch (requestLines[0].ToUpper())
                    {
                        case "GET":
                            method = RequestMethod.GET;
                            break;
                        default:
                            return false;
                    }
                    relativeURI = requestLines[1]; // URI
                    return ValidateIsURI(relativeURI);
            }

        private bool ValidateIsURI(string uri)
        {
            
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            //throw new NotImplementedException();
             bool res = true;
             int i =1;
             headerLines = new Dictionary<string, string>();
             while (i<Request3line.Length-2) { //السطر بتاعي مش ب null
                //بجيب الهيدر من الديكشيناري
                // Load header lines into HeaderLines dictionary
                if(Request3line[i].Contains(":"))
                { 
                    string[] Splitt = {": "};
                    string[] data = Request3line[i].Split(Splitt,StringSplitOptions.None);
                    headerLines.Add(data[0],data[1]); //key :value
                }
                else res = false;
                i++;
            }   
            
            return res;
        }

        private bool ValidateBlankLine()
        {
            //throw new NotImplementedException();
            // blank line check
             if (Request3line[(Request3line.Length-2)]=="") 
            { 
                return true;

            }
            else 
            { 
                return false;
            }
        }

    }
}
