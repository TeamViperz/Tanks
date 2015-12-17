using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Threading;
//using System.Windows.Forms.VisualStyles;
using WindowsGame2.utils;
using WindowsGame2.GameEngine;

namespace WindowsGame2.serverClientConnection
{
    public class ConnectionToServer
    {
        // create a Tcp socket  to connect to server
        private static TcpClient _clientSocket = null;

      
        TcpListener listener = null;
        TcpClient reciever = null;
        Stream r_stream = null;
        Class1 cl;


        private static BinaryWriter writer;
        private Parser parser;
        private static NetworkStream stream = null;

        private Game2 game = null;
        bool errorOcurred = false;
        int attempt;
        private Thread thread;

        public ConnectionToServer() { }

        public ConnectionToServer(Game2 game)
        {
            this.game = game;
            this.parser = new Parser(game);
           thread = new Thread(new ThreadStart(receiveData));
           cl = new Class1();
            Console.WriteLine("constructor");
        }

        /// <summary>
        /// connecting to the server socket
        /// </summary>
        public void sendJOINrequest()
        {
            sendData("JOIN#");
            thread.Start();
        }

        /// <summary>
        /// To fetch data from server
        /// </summary>
        public void receiveData()
        {
            

            try { 
            Console.WriteLine("recieving");

            //Creating listening Socket
            this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);

            Console.WriteLine("waiting for server response");

            String messageFromServer;
            
            //Establish connection upon server request
            while (true)
            {
                //Starts listening
                listener.Start();
                reciever = listener.AcceptTcpClient();
                r_stream = reciever.GetStream();
                Byte[] bytes = new Byte[256];

                int i;
                messageFromServer = null;
                Console.WriteLine("recieving1");


                while ((i = r_stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    messageFromServer = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                }

                //parser.tokenizer(messageFromServer);
                parser.parse(messageFromServer);
                Thread thread = new Thread(new ThreadStart(() => cl.move(this)));
                thread.Start();
                thread.Join();
                // Print the raw message from the server
                Console.WriteLine("\nServer messege:- " + messageFromServer + "\n");

            r_stream.Close();
            listener.Stop();
            reciever.Close();
        }
               
            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (RECEIVING) Failed! \n " + e.Message);
                errorOcurred = true;
            }
            finally
            {
                if (reciever != null)
                    if (reciever.Connected)
                        reciever.Close();
                if (errorOcurred)
                    receiveData();
            }
        }


        /// <summary>
        /// method to send data to an already connected server
        /// </summary>
        /// <param name="data"></param>
        public void sendData(String data)
        {
            try
            {
                // Create a new TCP client socket to send data to the server
                _clientSocket = new TcpClient();

                _clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 6000);

                if (_clientSocket.Connected)
                {
                    //To write to the socket
                    stream = _clientSocket.GetStream();

                    //Create objects for writing across stream
                    writer = new BinaryWriter(stream);
                    Byte[] tempStr = Encoding.ASCII.GetBytes(data);

                    //writing to the port                
                    writer.Write(tempStr);

                    writer.Close();
                    stream.Close();

                }
            }
            catch (Exception e)
            {
                attempt++;
                // Console.Clear();
                Console.WriteLine("Sending data to server failed due to " + e.Message);
                Console.WriteLine("Attempt " + attempt + " to send data to server.....");
                sendData(data);
            }


        }
    }
}
