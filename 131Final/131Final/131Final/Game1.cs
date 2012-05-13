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
using Engine;
using System.Threading;

namespace VeryRealTournament
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        OverLord theOverLord = new OverLord();
        SpriteFont[] mainMenuFonts = new SpriteFont[2];
        string[] diplayStrings = new string[2]{"Creep Conundrum: Strife", "Press Enter to Play"};
        byte gameState = 0; //0 = main menu, 1 = loading, 2 = playing, 3 = game over
        Thread loadingThread;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            Window.Title = "No";
            
            base.Initialize();
            SoundManager.PlayLoop(Musics.One);
        }
        protected override void LoadContent()
        {
            //this.IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 1280;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainMenuFonts[0] = Content.Load<SpriteFont>(@"Fonts\MainMenuFont1");
            mainMenuFonts[1] = Content.Load<SpriteFont>(@"Fonts\MainMenuFont2");
            
            
            SoundManager.InitMain(Content);
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            //if (Keyboard.GetState().IsKeyDown(Keys.F11))
            //{
            //    graphics.PreferredBackBufferHeight = 1280;
            //    graphics.PreferredBackBufferWidth = 1024;
            //    graphics.IsFullScreen = !graphics.IsFullScreen;
            //    graphics.ApplyChanges();
            //    //GridManager.InitLineDrawer(spriteBatch.GraphicsDevice);
            //}

            if (gameState == 2)
                theOverLord.Update(graphics, spriteBatch, gameTime, GraphicsDevice);
            else if ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter)) && gameState == 0)
            {
                gameState = 1;
                SoundManager.PlayLoop(Musics.Two);
                loadingThread = new Thread(loadMethod);
                loadingThread.Name = "Loading Game Thread";
                loadingThread.IsBackground = true;
                loadingThread.Start();
            }
            else if (gameState == 1)
            {
                diplayStrings[0] = theOverLord.loadDisplay;
                diplayStrings[1] = "Dancing Monkeys";
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            if (gameState == 2)
                theOverLord.Draw(gameTime, spriteBatch, graphics);
            else
            {
                Vector2 v = mainMenuFonts[0].MeasureString(diplayStrings[0]);
                spriteBatch.DrawString(mainMenuFonts[0], diplayStrings[0], new Vector2(graphics.PreferredBackBufferWidth / 2 - v.X / 2, graphics.PreferredBackBufferHeight / 2 - v.Y / 2), Color.Black);
                v.X = graphics.PreferredBackBufferWidth / 2 - mainMenuFonts[1].MeasureString(diplayStrings[1]).X / 2;
                v.Y = v.Y + graphics.PreferredBackBufferHeight / 2 - v.Y / 2;
                spriteBatch.DrawString(mainMenuFonts[1], diplayStrings[1], v, Color.Black);
                //Do Shit.
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// This loads all the content.
        /// Called through in a thread.
        /// </summary>
        public void loadMethod()
        {
            theOverLord.Init(spriteBatch);
            theOverLord.LoadContent(Content);
            gameState = 2;
        }
    }
}
