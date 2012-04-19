/*
 * 4/19/2012 Player Class started
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine
{
    public struct PlyaerData
    {

    }
    public struct Cursor
    {
        public int x;
        public int y;
        public Texture2D texture;
    }

    /// <summary>
    /// hold cursor + draw
    /// update
    /// handle current money
    /// controller interface
    /// </summary>
    public class Player
    {
        SpriteBatch spriteBtach;
        PlayerMap playerMap;
        public Cursor cursor;
        int screen;

        

        public Player(SpriteBatch SB/*, OverLord OV*/, int SN)
        {
            spriteBtach = SB;
            screen = SN;
            playerMap = new PlayerMap();
            cursor = new Cursor();
            cursor.x = 0;
            cursor.y = 0;
        }

        public void Update(GameTime gameTime)
        {

            playerMap.Update(gameTime);
        }

        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            spriteBtach.Draw(cursor.texture, new Vector2(cursor.x, cursor.y), Color.White);
            playerMap.Draw(screen, spriteBtach, graphics, true, gameTime);
        }
    }
}
