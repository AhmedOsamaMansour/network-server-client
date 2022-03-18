using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;
        int port;
        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            this.port = portNumber;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostendpoint = new IPEndPoint(IPAddress.Any, port);
            serverSocket.Bind(hostendpoint);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(100);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket client = this.serverSocket.Accept();
                Thread thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                thread.Start(client);
                
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            // TODO: receive requests in while true until remote client closes the socket.
            Socket clientSocket = (Socket)obj;
            clientSocket.ReceiveTimeout = 0;
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] recievedata = new byte[100000];
                    int recievelen = clientSocket.Receive(recievedata);
                    string dataa = Encoding.ASCII.GetString(recievedata);
                    // TODO: break the while loop if receivedLen==0
                    if (recievelen == 0)
                        break;
                    // TODO: Create a Request object using received request string
                    Request request = new Request(dataa);
                    // TODO: Call HandleRequest Method that returns the response
                    Response serverresponse = HandleRequest(request);
                    string res = serverresponse.ResponseString;
                    byte[] response = Encoding.ASCII.GetBytes(res);
                    // TODO: Send Response back to client
                    clientSocket.Send(response);

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string content;
            try
            {

                //TODO: check for bad request   
                if (!request.ParseRequest())
                {
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    return new Response(StatusCode.BadRequest, "text/html", content, "");
                }
                  if(request.relativeURI=="/")
                    request.relativeURI="main.html";

                //TODO: map the relativeURI in request to get the physical path of the resource.
                string phyPath = Configuration.RootPath + request.relativeURI;
                //TODO: check for redirect
                string redirectionPath = GetRedirectionPagePathIFExist(request.relativeURI);
                if (!String.IsNullOrEmpty(redirectionPath))//redirection
                {
                    //content = LoadDefaultPage(Configuration.RedirectionDefaultPageName);
                    phyPath = Configuration.RootPath + "/" + redirectionPath;
                    content = LoadDefaultPage(redirectionPath);//
                    return new Response(StatusCode.Redirect, "text/html", content, redirectionPath);
                }

                //TODO: check file exists
                int test;
                test= LoadDefaultPage(request.relativeURI).Length;
                if (test==0 )
                {
                    content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    return new Response(StatusCode.NotFound, "text/html", content, "");
                }

                //TODO: read the physical file
                

                // Create OK response
                content= LoadDefaultPage(request.relativeURI);
                return new Response(StatusCode.OK, "text/html", content, "");
            }

            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                return new Response(StatusCode.InternalServerError, "html", content, "");
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            if (Configuration.RedirectionRules.ContainsKey(relativePath))
                return Configuration.RedirectionRules[relativePath];
            return string.Empty;

            
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            char h = defaultPageName[0];
            string [] splitt;
            if (h == '/')
            {
                splitt =defaultPageName.Split('/');
                defaultPageName= splitt[1];

            }
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            if (!File.Exists(filePath)) { 
                Logger.LogException(new FileNotFoundException("cannot find the file", filePath));
            return string.Empty;
            }
            // else read file and return its content
            string s= File.ReadAllText(filePath);
            return s;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                string[] RulesArr = File.ReadAllLines(filePath);

                Configuration.RedirectionRules = new Dictionary<string, string>();
                // then fill Configuration.RedirectionRules dictionary 
                for (int i = 0; i < RulesArr.Length; i++)
                {
                    string[] rule = RulesArr[i].Split(',');
                    Configuration.RedirectionRules.Add(rule[0], rule[1]);
                }
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        
    }
}
    }
