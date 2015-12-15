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

        int gridCellSize = 45;
        Vector2 position = Vector2.Zero;
        Texture2D gridTexture;
        Texture2D tankTexture;
        Vector2 rocketDirection;
        public gridCellData[,] gridCell=new gridCellData[10,10];
        public tankData[] tank = new tankData[5];
        public struct gridCellData{
            public int verticalPosition;
            public int horizontalPosition;
        }

        public struct tankData
        {
            public int verticalPosition;
            public int horizontalPosition;
            public float angle;
            public String direction;
            
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
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Riemer's 2D XNA Tutorial";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            Console.WriteLine(MathHelper.ToRadians(90));

            backgroundTexture = Content.Load<Texture2D>("background");
            foregroundTexture = Content.Load<Texture2D>("foreground");
            gridTexture = Content.Load<Texture2D>("cell");
            tankTexture = Content.Load<Texture2D>("tank1");
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
            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //DrawScenery();
            drawGrid();
            drawTank();
            
            //spriteBatch.Draw(backgroundTexture, new Rectangle(24, 28, 45, 45), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(20, 20, 45, 45);
            Rectangle screenRectangle1 = new Rectangle(70, 20, 45, 45);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.Yellow);
            spriteBatch.Draw(foregroundTexture, screenRectangle, Color.White);
            spriteBatch.Draw(backgroundTexture, screenRectangle1, Color.Yellow);
            spriteBatch.Draw(foregroundTexture, screenRectangle1, Color.White);
        }
        private void drawGrid() {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {

                    spriteBatch.Draw(gridTexture, new Rectangle(gridCell[i, j].horizontalPosition, gridCell[i, j].verticalPosition, 45, 45), Color.White);
                    if(i==0 && j==4){
                        spriteBatch.Draw(tankTexture, new Rectangle(gridCell[i, j].horizontalPosition, gridCell[i, j].verticalPosition, 45, 45), Color.White);
                    }
                    
                    
                }
            }
        
        }
        private void setUpGrid() {
            Console.WriteLine("innn");
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    gridCell[i, j].horizontalPosition = 20 + j * 45;
                    gridCell[i, j].verticalPosition = 20 + i * 45;
                    if (j == 9) {
                        Console.Write(j);
                        Console.Write("\n"); }
                    else { Console.Write(j); }
                }
                    
                }
            }
        private void setUpTanks() {
            for (int i = 0; i < 5; i++) {
                Console.WriteLine(i + i);
                tank[i].horizontalPosition = 20 + i * 45;
                tank[i].verticalPosition = 20 + i * 45;
                tank[i].angle = 0;
            }
        }
        private void drawTank() {
            for (int k = 0; k < 5; k++)
            {
                //Console.WriteLine(1+1);
                spriteBatch.Draw(tankTexture, new Vector2(tank[k].horizontalPosition+45/2, tank[k].verticalPosition+45/2), null,Color.White,tank[k].angle,new Vector2(45/2,45/2),1,SpriteEffects.None,1);
                //spriteBatch.Draw(tankTexture, new Vector2(tank[k].horizontalPosition, tank[k].verticalPosition), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            }
        }
        private void ProcessKeyboard()
        {
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Up)) {
                tank[1].angle = MathHelper.ToRadians(90);
                Console.WriteLine("up pressed");
                //rocketAngle = players[currentPlayer].Angle;
                tank[1].verticalPosition += 20;
                
                Vector2 up = new Vector2(0, -1);
                Matrix rotMatrix = Matrix.CreateRotationZ(tank[1].angle);
                rocketDirection = Vector2.Transform(up, rotMatrix);
            }
                
            
        }

        
        }
    }

