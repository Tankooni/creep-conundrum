using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine
{
    class Menu
    {
        private int myScreen;
        private static List<Menu> instances = new List<Menu>();
        private Player myPlayer;
        private string myData;
        private static Texture2D myTexture;
        static SpriteBatch spriteBtach;
        public Vector2 centerVec = Vector2.Zero;
        public float cursorAngle = 0;

        public int Screen
        {
            get { return myScreen; }
        }
        public string Data
        {
            get { return myData; }
        }

        public Menu() {}
        /// <summary>
        /// Every time a menu is created, it checks to see if a menu of that type already exists. If so, it will delete both.
        /// 
        /// </summary>
        /// <param name="Screen">Tells it which screen it is going to draw on.</param>
        /// <param name="Data">Tower, Minion, Menu, TowerGroup, MinionGroup</param>
        /// <param name="PD">The set of data of the player that created menu. Allows the menu to interact with player.</param>
        /// <returns>It will return early if it finds an occurrence of this menu already in the list of menus.</returns>
        public Menu(int Screen, string Data, Player PD)
        {
            myPlayer = PD;

            myData = Data;
            
            for (int i = instances.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(Data + " " + instances[i].Data);
                if(Data == instances[i].Data)
                {
                    myPlayer.menuOpen = false;
                    instances.Remove(instances[i]);
                    return;
                }
            }
            instances.Add(this);
            myPlayer.menuOpen = true;
            centerVec = new Vector2(spriteBtach.GraphicsDevice.Viewport.Width / 2 - myTexture.Width, spriteBtach.GraphicsDevice.Viewport.Height / 2 - myTexture.Height);
        }

        public static void Init(SpriteBatch SB)
        {
            spriteBtach = SB;
            int outerRadius = (int)(SB.GraphicsDevice.Viewport.Height / 2) / 2;
            myTexture = new Texture2D(SB.GraphicsDevice, outerRadius, outerRadius, false, SurfaceFormat.Color);
            int center = outerRadius / 2;
            Color[] data = new Color[outerRadius * outerRadius];

            for (int x = 0; x < outerRadius; x++)
            {
                for (int y = 0; y < outerRadius; y++)
                {
                    //Console.WriteLine(Math.Sqrt(Math.Pow((center - x), 2) + Math.Pow((center - y), 2)) + ", " + outerRadius);
                    if(Math.Sqrt(Math.Pow((center - x), 2) +  Math.Pow((center - y), 2)) < outerRadius / 2)
                        data[x * outerRadius + y] = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
            }

            myTexture.SetData(data);
        } // End of Init

        public Menu closeMenu()
        {
            instances.Remove(this);
            return new Menu();
        }

        public static void Update(GameTime gameTime)
        {
            for (int i = instances.Count - 1; i >= 0; i--)
            {
                instances[i].cursorAngle = (float)Math.Atan2(instances[i].myPlayer.Input.Current.y1, instances[i].myPlayer.Input.Current.x1);
            }
        }
        /// <summary>
        /// WTF MATH
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Draw(GameTime gameTime)
        {
            //Console.WriteLine(instances.IndexOf(this));
            for (int i = instances.Count - 1; i >= 0; i--)
            {
                spriteBtach.Draw(myTexture, SplitScreenAdapter.splitConvert(instances[i].myPlayer.Screen, instances[i].centerVec, spriteBtach), Color.FromNonPremultiplied(100,100,100,200));
                GridManager.DrawLine(spriteBtach, 1, Color.Red, 
                    (SplitScreenAdapter.splitConvert(instances[i].myPlayer.Screen, instances[i].centerVec, spriteBtach) + new Vector2(myTexture.Width/2, myTexture.Height/2)), 
                    (SplitScreenAdapter.splitConvert(instances[i].myPlayer.Screen, new Vector2((float)(myTexture.Width * Math.Cos(instances[i].cursorAngle)), (float)(myTexture.Width * Math.Sin(instances[i].cursorAngle))), spriteBtach))
                    );
            }

        }
    }
}
