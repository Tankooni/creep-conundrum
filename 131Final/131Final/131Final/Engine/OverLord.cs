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
        Player[] player = new Player[4];
        SpriteFont defaultFont;
        double nextSpawnTime = 10;
        bool first = true;

        public void Init(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 4; i++)
                player[i] = new Player(spriteBatch, this, i + 1, i + 1, defaultFont);
        }

        public void LoadContent(ContentManager Content)
        {
            defaultFont = Content.Load<SpriteFont>("defaultFont");
            SoundManager.Init(Content);
            PlayerData pData1 = new PlayerData();
            pData1.raceName = "WALEK";
            pData1.startHealth = 100;
            pData1.startMoney = 100;
            pData1.towerList = DataParser.getGroupTowerData(pData1.raceName, "students");

            PlayerData pData2 = new PlayerData();
            pData2.raceName = "MOOGLES";
            pData2.startHealth = 200;
            pData1.startMoney = 50;
            pData2.towerList = DataParser.getGroupTowerData(pData2.raceName, "Nins");
            for (int x = 1; x <= 2; x++)
            {
                player[x].Load(pData1);
            }
            for (int x = 2; x <= 4; x++)
            {
                player[x].Load(pData2);
            }

            //Load neccessary game content for session here
        }

        public void Update(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, GraphicsDevice gDevice)
        {
            SoundManager.gameTime = gameTime;
            if (first)
            {
                SoundManager.PlaySetList(new Musics[] { Musics.CATGROOVE, Musics.Corncob, Musics.FasterDoesIt, Musics.GrooveGrove });
                first = false;
            }
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
            /*Imma take yer space case and use it for interval spawnzzz ~Elser */
            
            //if (Keyboard.GetState().IsKeyDown(Keys.Space))
            //{

            //    /*How to spawn a creep wave*/
            //    CreepData tempD = new CreepData();
            //    tempD.Speed = new Random().NextDouble() * 1.5 + 0.5;
            //    tempD.Damage = 1;
            //    tempD.Value = 1000;
            //    for (int i = 0; i < 4; i++)
            //        player[i].addCreepWave(tempD, gameTime.TotalGameTime.TotalMilliseconds + 1000, 4, spriteBatch);
            //    //    //addCreepWave(CreepData, When the Wave Starts, Minions to this wave, SpriteBatch);
            //    //    for (int i = 0; i < 4; i++)
            //    //        temp[i].addCreepWave(tempD, gameTime.TotalGameTime.TotalMilliseconds + 1000, 4, spriteBatch);
            //    //    wavesSpawned++;
            //}
            if ((nextSpawnTime <= gameTime.TotalGameTime.TotalSeconds))
            {
                CreepData data = new CreepData();
                nextSpawnTime = gameTime.TotalGameTime.TotalSeconds + 10;
                data = getRandomCreepData(data);
                spawnCreepWave(data, gameTime, spriteBatch);
            }

            for (int i = 0; i < 4; i++)
                player[i].Update(gameTime);
        }

        //Were we going to have a creepOverlord too? .... I forgot ~Elser */
        public void spawnCreepWave(CreepData data, GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 4; i++)
            {
                player[i].addCreepWave(data, gameTime.TotalGameTime.TotalMilliseconds + 1000, new Random().Next(1, 10), spriteBatch);
            }
        }

        public CreepData getRandomCreepData(CreepData data)
        {
            data.Speed = new Random().NextDouble() * 1.5 + 0.5;
            data.Damage = 1;
            data.Value = (SystemVars.totalWaves/10) + 1;
            data.Health = (SystemVars.totalWaves/1) + 10;
            SystemVars.totalWaves++;
            Console.WriteLine("CurrentWave: " + SystemVars.totalWaves);
            return data;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            SplitScreenAdapter.drawLines(spriteBatch);
            for (int i = 0; i < 4; i++)
                player[i].Draw(graphics, gameTime);

            //Vector2 tempV = defaultFont.MeasureString(wavesSpawned.ToString());
            //tempV.Y = 0;
        }

        public void EndGame()
        {
            //Declare winner and unload content, blah blah
        }

        public void StartGame()
        {
            //Begin the game (start spawning creep, let players start doing stuff)
        }
    }
}