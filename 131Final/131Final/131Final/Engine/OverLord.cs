using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Engine;


/* Class added on 4/23/2012 by Michael Elser */

namespace Engine
{
    public class OverLord
    {
        public void LoadContent()
        {
            //Load neccessary game content for session here
        }

        public void UPDATE(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Player[] player, GraphicsDevice gDevice)
        {
            //This will be handled by a controller class later on
            /*Fullscreenness of awesome!*/
            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                graphics.PreferredBackBufferHeight = gDevice.DisplayMode.Height;
                graphics.PreferredBackBufferWidth = gDevice.DisplayMode.Width;
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
            for (int i = 0; i < 4; i++)
                player[i].Update(gameTime);

        }

        public void DRAW(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Player[] player)
        {
            SplitScreenAdapter.drawLines(spriteBatch);
            for (int i = 0; i < 4; i++)
                player[i].Draw(graphics, gameTime);

            //Vector2 tempV = defaultFont.MeasureString(wavesSpawned.ToString());
            //tempV.Y = 0;
        }

        public void ENDGAME()
        {
            //Declare winner and unload content, blah blah
        }

        public void STARTGAME()
        {
            //Begin the game (start spawning creep, let players start doing stuff)
        }
    }
}
