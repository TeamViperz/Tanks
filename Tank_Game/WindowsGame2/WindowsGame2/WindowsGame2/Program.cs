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
            
            using (Game1 game2 = new Game1())
            {
                game2.Run();  
                
            }
            
        }
        
    }
#endif
}

