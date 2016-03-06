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
        SpriteFont font;
        ConnectionToServer myConnection;

        int gridCellSize = 50;
        Vector2 position = Vector2.Zero;
        Texture2D gridTexture;
        Texture2D blueTankTexture;
        Texture2D redTankTexture;
        Texture2D brownTankTexture;
        Texture2D greenTankTexture;
        Texture2D purpleTankTexture;
        Texture2D tankTexture;
        Texture2D bulletTexture1;
        Texture2D bulletTexture2;
        Texture2D stoneTexture;
        Texture2D brickTexture1;
        Texture2D brickTexture2;
        Texture2D brickTexture3;
        Texture2D brickTexture4;
        Texture2D brickTexture5;
        Texture2D coinTexture;
        Texture2D lifeTexture;
        Texture2D logoTexture;
        //Texture2D emptyTankTexture;
        Texture2D textArea;
        Texture2D waterTexture;
        int init = 0;
        Game2 game;
        public bulletData[] bullet = new bulletData[20];
        public brickData[] bricks = new brickData[20];
        int bulletIndex = 0;
        public int brickIndex = 0;
        public Color[] tankColours;
        public Color[] textCoulours;
        public Texture2D[] tanks;
        public gridCellData[,] gridCell = new gridCellData[10, 10];
        public tankData[] tank = new tankData[5];
        public struct gridCellData
        {
            public int verticalPosition;
            public int horizontalPosition;
            public bool occupied;
            public String occupiedBy;
        }

        public struct tankData
        {
            public int verticalPosition;
            public int horizontalPosition;
            public float angle;
            public int direction;
            public bool isMoving;
            public bool isEmpty;
            public Color tankColor;
            public int whetherShoot;
            public int damageLevel;
            public int health;
            public int points;
            public int coins;
            public Boolean isPlaying;
            public int terminate;
            public Texture2D tankTexture;

        }
        public struct bulletData
        {
            public int direction;
            public int verticalPos;
            public int horizontalPos;
            public bool isFlying;
            public Texture2D texture;
            public Color bulletColor;
        }

        public struct brickData
        {
            public int status;//0 if full, 1 if half, 2 if .25, 3 if vanished
            public int verticalLocation;
            public int horizontalLocation;
            public Texture2D brickTexture;
            public bool isFull;

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
            game = new Game2();
            myConnection = new ConnectionToServer(game);

        }

        protected override void Initialize()
        {
            myConnection.sendJOINrequest();
            graphics.PreferredBackBufferWidth = 1250;
            graphics.PreferredBackBufferHeight = 650;
            graphics.IsFullScreen = false;
            //tankTexture = Content.Load<Texture2D>("tank3");
            tankColours = new Color[] { Color.OrangeRed, Color.Plum, Color.LightGreen, Color.SkyBlue, Color.Chocolate };
            textCoulours = new Color[] { Color.Red, Color.Purple, Color.Green, Color.Blue, Color.Brown };

            for (int i = 0; i < 5; i++)
            {

                tank[i].isEmpty = true;
                tank[i].isPlaying = true;
                tank[i].tankColor = tankColours[i];
                // tank[i].tankTexture = tankTexture;
                tank[i].terminate = 0;
            }
            for (int i = 0; i < 5; i++) { bricks[i].isFull = false; }
            graphics.ApplyChanges();
            Window.Title = "TankFury";
            Console.WriteLine("running");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            // //Console.WriteLine(MathHelper.ToRadians(90));

            backgroundTexture = Content.Load<Texture2D>("background3");
            foregroundTexture = Content.Load<Texture2D>("foreground");
            gridTexture = Content.Load<Texture2D>("cell5");
            blueTankTexture = Content.Load<Texture2D>("tank3");
            redTankTexture = Content.Load<Texture2D>("redTank");
            brownTankTexture = Content.Load<Texture2D>("brownTank");
            greenTankTexture = Content.Load<Texture2D>("greenTank");
            purpleTankTexture = Content.Load<Texture2D>("purpleTank");
            tankTexture = Content.Load<Texture2D>("tank3");
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            waterTexture = Content.Load<Texture2D>("water");
            brickTexture1 = Content.Load<Texture2D>("brick_full");
            brickTexture2 = Content.Load<Texture2D>("brick_75");
            brickTexture3 = Content.Load<Texture2D>("brick_50");
            brickTexture4 = Content.Load<Texture2D>("brick_25");
            brickTexture5 = Content.Load<Texture2D>("empty");
            stoneTexture = Content.Load<Texture2D>("stone");
            bulletTexture1 = Content.Load<Texture2D>("rocket");
            bulletTexture2 = Content.Load<Texture2D>("empty");
            textArea = Content.Load<Texture2D>("textArea3");
            // emptyTankTexture = Content.Load<Texture2D>("empty");
            font = Content.Load<SpriteFont>("myFont");
            lifeTexture = Content.Load<Texture2D>("lifepack");
            coinTexture = Content.Load<Texture2D>("coin");
            logoTexture = Content.Load<Texture2D>("logo");

            setUpGrid();


        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            ProcessKeyboard();
            updateTank();
            updateBrickAndTanks();
            for (int i = 0; i < 20; i++) { launchRocket(i); }


            base.Update(gameTime);
        }
        public void drawArena()
        {
            Player player;
            brickIndex = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {

                    if (game.board[i, j] == "B" && init == 0)
                    {
                        gridCell[j, i].occupied = true;
                        gridCell[j, i].occupiedBy = "B";
                        bricks[brickIndex].horizontalLocation = j * gridCellSize + leftMargin;
                        bricks[brickIndex].verticalLocation = i * gridCellSize + topMargin;
                        bricks[brickIndex].status = 0;
                        bricks[brickIndex].brickTexture = brickTexture1;
                        bricks[brickIndex].isFull = true;
                        brickIndex++;
                        spriteBatch.Draw(brickTexture1, new Vector2(j * gridCellSize + leftMargin, i * gridCellSize + topMargin), Color.White);
                    }
                    if (game.board[i, j] == "W")
                    {
                        gridCell[j, i].occupied = true;
                        gridCell[j, i].occupiedBy = "W";
                        spriteBatch.Draw(waterTexture, new Vector2(j * gridCellSize + leftMargin, i * gridCellSize + topMargin), Color.White);
                    }
                    if (game.board[i, j] == "S")
                    {
                        gridCell[j, i].occupied = true;
                        gridCell[j, i].occupiedBy = "S";
                        spriteBatch.Draw(stoneTexture, new Vector2(j * gridCellSize + leftMargin, i * gridCellSize + topMargin), Color.White);
                    }
                    if (game.board[i, j] == "L")
                    {
                        spriteBatch.Draw(lifeTexture, new Vector2(j * gridCellSize + leftMargin, i * gridCellSize + topMargin), Color.White);
                    }
                    if (game.board[i, j] == "C")
                    {
                        spriteBatch.Draw(coinTexture, new Vector2(j * gridCellSize + leftMargin, i * gridCellSize + topMargin), Color.White);
                    }
                    if (game.board[i, j] == "X") { }
                    if (game.board[i, j] == "0")
                    {
                        //  Console.WriteLine("player 0: location:" + i + "," + j + " isempty:" + tank[0].isEmpty);
                        init = 1;
                        player = game.player[0];

                        if (!tank[0].isEmpty && tank[0].isPlaying)
                        {
                            if (gridCell[(tank[0].horizontalPosition - leftMargin) / gridCellSize, (tank[0].verticalPosition - topMargin) / gridCellSize].occupied)
                            {
                                //Console.WriteLine("rearranging the occupation at:row:" + (tank[0].horizontalPosition - leftMargin) / gridCellSize + " column:" + (tank[0].verticalPosition - topMargin) / gridCellSize);
                                gridCell[(tank[0].horizontalPosition - leftMargin) / gridCellSize, (tank[0].verticalPosition - topMargin) / gridCellSize].occupied = false;
                            }
                            if (tank[0].health == 0)
                            {
                                //tank[0].tankTexture = emptyTankTexture;
                                tank[0].isPlaying = false;
                            }

                        }

                        if (tank[0].isPlaying)
                        {

                            tank[0].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                            tank[0].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                            tank[0].direction = player.direction;
                            gridCell[j, i].occupied = true;
                            tank[0].angle = MathHelper.ToRadians((player.direction + 1) * 90);
                            tank[0].isEmpty = false;

                            gridCell[j, i].occupiedBy = "0";
                        }


                    }
                    if (game.board[i, j] == "1")
                    {
                        player = game.player[1];

                        if (!tank[1].isEmpty && tank[1].isPlaying)
                        {
                            if (gridCell[(tank[1].horizontalPosition - leftMargin) / gridCellSize, (tank[1].verticalPosition - topMargin) / gridCellSize].occupied)
                            {
                                gridCell[(tank[1].horizontalPosition - leftMargin) / gridCellSize, (tank[1].verticalPosition - topMargin) / gridCellSize].occupied = false;
                            }
                            if (tank[1].health == 0)
                            {

                                tank[1].isPlaying = false;
                                //tank[1].tankTexture = emptyTankTexture;
                                //Console.WriteLine("insid tank1 1:isplaying:" + tank[1].isPlaying + "row:" + (tank[1].verticalPosition - topMargin) / gridCellSize + "column:" + (tank[1].horizontalPosition - leftMargin) / gridCellSize);
                            }
                        }

                        if (tank[1].isPlaying)
                        {

                            tank[1].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                            tank[1].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                            tank[1].direction = player.direction;
                            gridCell[j, i].occupied = true;
                            tank[1].angle = MathHelper.ToRadians((player.direction + 1) * 90);
                            tank[1].isEmpty = false;
                            gridCell[j, i].occupiedBy = "1";
                        }

                    }
                    if (game.board[i, j] == "2")
                    {
                        player = game.player[2];

                        if (!tank[2].isEmpty && tank[2].isPlaying)
                        {
                            //Console.WriteLine("inside tank 2:health:" + tank[2].health);
                            if (gridCell[(tank[2].horizontalPosition - leftMargin) / gridCellSize, (tank[2].verticalPosition - topMargin) / gridCellSize].occupied)
                            {
                                gridCell[(tank[2].horizontalPosition - leftMargin) / gridCellSize, (tank[2].verticalPosition - topMargin) / gridCellSize].occupied = false;
                            }
                            if (tank[2].health == 0)
                            {

                                tank[2].isPlaying = false;
                                //tank[2].tankTexture = emptyTankTexture;
                                //Console.WriteLine("insid tank2 2:isplaying:" + tank[2].isPlaying + "row:" + (tank[2].verticalPosition - topMargin) / gridCellSize + "column:" + (tank[2].horizontalPosition - leftMargin) / gridCellSize);
                            }
                        }

                        if (tank[2].isPlaying)
                        {

                            tank[2].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                            tank[2].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                            tank[2].direction = player.direction;
                            gridCell[j, i].occupied = true;
                            tank[2].angle = MathHelper.ToRadians((player.direction + 1) * 90);
                            tank[2].isEmpty = false;
                            gridCell[j, i].occupiedBy = "2";
                            //Console.WriteLine("tank 2 occupied: row:" + j + "column:" + i + "occupied:" + gridCell[i, j].occupied);
                        }
                    }
                    if (game.board[i, j] == "3")
                    {
                        player = game.player[3];

                        if (!tank[3].isEmpty && tank[3].isPlaying)
                        {
                            if (gridCell[(tank[3].horizontalPosition - leftMargin) / gridCellSize, (tank[3].verticalPosition - topMargin) / gridCellSize].occupied)
                            {
                                gridCell[(tank[3].horizontalPosition - leftMargin) / gridCellSize, (tank[3].verticalPosition - topMargin) / gridCellSize].occupied = false;
                            }
                            if (tank[3].health == 0)
                            {
                                tank[3].isPlaying = false;
                                //tank[4].tankTexture = emptyTankTexture;
                                //Console.WriteLine("insid tank3 3:isplaying:" + tank[3].isPlaying + "row:" + (tank[3].verticalPosition - topMargin) / gridCellSize + "column:" + (tank[3].horizontalPosition - leftMargin) / gridCellSize);
                            }
                        }
                        if (tank[3].isPlaying)
                        {
                            tank[3].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                            tank[3].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                            tank[3].direction = player.direction;
                            gridCell[j, i].occupied = true;
                            tank[3].angle = MathHelper.ToRadians((player.direction + 1) * 90);
                            tank[3].isEmpty = false;
                            gridCell[j, i].occupiedBy = "3";
                        }
                    }
                    if (game.board[i, j] == "4")
                    {
                        player = game.player[4];

                        if (!tank[4].isEmpty && tank[4].isPlaying)
                        {
                            if (gridCell[(tank[4].horizontalPosition - leftMargin) / gridCellSize, (tank[4].verticalPosition - topMargin) / gridCellSize].occupied)
                            {
                                gridCell[(tank[4].horizontalPosition - leftMargin) / gridCellSize, (tank[4].verticalPosition - topMargin) / gridCellSize].occupied = false;
                            }
                            if (tank[4].health == 0)
                            {
                                tank[4].isPlaying = false;
                                //tank[4].tankTexture = emptyTankTexture;
                                //Console.WriteLine("insid tank4 4:isplaying:" + tank[4].isPlaying + "row:" + (tank[4].verticalPosition - topMargin) / gridCellSize + "column:" + (tank[4].horizontalPosition - leftMargin) / gridCellSize);
                            }
                        }
                        if (tank[4].isPlaying)
                        {
                            tank[4].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                            tank[4].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                            tank[4].direction = player.direction;
                            gridCell[j, i].occupied = true;
                            tank[4].angle = MathHelper.ToRadians((player.direction + 1) * 90);
                            tank[4].isEmpty = false;
                            gridCell[j, i].occupiedBy = "4";
                        }
                    }

                }

            }

        }



        public void drawBricks()
        {
            for (int i = 0; i < game.brickLen; i++)
            {
                spriteBatch.Draw(bricks[i].brickTexture, new Vector2(bricks[i].horizontalLocation, bricks[i].verticalLocation), Color.White);
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 1250, 750), Color.White);
            spriteBatch.Draw(logoTexture, new Rectangle(60, 600, 300, 30), Color.White);
            drawGrid();
            //drawTank();
            DrawText();

            //for (int i = 0; i < 20; i++) { drawBullet(i); }
            drawArena();
            for (int i = 0; i < 20; i++) { drawBullet(i); }
            if (init == 1) { drawBricks(); }
            drawTank();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawText()
        {
            spriteBatch.Draw(textArea, new Rectangle(780, 60, 400, 400), Color.White);
            spriteBatch.DrawString(font, "My player number:" + game.myPlayerNumber, new Vector2(900, 90), Color.Black);
            for (int i = 0; i < 5; i++)
            {
                //spriteBatch.DrawString(font, "Player:" + i.ToString(), new Vector2(800, 120 + 65 * i), textCoulours[i]);
                spriteBatch.DrawString(font, i.ToString(), new Vector2(860, 200 + 42 * i), textCoulours[i]);
                if (!tank[i].isEmpty)
                {

                    //spriteBatch.DrawString(font, "Points:" + tank[i].points.ToString(), new Vector2(800, 145 + 65 * i), textCoulours[i]);
                    //spriteBatch.DrawString(font, "Health:" + tank[i].health.ToString(), new Vector2(925, 145 + 65 * i), textCoulours[i]);
                    //spriteBatch.DrawString(font, "Coins:" + tank[i].points.ToString(), new Vector2(1050, 145 + 65 * i), textCoulours[i]);
                    spriteBatch.DrawString(font, tank[i].points.ToString(), new Vector2(925, 200 + 42 * i), textCoulours[i]);
                    spriteBatch.DrawString(font, tank[i].health.ToString(), new Vector2(1010, 200 + 42 * i), textCoulours[i]);
                    spriteBatch.DrawString(font, tank[i].points.ToString(), new Vector2(1085, 200 + 42 * i), textCoulours[i]);
                }

            }

        }
        private void drawGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {

                    spriteBatch.Draw(gridTexture, new Rectangle(gridCell[i, j].horizontalPosition, gridCell[i, j].verticalPosition, gridCellSize, gridCellSize), Color.White);



                }
            }

        }
        private void setUpGrid()
        {
            //Console.WriteLine("innn");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gridCell[i, j].horizontalPosition = leftMargin + j * gridCellSize;
                    gridCell[i, j].verticalPosition = topMargin + i * gridCellSize;
                    if (j == 9)
                    {
                        //Console.Write(j);
                        //Console.Write("\n");
                    }
                    else
                    { //Console.Write(j);
                    }
                }

            }
        }

        public void launchRocket(int i)
        {
            if (bullet[i].isFlying)
            {
                //Console.WriteLine("inside launch");
                int row = (bullet[i].verticalPos - topMargin) / gridCellSize;
                int column = (bullet[i].horizontalPos - leftMargin) / gridCellSize;
                if (bullet[i].direction == 0)
                {
                    if (bullet[i].verticalPos > topMargin + gridCellSize)
                    {
                        if (!gridCell[column, row - 1].occupied || gridCell[column, row - 1].occupiedBy == "W")
                        {
                            bullet[i].verticalPos -= gridCellSize / 5;
                        }
                        else
                        {

                            bullet[i].isFlying = false;
                            bullet[i].texture = bulletTexture2;
                            updateBrickOccupied(column, row - 1);

                        }
                    }
                    else
                    {
                        bullet[i].isFlying = false;
                        bullet[i].texture = bulletTexture2;
                    }
                }
                if (bullet[i].direction == 2)
                {
                    //Console.WriteLine("inside direction 2");
                    if (bullet[i].verticalPos < topMargin + gridCellSize * 9)
                    {
                        //Console.WriteLine("inside if");
                        if (!gridCell[column, row + 1].occupied || gridCell[column, row + 1].occupiedBy == "W")
                        {
                            //Console.WriteLine("grid cell not occupied row:" + (row + 1) + "column:" + column);
                            bullet[i].verticalPos += gridCellSize / 5;
                        }
                        else
                        {
                            //Console.WriteLine("grid cell row:" + row + "column" + "occcupied by:" + gridCell[column, row + 1].occupiedBy);
                            bullet[i].isFlying = false;
                            bullet[i].texture = bulletTexture2;
                            updateBrickOccupied(column, row + 1);
                        }
                    }
                    else
                    {
                        bullet[i].isFlying = false;
                        bullet[i].texture = bulletTexture2;
                    }

                }
                if (bullet[i].direction == 3)
                {
                    if (bullet[i].horizontalPos > leftMargin + gridCellSize)
                    {
                        if (!gridCell[column - 1, row].occupied || gridCell[column - 1, row].occupiedBy == "W")
                        {
                            bullet[i].horizontalPos -= gridCellSize / 5;
                        }
                        else
                        {
                            bullet[i].isFlying = false;
                            bullet[i].texture = bulletTexture2;
                            updateBrickOccupied(column - 1, row);
                        }
                    }
                    else
                    {
                        bullet[i].isFlying = false;
                        bullet[i].texture = bulletTexture2;
                    }
                }
                if (bullet[i].direction == 1)
                {
                    //Console.WriteLine("inside direction1:row:" + row + " column:" + column + " occupied:" + gridCell[column, row].occupied);
                    if (bullet[i].horizontalPos < leftMargin + gridCellSize * 9)
                    {
                        if (!gridCell[column + 1, row].occupied || gridCell[column + 1, row].occupiedBy == "W")
                        {
                            bullet[i].horizontalPos += gridCellSize / 5;
                        }
                        else
                        {
                            bullet[i].isFlying = false;
                            bullet[i].texture = bulletTexture2;
                            updateBrickOccupied(column + 1, row);
                        }
                    }
                    else
                    {
                        bullet[i].isFlying = false;
                        bullet[i].texture = bulletTexture2;
                    }
                }



            }
        }
        /* private void setUpTanks()
         {
            
             Player player;
             for (int i = 0; i < 10; i++)
             {
                 for (int j = 0; j < 10; j++)
                 {

                     if (game.board[i, j] == "0")
                     {
                         player = game.player[0];
                         tank[0].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                         tank[0].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                         tank[0].direction = player.direction;
                         gridCell[i, j].occupied = true;
                         tank[0].angle = MathHelper.ToRadians((player.direction + 1) * 90);
                         tank[0].isEmpty=false;

                     }
                     if (game.board[i, j] == "1") {
                         player = game.player[1];
                         tank[1].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                         tank[1].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                         tank[1].direction = player.direction;
                         gridCell[i, j].occupied = true;
                         tank[1].angle = MathHelper.ToRadians((player.direction + 1) * 90);
                         tank[1].isEmpty=false;
                     }
                     if (game.board[i, j] == "2") {
                         player = game.player[2];
                         tank[2].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                         tank[2].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                         tank[2].direction = player.direction;
                         gridCell[i, j].occupied = true;
                        
                         tank[2].angle = MathHelper.ToRadians((player.direction + 1) * 90);
                         tank[2].isEmpty=false;
                     }
                     if (game.board[i, j] == "3") {
                         player = game.player[3];
                         tank[3].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                         tank[3].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                         tank[3].direction = player.direction;
                         gridCell[i, j].occupied = true;
                         tank[3].angle = MathHelper.ToRadians((player.direction + 1) * 90);
                         tank[3].isEmpty=false;
                     }
                     if (game.board[i, j] == "4") {
                         player = game.player[0];
                         tank[4].horizontalPosition = player.playerLocationX * gridCellSize + leftMargin;
                         tank[4].verticalPosition = player.playerLocationY * gridCellSize + topMargin;
                         tank[4].direction = player.direction;
                         gridCell[i, j].occupied = true;
                         tank[4].angle = MathHelper.ToRadians((player.direction+1)*90);
                         tank[4].isEmpty=false;
                     }

                 }
             }
         }*/
        private void drawTank()
        {
            tanks = new Texture2D[] { redTankTexture, purpleTankTexture, greenTankTexture, blueTankTexture, brownTankTexture };
            for (int k = 0; k < 5; k++)
            {
                //if (!tank[k].isEmpty && tank[k].isPlaying)
                if (tank[k].isPlaying)
                {
                    // Console.WriteLine("value of k outside the if:" + k+" health:"+tank[k].health);
                    if (tank[k].health > 0)
                    {
                        // Console.WriteLine("k is:" + k+" /////////////////////////////////////////////////////////////////////////////////");
                        spriteBatch.Draw(tanks[k], new Vector2(tank[k].horizontalPosition + gridCellSize / 2, tank[k].verticalPosition + gridCellSize / 2), null, tank[k].tankColor, tank[k].angle, new Vector2(gridCellSize / 2, gridCellSize / 2), 1, SpriteEffects.None, 1);
                    }
                    else
                    {
                        //tank[k].isEmpty = true;
                        //tank[k].isPlaying = false;
                    }
                }
                else if (!tank[k].isPlaying)
                { //Console.WriteLine("tank is not playing:" + k);
                  // Console.WriteLine("inside else////////////////////////////////////////////////////////////////////////////////");
                  // spriteBatch.Draw(gridTexture, new Vector2(tank[k].horizontalPosition + gridCellSize / 2, tank[k].verticalPosition + gridCellSize / 2), null, tank[k].tankColor, tank[k].angle, new Vector2(gridCellSize / 2, gridCellSize / 2), 1, SpriteEffects.None, 1);
                }
            }
        }

        public void updateTank()
        {
            for (int i = 0; i < 5; i++)
            {
                if (!tank[i].isEmpty)
                {
                    ////Console.WriteLine("inside tank update:health:"+game.player[i].health);
                    ////Console.WriteLine("inside tank update:shoot:" + game.player[i].whetherShot);
                    tank[i].health = game.player[i].health;
                    tank[i].points = game.player[i].points;
                    tank[i].coins = game.player[i].coins;

                    if (game.player[i].whetherShot == 1 & game.player[i].timeToShot)
                    {
                        game.player[i].timeToShot = false;
                        if (bulletIndex < 20)
                        {
                            //Console.WriteLine("going to fly");
                            bullet[bulletIndex].direction = tank[i].direction;
                            bullet[bulletIndex].horizontalPos = tank[i].horizontalPosition;
                            bullet[bulletIndex].verticalPos = tank[i].verticalPosition;
                            bullet[bulletIndex].isFlying = true;
                            bullet[bulletIndex].texture = bulletTexture1;
                            bullet[bulletIndex].bulletColor = tank[i].tankColor;
                            bulletIndex++;
                        }
                        else
                        {
                            bulletIndex = 0;
                            bullet[bulletIndex].direction = tank[i].direction;
                            bullet[bulletIndex].horizontalPos = tank[i].horizontalPosition;
                            bullet[bulletIndex].verticalPos = tank[i].verticalPosition;
                            bullet[bulletIndex].isFlying = true;
                            bullet[bulletIndex].texture = bulletTexture1;
                            bulletIndex++;
                        }
                    }
                }
            }

        }

        private void drawBullet(int i)
        {
            if (bullet[i].isFlying)
            {
                //Console.WriteLine("inside drawbullet");

                spriteBatch.Draw(bullet[i].texture, new Vector2(bullet[i].horizontalPos + gridCellSize / 2, bullet[i].verticalPos + gridCellSize / 2), null, bullet[i].bulletColor, MathHelper.ToRadians(bullet[i].direction * 90), new Vector2(gridCellSize / 2, gridCellSize / 2), 1, SpriteEffects.None, 1);

            }
        }
        private void ProcessKeyboard()
        {
            KeyboardState keybState = Keyboard.GetState();

            //Console.WriteLine("Time to press");
            if (keybState.IsKeyDown(Keys.Right))
            {
                //Console.WriteLine("right button pressed");
                myConnection.sendData("RIGHT#");
            }
            if (keybState.IsKeyDown(Keys.Left))
            {
                myConnection.sendData("LEFT#");
            }
            if (keybState.IsKeyDown(Keys.Up))
            {
                myConnection.sendData("UP#");
            }
            if (keybState.IsKeyDown(Keys.Down))
            {
                myConnection.sendData("DOWN#");
            }
            if (keybState.IsKeyDown(Keys.Space))
            {
                myConnection.sendData("SHOOT#");
            }

        }
        public void updateBrickOccupied(int column, int row)
        {
            for (int i = 0; i < 20; i++)
            {
                if (bricks[i].isFull)
                {
                    if (bricks[i].horizontalLocation == column * gridCellSize + leftMargin && bricks[i].verticalLocation == row * gridCellSize + topMargin)
                    {
                        if (bricks[i].status == 4)
                        {
                            //Console.WriteLine("inside updateBrickStatus:row:" + row + " column" + column);
                            gridCell[column, row].occupied = false;
                            bricks[i].isFull = false;
                        }
                    }
                }
            }
        }
        public void updateBrickAndTanks()
        {


            for (int i = 0; i < 20; i++)
            {
                if (game.bricks[i].isFull)
                {
                    //bricks[i].status = game.bricks[i].damageLevel;

                    for (int j = 0; j < 20; j++)
                    {

                        if (bricks[j].horizontalLocation == game.bricks[i].locationX * gridCellSize + leftMargin && bricks[j].verticalLocation == game.bricks[i].locationY * gridCellSize + topMargin)
                        {
                            bricks[j].status = game.bricks[i].damageLevel;

                            if (bricks[j].status == 1)
                            {
                                bricks[j].brickTexture = brickTexture2;
                            }
                            if (bricks[j].status == 2)
                            {
                                bricks[j].brickTexture = brickTexture3;
                            }
                            if (bricks[j].status == 3)
                            {
                                bricks[j].brickTexture = brickTexture4;
                            }
                            if (bricks[j].status == 4)
                            {
                                //Console.WriteLine("inside status 4 row:" + game.bricks[i].locationY + "column:" + game.bricks[i].locationX);
                                bricks[j].brickTexture = brickTexture5;


                                game.bricks[i].isFull = false;
                            }
                        }
                    }
                }
            }
            /* if (gridCell[column, row].occupiedBy == "B")
             {
                 for (int i = 0; i < 20; i++)
                 {
                     if (bricks[i].isFull)
                     {
                         if (bricks[i].horizontalLocation == column * gridCellSize + leftMargin && bricks[i].verticalLocation == row * gridCellSize + topMargin)
                         {
                             bricks[i].status++;
                             if (bricks[i].status == 1)
                             {
                                 bricks[i].brickTexture = brickTexture2;
                             }
                             if (bricks[i].status == 2)
                             {
                                 bricks[i].brickTexture = brickTexture3;
                             }
                             if (bricks[i].status == 3)
                             {
                                 bricks[i].brickTexture = brickTexture4;
                             }
                             if (bricks[i].status == 4)
                             {
                                 //Console.WriteLine("inside status 4 row:" + row + "column:" + column);
                                 bricks[i].brickTexture = brickTexture5;
                                 gridCell[column, row].occupied = false;
                                 bricks[i].isFull = false;
                             }
                         }
                     }
                 }
             }
         if (gridCell[column, row].occupiedBy == "0")
         {
         }
         if (gridCell[column, row].occupiedBy == "1") { }
         if (gridCell[column, row].occupiedBy == "2") { }
         if (gridCell[column, row].occupiedBy == "3") { }
         if (gridCell[column, row].occupiedBy == "4") { }*/
        }


    }
}

