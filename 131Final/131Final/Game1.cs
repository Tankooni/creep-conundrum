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

namespace _131Final
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PlayerMap temp;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            Window.Title = "Real Tournament";
            base.Initialize();
            temp = new PlayerMap();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
                GridManager.InitLineDrawer(spriteBatch.GraphicsDevice, 15);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                CreepData tempD = new CreepData();
                tempD.Speed = 1.0;
                temp.addCreepWave(tempD, gameTime.TotalGameTime.TotalMilliseconds + 3000, 7, spriteBatch);
            }
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
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
