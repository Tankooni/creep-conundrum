﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine
{
    enum MenuType
    {
        Tower,
        Minion,
        Player,
        TowerGroup,
        MinionGroup
    }

    /// <summary>
    /// Holds a string that represents the choice, and two angles in radians that represent where the choice is.
    /// </summary>
    struct Choice
    {
        public string choice;
        public float angle1, angle2;

        public Choice(string Choice, float Angle1, float Angle2)
        {
            this.choice = Choice;
            this.angle1 = Angle1;
            this.angle2 = Angle2;
        }

        public override string ToString() { return "Choice: " + choice + ", Angle1: " + angle1 + ", Angle2: " + angle2; }
    }

    class Menu
    {
        private static List<Menu> instances = new List<Menu>();
        private Player myPlayer;
        private MenuType myData;
        private static Texture2D myTexture;
        static SpriteBatch spriteBtach;
        public Vector2 centerVec = Vector2.Zero;
        public float cursorAngle = 0;
        List<Choice> choices = new List<Choice>();

        public MenuType Data
        {
            get { return myData; }
        }

        public Menu() {}
        /// <summary>
        /// Every time a menu is created, it checks to see if a menu of that type already exists. If so, it will delete both.
        /// 
        /// </summary>
        /// <param name="Screen">Tells it which screen it is going to draw on.</param>
        /// <param name="Data">Tower, Minion, Players, TowerGroup, MinionGroup</param>
        /// <param name="PD">The set of data of the player that created menu. Allows the menu to interact with player.</param>
        /// <returns>It will return early if it finds an occurrence of this menu already in the list of menus.</returns>
        public Menu(int Screen, MenuType Data, Player PD)
        {
            myPlayer = PD;

            myData = Data;
            
            for (int i = instances.Count - 1; i >= 0; i--)
            {
                if(Data == instances[i].Data && myPlayer.Screen == instances[i].myPlayer.Screen)
                {
                    myPlayer.menuOpen = false;
                    instances.Remove(instances[i]);
                    return;
                }
            }
            instances.Add(this);
            myPlayer.menuOpen = true;
            centerVec = new Vector2(spriteBtach.GraphicsDevice.Viewport.Width / 2 - myTexture.Width, spriteBtach.GraphicsDevice.Viewport.Height / 2 - myTexture.Height);

            switch(myData)
            {
                case MenuType.Minion:
                    break;
                case MenuType.MinionGroup:
                    break;
                case MenuType.Player:
                    for (int i = 0; i < 4; i++)
                    {
                        if (i != (Screen-1))
                            choices.Add(new Choice(""+i ,0 , 0));
                    }
                    break;
                case MenuType.Tower:
                    break;
                case MenuType.TowerGroup:
                    break;
            }
            float tempAng = (float)MathHelper.TwoPi / choices.Count;

            for (int i = 0; i < choices.Count; i++)
            {
                Choice temp = choices[i];
                temp.angle1 = tempAng*i;
                temp.angle2 = tempAng * (i+1);
                choices[i] = temp;
            }
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

        /// <summary>
        /// Call this to close the menu;
        /// </summary>
        /// <param name="isCancel">This determines whether or not the menu does something when it is closed.</param>
        public void closeMenu(bool isCancel)
        {
            if (!isCancel)
            {
                switch (myData)
                {
                    case MenuType.Minion:
                        break;
                    case MenuType.MinionGroup:
                        break;
                    case MenuType.Player:

                        break;
                    case MenuType.Tower:
                        break;
                    case MenuType.TowerGroup:
                        break;
                }
            }
            myPlayer.menuOpen = false;
            instances.Remove(this);
        }

        public static void Update(GameTime gameTime)
        {
            for (int i = instances.Count - 1; i >= 0; i--)
            {
                instances[i].cursorAngle = (float)Math.Atan2(instances[i].myPlayer.Input.Current.y1, instances[i].myPlayer.Input.Current.x1);
                if (instances[i].cursorAngle < 0) instances[i].cursorAngle = (float)MathHelper.TwoPi + instances[i].cursorAngle;

                if (instances[i].myPlayer.Input.Current.B)
                {
                    instances[i].closeMenu(true);
                    continue;
                }

                if (instances[i].myPlayer.Input.Current.A)
                {
                    for(int j = instances[i].choices.Count-1; j >= 0; j--)
                    {
                        if (instances[i].cursorAngle > instances[i].choices[j].angle1 && instances[i].cursorAngle < instances[i].choices[j].angle2)
                        {
                            if ((instances[i].myPlayer.currentMoney -= instances[i].myPlayer.currentMinion.Value) >= 0)
                            {
                                instances[i].myPlayer.myOverlord.spawnMinionWave(instances[i].myPlayer.currentMinion, int.Parse(instances[i].choices[j].choice), gameTime, spriteBtach);
                                
                            }
                            else
                            {
                                instances[i].myPlayer.currentMoney += instances[i].myPlayer.currentMinion.Value;
                            }
                            instances[i].closeMenu(true);
                            break;
                            //Console.WriteLine(MathHelper.ToDegrees(instances[i].choices[j].angle1) + " " + MathHelper.ToDegrees(instances[i].choices[j].angle2));
                        }
                        //Console.WriteLine(instances[i].choices[j].choice + " " + MathHelper.ToDegrees(instances[i].cursorAngle));
                        //Console.WriteLine(MathHelper.ToDegrees(instances[i].choices[j].angle1) + " " + MathHelper.ToDegrees(instances[i].choices[j].angle2));
                    }
                }

                //if (instances[i].myPlayer.Input.Current.B)
                //{
                //    instances[i].closeMenu(true);
                //}
                //Console.WriteLine(Math.Cos(instances[i].cursorAngle) + " " + Math.Sin(instances[i].cursorAngle));
            }
        }

        /// <summary>
        /// This draws the circle that represents the menu, the line selector, and the various options.
        /// </summary>
        /// <param name="gameTime">Does this need explanation?</param>
        public static void Draw(GameTime gameTime)
        {
            for (int i = instances.Count - 1; i >= 0; i--)
            {
                spriteBtach.Draw(myTexture, SplitScreenAdapter.splitConvert(instances[i].myPlayer.Screen, instances[i].centerVec, spriteBtach), Color.FromNonPremultiplied(100,100,100,200));
                Vector2 temp = SplitScreenAdapter.splitConvert(instances[i].myPlayer.Screen, instances[i].centerVec, spriteBtach) + new Vector2(myTexture.Width / 2, myTexture.Height / 2);
                if (instances[i].myPlayer.Input.Current.x1 != 0 || instances[i].myPlayer.Input.Current.y1 != 0)
                    GridManager.DrawLine(spriteBtach, 1, Color.Red, temp, temp + new Vector2((float)Math.Cos(instances[i].cursorAngle) * myTexture.Width/2, -(float)Math.Sin(instances[i].cursorAngle) * myTexture.Height/2));
                for (int j = 0; j < instances[i].choices.Count; j++)
                {
                    GridManager.DrawLine(spriteBtach, 1, Color.Purple, temp, temp + new Vector2((float)Math.Cos(instances[i].choices[j].angle1) * myTexture.Width / 2, -(float)Math.Sin(instances[i].choices[j].angle1) * myTexture.Height / 2));
                    spriteBtach.DrawString(instances[i].myPlayer.playerData.spriteFont, "P" + (int.Parse(instances[i].choices[j].choice)+1), temp + new Vector2((float)Math.Cos((instances[i].choices[j].angle2 - MathHelper.PiOver4)) * myTexture.Width / 4, -(float)Math.Sin((instances[i].choices[j].angle2 - MathHelper.PiOver4)) * myTexture.Height / 4), Color.PowderBlue);
                }
            }

        }
    }
}
