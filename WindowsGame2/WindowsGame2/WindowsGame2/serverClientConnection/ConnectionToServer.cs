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

        private Game2 game;
        private bool errorOcurred;
        int attempt;
        private Thread thread;

        private Cell nextMove;
        private Ai ai;
        private bool packPresents;
       

        public ConnectionToServer() { }

        public ConnectionToServer(Game2 game)
        {
            this.game = game;
            this.parser = new Parser(game);
           thread = new Thread(new ThreadStart(receiveData));
            ai = new Ai(game);
            packPresents = false;
            errorOcurred = false;
           
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
        //    Console.WriteLine("recieving");

            //Creating listening Socket
                this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);

      //      Console.WriteLine("waiting for server response");

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
             //   Console.WriteLine("recieving1");


                while ((i = r_stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    messageFromServer = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                }

                
                parser.parse(messageFromServer);
               // game.addPacksToBoard();
                   


                    // path finding starts here
                    if (messageFromServer != null)
                {
                    if (messageFromServer.StartsWith("G"))
                    {
                            Console.WriteLine("inide G");
                            // to keep syn the game clock with server clock
                            game.gameClock += 1;

                            //  Console.WriteLine(game.gameClock);

                            game.addPacksToBoard();
                            game.updatePacks(game.gameClock);
                            

                            Console.WriteLine("\n");


                            // Print the map (Game board) on the Console
                            parser.tokenizer.printBoard();

                            Console.WriteLine("\n");

                            //path finding starts ( The whole ai business happens here..... )
                            nextMove = ai.findPath(game);

                            int currentX = game.me.playerLocationX;
                            int currentY = game.me.playerLocationY;
                            
                            Console.WriteLine("\nNextX:- " + nextMove.x + " NextY:- " + nextMove.y + "\n");

                            // Print the raw message from the server
                            Console.WriteLine("\nServer messege:- " + messageFromServer + "\n");

                            // no movements if there isn't a reachable goal on the board
                            if (nextMove.x != currentX || nextMove.y != currentY) { packPresents = true; }

                            // #### Detect an Enemy who can be shoot out ####
                            /* STEPS

                            1. A* search other players
                            2. filter the straight paths (enemy can see mee - I can see enemy) - using a new method in Ai class
                            3. cancel the other tasks
                            4. if health is low? don't engage (hide) : engage (shoot) face to face
                            
                            */

                        foreach (var enemy in game.player)
                        {
                                Console.WriteLine("my player no "+ game.myPlayerNumber + " Player Loc " + enemy.playerLocationX + "," + enemy.playerLocationY + "Player NO " + enemy.playerNumber);
                            }
                        if (game.enemyPresents)
                        {
                                Console.WriteLine("I can see an enemy !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                
                                int myDirection = game.player[game.myPlayerNumber].direction;
                                Console.WriteLine("My direction is " + myDirection);
                                // shoot
                                if (nextMove.x == currentX + 1)
                                {
                                    if (myDirection == 1)
                                    {
                                        sendData("SHOOT#");
                                       
                                        
                                    }
                                    // if Im gonna shoot, instead of protecting myself                            

                                    
                                    else  { sendData("RIGHT#"); }

                                    r_stream.Close();
                                    listener.Stop();
                                    reciever.Close();
                                    game.enemyPresents = false;
                                    continue;

                                }
                    
                                 else if (nextMove.x == currentX - 1 )
                                {
                                    if (myDirection == 3)
                                    {
                                        sendData("SHOOT#");
                                       
                                    }

                                    // if Im gonna shoot, instead of protecting myself                            

                                    
                                    else { sendData("LEFT#"); }

                                    r_stream.Close();
                                    listener.Stop();
                                    reciever.Close();
                                    game.enemyPresents = false;
                                    continue;
                                }
                                 else if (nextMove.y == currentY + 1)
                                {
                                    if (myDirection == 2)
                                    {
                                        sendData("SHOOT#");
                                       
                                    }

                                    // if Im gonna shoot, instead of protecting myself                            

                                   
                                    else { sendData("DOWN#"); }

                                    r_stream.Close();
                                    listener.Stop();
                                    reciever.Close();
                                    game.enemyPresents = false;
                                    continue;
                                }
                                 else if (nextMove.y == currentY - 1)
                                {
                                    if (myDirection == 0)
                                    {
                                        sendData("SHOOT#");
                                        
                                    }

                                    // if Im gonna shoot, instead of protecting myself                            

                                    else { sendData("UP#"); }

                                    r_stream.Close();
                                    listener.Stop();
                                    reciever.Close();
                                    game.enemyPresents = false;
                                    continue;

                                }
                                
                            game.enemyPresents = false;
                        }
                            

                            // TO DO:- initialy tank direction is up, it wants to go right... timeCostToTarget is lack of the time to turn right... has to fix this.             
                           

                            if (packPresents)
                            {
                                Console.WriteLine("inside pack presents");
                                Console.WriteLine(currentX+","+ currentY+ " next move:- " + nextMove.x+ "," + nextMove.y);
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
                                packPresents = false;
                            }
                        }
                }
               

            r_stream.Close();
            listener.Stop();
            reciever.Close();
        }


               
            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (RECEIVING) Failed!  " + e.Message+ "\n"+e.Source+ "\n" + e.Data);
                errorOcurred = true;
            }
            finally
            {
                
               
                if (reciever != null)
                    if (reciever.Connected)
                        reciever.Close();
               // if (errorOcurred)
                   // receiveData();
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
                    Console.WriteLine("Sentdata "+data);
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
