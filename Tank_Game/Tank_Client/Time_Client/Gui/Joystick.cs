using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Time_Client.client;

namespace Time_Client.Gui
{
    public partial class Joystick : Form
    {
        private ConnectionToServer connectionToServer;
        public Joystick()
        {
            InitializeComponent();
        }

        public Joystick(ConnectionToServer connectionToServer)
        {
            this.connectionToServer = connectionToServer;
            InitializeComponent();

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            connectionToServer.sendData("UP#");
        }

        private void btnShoot_Click(object sender, EventArgs e)
        {
            connectionToServer.sendData("SHOOT#");
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            connectionToServer.sendData("DOWN#");
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            connectionToServer.sendData("RIGHT#");
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            connectionToServer.sendData("LEFT#");
        }


    

        private void Joystick_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 65)
            {
                //left arrow key is pressed
                connectionToServer.sendData("LEFT#");

            }
            if (e.KeyValue == 87)
            {
                //up arrow key is pressed
                connectionToServer.sendData("UP#");
            }
            if (e.KeyValue ==68)
            {
                //right arrow key is pressed
                connectionToServer.sendData("RIGHT#");
            }
            if (e.KeyValue == 88)
            {
                //down arrow key is pressed
                connectionToServer.sendData("DOWN#");
            }
            if (e.KeyValue == 83)
            {
                //space key is pressed
                connectionToServer.sendData("SHOOT#");

            }
        }


        



    }
}
