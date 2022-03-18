using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }
    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
             //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            headerLines.Add(contentType);
            headerLines.Add(content.Length.ToString());
            headerLines.Add(DateTime.Now.ToString("ddd , dd MMM yyyy  HH : mm : ss 'EST'"));
            string s = GetStatusLine(code);

            // TODO: Create the request string
            if(code== StatusCode.Redirect)  //redirection has the location
            {
                headerLines.Add(redirectoinPath);
                responseString = s  + "content type" + ": " + headerLines[0] + "\r\n" +
                    "content length" + ": " + headerLines[1] + "\r\n" +
                    "Date" + ": " + headerLines[2] + "\r\n" +
                    "location" + ": " + headerLines[3] + "\r\n"+
                    "\r\n"+
                     content;
            }
            else
            {
                responseString = s + "content type" + ": " + headerLines[0] + "\r\n" +
                    "content length" + ": " + headerLines[1] + "\r\n" +
                    "Date" + ": " + headerLines[2] + "\r\n" +
                     "\r\n"+
                     content;            
            }
        }
        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            if(code==StatusCode.OK)
            {
                statusLine = "HTTP/1.1 " + "200 " + "ok"+"\r\n";
            }
            else if(code == StatusCode.Redirect)
            {
                statusLine = "HTTP/1.1 " + "301 " + "Redirect"+"\r\n";
            }
            else if(code == StatusCode.BadRequest)
            {
                statusLine = "HTTP/1.1 " + "400 " + "Bad request"+"\r\n";
            }
            else if (code == StatusCode.NotFound)
            {
                statusLine = "HTTP/1.1 " + "404 " + "Not Found"+"\r\n";
            }
            else if(code == StatusCode.InternalServerError)
            {
                statusLine = "HTTP/1.1 " + "500 " + "Internal Server Error"+"\r\n";
            }
            return statusLine;
        }
    }
}