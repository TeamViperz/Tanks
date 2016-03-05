using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WindowsGame2.Gui;
using WindowsGame2.serverClientConnection;

namespace WindowsGame2.GameEngine
{
    public class GameConsole
    {


       /* static void Main(string[] args)
        {
            // Make a new Game
            Game game = new Game();

            // Create a new gateway to communicate with the server
            ConnectionToServer myConnection = new ConnectionToServer(game);

            Console.Title = "Client";

            Console.WriteLine("running"); 
            //using (Game1 game2 = new Game1())
            //{
             //   game2.Run();
            //}
            // Send initial join request to the server.
            myConnection.sendJOINrequest();

            // Create a new thread to handle incoming traffic from server
            Thread thread = new Thread(new ThreadStart(() => myConnection.receiveData()));
           thread.Start();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Joystick(myConnection));


        }*/

    }
}
