using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Threading;
using WindowsGame2.ai;
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
      


        private static BinaryWriter writer;
        private Parser parser;
        private static NetworkStream stream = null;

        private Game2 game = null;
        bool errorOcurred = false;
        int attempt;
        private Thread thread;

        private Cell nextMove;
        private Ai ai;
        private bool targetPresents = false;

        public ConnectionToServer() { }

        public ConnectionToServer(Game2 game)
        {
            this.game = game;
            this.parser = new Parser(game);
           thread = new Thread(new ThreadStart(receiveData));
            ai = new Ai(game);
           
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

                    

                    // path finding starts here
                    if (messageFromServer != null)
                {
                    if (messageFromServer.StartsWith("G"))
                    {
                            // to keep syn the game clock with server clock
                            game.gameClock += 1;

                            Console.WriteLine(game.gameClock);
                           // game.updateLifePacks(game.gameClock);
                           // game.addLifePacksToBoard();

                            Console.WriteLine("\n");


                            // Print the map (Game board) on the Console
                            parser.tokenizer.printBoard();

                            Console.WriteLine("\n");

                            //path finding demo
                            nextMove = ai.findPath(game);

                            int currentX = game.player[0].playerLocationX;
                            int currentY = game.player[0].playerLocationY;
                            Console.WriteLine("\nCurrentX:- " + currentX + " CurrentY:- " + currentY + "\n");
                            Console.WriteLine("\nNextX:- " + nextMove.x + " NextY:- " + nextMove.y + "\n");

                            if (nextMove.x != currentX || nextMove.y != currentY) { targetPresents = true; }

                            // eg:- initialy tank direction is up, it wants to go right... timeCostToTarget is lack of the time to turn right... has to fix this.             
                            Console.WriteLine(game.timeCostToTarget);

                            if (targetPresents)
                            {
                                // move the tank
                                if (nextMove.x == currentX + 1)
                                {
                                    sendData("RIGHT#");
                                }
                                else if (nextMove.x == currentX - 1)
                                {
                                    sendData("LEFT#");
                                }
                                else if (nextMove.y == currentY + 1)
                                {
                                    sendData("DOWN#");
                                }
                                else if (nextMove.y == currentY - 1)
                                {
                                    sendData("UP#");
                                }
                                targetPresents = false;
                            }
                        }
                }
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
