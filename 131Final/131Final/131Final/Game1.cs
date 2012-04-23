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

namespace VeryRealTournament
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int wavesSpawned;
        Player[] player = new Player[4];

        SpriteFont defaultFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            Window.Title = "Creep Conundrum : Strife";
            base.Initialize();
            for (int i = 0; i < 4; i++)
            {
                player[i] = new Player(spriteBatch, PlayerIndex.One, i+1, defaultFont);
            }
        }
        protected override void LoadContent()
        {
            this.IsMouseVisible = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultFont = Content.Load<SpriteFont>("DefaultFont");
        }
        protected override void UnloadContent()
        {
            
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            /*Fullscreenness of awesome!*/
            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
                GridManager.InitLineDrawer(spriteBatch.GraphicsDevice);
            }
            /*teh space case is me ~ Zack*/
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {

                /*How to spawn a creep wave*/
                CreepData tempD = new CreepData();
                tempD.Speed = new Random().NextDouble() * 1.5 + 0.5;
                tempD.Damage = 1;
                tempD.Value = 1000;
                for (int i = 0; i < 4; i++)
                    player[i].addCreepWave(tempD, gameTime.TotalGameTime.TotalMilliseconds + 1000, 4, spriteBatch);
            //    //addCreepWave(CreepData, When the Wave Starts, Minions to this wave, SpriteBatch);
            //    for (int i = 0; i < 4; i++)
            //        temp[i].addCreepWave(tempD, gameTime.TotalGameTime.TotalMilliseconds + 1000, 4, spriteBatch);
            //    wavesSpawned++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            for (int i = 0; i < 4; i++)
                player[i].Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            SplitScreenAdapter.drawLines(spriteBatch);
            for (int i = 0; i < 4; i++)
                player[i].Draw(graphics, gameTime);

            Vector2 tempV = defaultFont.MeasureString(wavesSpawned.ToString());
            tempV.Y = 0;
            spriteBatch.DrawString(defaultFont,wavesSpawned.ToString(),new Vector2(300,300) - tempV,Color.Black);

            spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}
