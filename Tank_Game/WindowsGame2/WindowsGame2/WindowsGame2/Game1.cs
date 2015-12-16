using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WindowsGame2.serverClientConnection;
using WindowsGame2.GameEngine;
using System.Threading;

namespace WindowsGame2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Texture2D backgroundTexture;
        Texture2D foregroundTexture;
        int screenWidth;
        int screenHeight;
        int topMargin = 60;
        int leftMargin = 60;
        ConnectionToServer myConnection;

        int gridCellSize = 50;
        Vector2 position = Vector2.Zero;
        Texture2D gridTexture;
        Texture2D tankTexture;
        Texture2D bulletTexture;
        public bulletData bullet = new bulletData();

        Vector2 rocketDirection;
        public gridCellData[,] gridCell = new gridCellData[10, 10];
        public tankData[] tank = new tankData[5];
        public struct gridCellData
        {
            public int verticalPosition;
            public int horizontalPosition;
            public bool occupied;
        }

        public struct tankData
        {
            public int verticalPosition;
            public int horizontalPosition;
            public float angle;
            public String direction;
            public bool isMoving;

        }
        public struct bulletData
        {
            public string direction;
            public int verticalPos;
            public int horizontalPos;
            public bool isFlying;
        }

        int[,] map = new int[,]
        {
            {1, 1, 0,},
            {0, 1, 1,},
            {1, 1, 0,},
        };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            bullet.verticalPos = topMargin + gridCellSize;
            bullet.horizontalPos = leftMargin + 3 * gridCellSize;
            bullet.isFlying = false;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Riemer's 2D XNA Tutorial";

            Game2 game = new Game2();
            //Game1 game3 = new Game1();

            // Create a new gateway to communicate with the server
           myConnection = new ConnectionToServer(game);

            //Console.Title = "Client";

            // Send initial join request to the server.
            myConnection.sendJOINrequest();
            //game3.Run();
            /*using (Game1 game2 = new Game1())
            {
                game2.Run();
            }*/


            // Create a new thread to handle incoming traffic from server
            //myConnection.receiveData();
           // Thread thread = new Thread(new ThreadStart(() => myConnection.receiveData()));
           // thread.Start();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            Console.WriteLine(MathHelper.ToRadians(90));

            backgroundTexture = Content.Load<Texture2D>("background");
            foregroundTexture = Content.Load<Texture2D>("foreground");
            gridTexture = Content.Load<Texture2D>("cell1");
            tankTexture = Content.Load<Texture2D>("tank3");
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            setUpGrid();
            //drawTank();
            setUpTanks();


        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            ProcessKeyboard();
            launchRocket();
            //myConnection.receiveData();
            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //DrawScenery();
            drawGrid();
            drawTank();
            drawBullet();

            //spriteBatch.Draw(backgroundTexture, new Rectangle(24, 28, 45, 45), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(20, 20, gridCellSize, gridCellSize);
            Rectangle screenRectangle1 = new Rectangle(70, 20, gridCellSize, gridCellSize);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.Yellow);
            spriteBatch.Draw(foregroundTexture, screenRectangle, Color.White);
            spriteBatch.Draw(backgroundTexture, screenRectangle1, Color.Yellow);
            spriteBatch.Draw(foregroundTexture, screenRectangle1, Color.White);
        }
        private void drawGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {

                    spriteBatch.Draw(gridTexture, new Rectangle(gridCell[i, j].horizontalPosition, gridCell[i, j].verticalPosition, gridCellSize, gridCellSize), Color.White);
                    if (i == 0 && j == 4)
                    {
                        spriteBatch.Draw(tankTexture, new Rectangle(gridCell[i, j].horizontalPosition, gridCell[i, j].verticalPosition, gridCellSize, gridCellSize), Color.White);
                    }


                }
            }

        }
        private void setUpGrid()
        {
            Console.WriteLine("innn");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gridCell[i, j].horizontalPosition = leftMargin + j * gridCellSize;
                    gridCell[i, j].verticalPosition = topMargin + i * gridCellSize;
                    if (j == 9)
                    {
                        Console.Write(j);
                        Console.Write("\n");
                    }
                    else { Console.Write(j); }
                }

            }
        }

        public void launchRocket()
        {
            if (bullet.isFlying)
            {
                int row = (bullet.verticalPos - topMargin) / gridCellSize;
                int column = (bullet.horizontalPos - leftMargin) / gridCellSize;
                if (bullet.direction == "up")
                {
                    if (bullet.verticalPos > topMargin)
                    {
                        if (!gridCell[column, row - 1].occupied)
                        {
                            bullet.verticalPos -= gridCellSize / 10;
                        }
                    }
                }
                if (bullet.direction == "down")
                {
                    Console.WriteLine("in:" + (topMargin + gridCellSize * 9));
                    Console.WriteLine("in2:" + bullet.verticalPos);
                    if (bullet.verticalPos < topMargin + gridCellSize * 9)
                    {
                        if (!gridCell[column, row].occupied)
                        {
                            bullet.verticalPos += gridCellSize;
                        }
                    }

                }
                if (bullet.direction == "left")
                {
                    if (bullet.horizontalPos > leftMargin)
                    {
                        if (!gridCell[column - 1, row].occupied)
                        {
                            bullet.horizontalPos -= gridCellSize;
                        }
                    }
                }
                if (bullet.direction == "right")
                {
                    if (bullet.horizontalPos < leftMargin + gridCellSize * 9)
                    {
                        if (!gridCell[column + 1, row].occupied)
                        {
                            bullet.horizontalPos += gridCellSize / 40;
                        }
                    }
                }



            }
        }
        private void setUpTanks()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(i + i);
                tank[i].horizontalPosition = leftMargin + i * gridCellSize;
                tank[i].verticalPosition = topMargin + i * gridCellSize;
                gridCell[i, i].occupied = true;
                tank[i].angle = 0;
            }
        }
        private void drawTank()
        {
            for (int k = 0; k < 5; k++)
            {
                //Console.WriteLine(1+1);
                spriteBatch.Draw(tankTexture, new Vector2(tank[k].horizontalPosition + gridCellSize / 2, tank[k].verticalPosition + gridCellSize / 2), null, Color.White, tank[k].angle, new Vector2(gridCellSize / 2, gridCellSize / 2), 1, SpriteEffects.None, 1);
                //spriteBatch.Draw(tankTexture, new Vector2(tank[k].horizontalPosition, tank[k].verticalPosition), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            }
        }
        private void drawBullet()
        {
            spriteBatch.Draw(tankTexture, new Vector2(bullet.horizontalPos, bullet.verticalPos), Color.White);
        }
        private void ProcessKeyboard()
        {
            KeyboardState keybState = Keyboard.GetState();

            if (keybState.IsKeyDown(Keys.Up))
            {
                tank[1].angle = MathHelper.ToRadians(90);
                Console.WriteLine("up pressed");
                //rocketAngle = players[currentPlayer].Angle;
                tank[1].isMoving = true;
                //tank[1].verticalPosition += gridCellSize;

                // Vector2 up = new Vector2(0, -1);
                // Matrix rotMatrix = Matrix.CreateRotationZ(tank[1].angle);
                // rocketDirection = Vector2.Transform(up, rotMatrix);
            }
            if (keybState.IsKeyDown(Keys.Space))
            {
                //if (tank[1].isMoving) {
                bullet.direction = "right";
                bullet.isFlying = true;
                Console.WriteLine("pressed");

                ///}
            }


        }


    }
    }

