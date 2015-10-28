using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Time_Client.utils;

namespace Time_Client.client
{
    /// <summary>
    /// Communicating agent with the server
    /// </summary>
    public class ConnectionToServer
    {
        // create a Tcp socket  to connect to server
        private static TcpClient _clientSocket = null;
        Socket connection = null; //The socket that is listened to 
        TcpListener listener = null;
        private static BinaryWriter writer;
        private Parser parser;
        private static NetworkStream stream = null;

        private Game game = null;
        bool errorOcurred = false;     
        int attempt;

        public ConnectionToServer(){}

        public ConnectionToServer(Game game)
        {
            this.game = game;
            this.parser = new Parser(game);
        }

        /// <summary>
        /// connecting to the server socket
        /// </summary>
        public void sendJOINrequest()
        {  
            sendData("JOIN#");
        }


        public  void receiveData()
        {

             try
            {
                //Creating listening Socket
                this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);

                Console.WriteLine("waiting for server response");

                //Starts listening
                this.listener.Start();
                //Establish connection upon server request
                while (true)
                {
                    //connection is connected socket
                    connection = listener.AcceptSocket();   

                    //Fetch the messages from the server
                    int asw = 0;

                    //create a network stream using connection
                    NetworkStream serverStream = new NetworkStream(connection);
                    List<Byte> inputStr = new List<byte>();

                    // fetch messages from  server
                    while (asw != -1)
                    {
                        asw = serverStream.ReadByte();
                        inputStr.Add((Byte)asw);
                    }

                    String messageFromServer = Encoding.UTF8.GetString(inputStr.ToArray());
                    messageFromServer = messageFromServer.Remove(messageFromServer.Length - 1);
                  
                    Console.Clear();
                    parser.parse(messageFromServer);
                    Console.WriteLine("\n");
                    parser.tokenizer.printBoard();

                    try
                    {

                        for (int i = 0; i < game.totalPlayers-1; i++)
                        {
                            Console.WriteLine(game.player[i].toString() + "\n");

                        }
                    }
                    catch (Exception)
                    {}
                
                    Console.WriteLine("Server messege:- " + messageFromServer + "\n");

                    // close the netork stream
                    serverStream.Close();       

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (RECEIVING) Failed! \n " + e.Message);
                errorOcurred = true;
            }
            finally
            {
                if (connection != null)
                    if (connection.Connected)
                        connection.Close();
                if (errorOcurred)
                   receiveData();
            }
        }

                   
        


        /// <summary>
        /// method to send data to an already connected server
        /// </summary>
        /// <param name="data"></param>
        public  void sendData(String data)
        {
            
            try
            {
                _clientSocket = new TcpClient(); //the even number bux fix


                _clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 6000);

                //Console.WriteLine("This is broadcasting00");
                if (_clientSocket.Connected)
                {
                    //To write to the socket
                    stream = _clientSocket.GetStream();

                    //Create objects for writing across stream
                    writer = new BinaryWriter(stream);
                    Byte[] tempStr = Encoding.ASCII.GetBytes(data);

                    //writing to the port                
                    writer.Write(tempStr);
                    //Console.WriteLine("This is broadcasting");
                    writer.Close();
                    stream.Close();

                }
            }
            catch (Exception e)
            {
                attempt ++;
                Console.Clear();
                Console.WriteLine("Sending data to server failed due to " + e.Message);
                Console.WriteLine("Attempt "+ attempt+" to send data to server.....");
                sendData(data);
            }
          

    }
}
}

