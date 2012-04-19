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
        PlayerMap temp;
        int wavesSpawned;

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
            temp = new PlayerMap();
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
                //graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                //graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
                GridManager.InitLineDrawer(spriteBatch.GraphicsDevice, 15);
            }
            /*teh space case is me ~ Zack*/
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {

                /*How to spawn a creep wave*/
                CreepData tempD = new CreepData();
                tempD.Speed = new Random().NextDouble()*1.5+0.5;
                //addCreepWave(CreepData, When the Wave Starts, Minions to this wave, SpriteBatch);
                temp.addCreepWave(tempD, gameTime.TotalGameTime.TotalMilliseconds + 1000, 15, spriteBatch);
                wavesSpawned++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                /*How to spawn a tower*/
                TowerData tempT = new TowerData();
                tempT.Damage = 1;
                tempT.Range = 100;
                tempT.RateOfFire = 100;
                for(int x = 0; x< 15; x++)
                    for(int y = 0; y < 15; y++)
                        //addTower(TowerData, Build Time(Not implemented yet), Xcoord, yCoord, SpriteBatch);
                        temp.addTower(tempT, 0.0, x, y, spriteBatch);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            temp.Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            SplitScreenAdapter.drawLines(spriteBatch);

            temp.Draw(1, spriteBatch, graphics, true, gameTime);
            temp.Draw(2, spriteBatch, graphics, false, gameTime);
            temp.Draw(3, spriteBatch, graphics, true, gameTime);
            temp.Draw(4, spriteBatch, graphics, false, gameTime);
            
            Vector2 tempV = defaultFont.MeasureString(wavesSpawned.ToString());
            tempV.Y = 0;
            spriteBatch.DrawString(defaultFont,wavesSpawned.ToString(),new Vector2(300,300) - tempV,Color.Black);

            spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}
