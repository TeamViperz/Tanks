using System;
using System.Threading;
using System.Windows.Forms;
using Time_Client.client;
using Time_Client.Gui;

namespace Time_Client.GameEngine
{
    public class GameConsole
    {
        

        static void Main(string[] args)
        {
            // Make a new Game
            Game game = new Game();
             
            // Create a new gateway to communicate with the server
            ConnectionToServer myConnection = new ConnectionToServer(game);

            Console.Title = "Client";

            // Send initial join request to the server.
            myConnection.sendJOINrequest();

            // Create a new thread to handle incoming traffic from server
            Thread thread = new Thread(new ThreadStart(() => myConnection.receiveData()));
            thread.Start();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Joystick(myConnection));
       
           
        }

    }
}
