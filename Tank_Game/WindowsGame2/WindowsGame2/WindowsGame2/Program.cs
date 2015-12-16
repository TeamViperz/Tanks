using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsGame2.Gui;
using WindowsGame2.serverClientConnection;
using WindowsGame2.GameEngine;
using System.Diagnostics;
using System.Threading;
namespace WindowsGame2
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Make a new Game
            Game2 game = new Game2();
            Game1 game3 = new Game1();

            // Create a new gateway to communicate with the server
            ConnectionToServer myConnection = new ConnectionToServer(game);

            //Console.Title = "Client";

            // Send initial join request to the server.
            myConnection.sendJOINrequest();
            
            //game3.Run();
            
            using (Game1 game2 = new Game1())
            {
                game2.Run();
                /*Thread thread = new Thread(new ThreadStart(() => myConnection.receiveData()));
                thread.IsBackground = true;
                thread.Start();*/
            }


            // Create a new thread to handle incoming traffic from server
            
            
           // Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(game3.Run());

           

            // Create a new thread to handle incoming traffic from server
            
        }
        
    }
#endif
}

