/*
 * 4/19/2012 Player Class started
 * 4/19/2012 Basic functionality of moving cursor, placing towers, and taking in input data
 *              Plans are to do key combos for menus, adding more variables for money and health, also drawing this data
 *              More words
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Engine
{
    public struct PlayerData
    {
        public string raceName;
        public double startHealth;
        public int startMoney;
        public List<CreepData> creepList;
        public List<TowerData> towerList;
        public PlayerMap playerMap;
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
        PlayerData playerData;
        SpriteBatch spriteBtach;
        PlayerMap playerMap;
        public Cursor cursor;
        int screen;
        Inputs input;
        double currentHealth;
        int currentMoney;
        SpriteFont defaultFont;

        public Player(SpriteBatch SB, /*, OverLord OV*/PlayerIndex PI, int SN, SpriteFont SF)
        {
            playerData = new PlayerData();
            playerData.startMoney = 200;
            playerData.startHealth = 100;

            currentHealth = playerData.startHealth;
            currentMoney = playerData.startMoney;

            spriteBtach = SB;
            screen = SN;
            playerMap = new PlayerMap(this);
            cursor = new Cursor();
            cursor.x = 0;
            cursor.y = 0;
            input = new Inputs(PI);
            defaultFont = SF;
        }

        public Player(SpriteBatch SB, /*, OverLord OV*/int PI, int SN, SpriteFont SF)
        {
            playerData = new PlayerData();
            playerData.startMoney = 50;
            playerData.startHealth = 100;

            currentHealth = playerData.startHealth;
            currentMoney = playerData.startMoney;

            spriteBtach = SB;
            screen = SN;
            playerMap = new PlayerMap(this);
            cursor = new Cursor();
            cursor.x = 0;
            cursor.y = 0;
            input = new Inputs(PI);
            defaultFont = SF;
        }

        public void Init()
        {

        }

        public void damagePlayer(int damage)
        {
            if ((currentHealth -= damage) < 0)
                currentHealth = 0;
        }

        public void addMoney(int money)
        {
            currentMoney += money;
        }

        public void addCreepWave(CreepData CD, double spawnTime, int cpw, SpriteBatch SB)
        {
            playerMap.addCreepWave(CD, spawnTime, cpw, SB);
        }

        public void Update(GameTime gameTime)
        {
            input.Update();
            #region Cursor Input
            if (input.Current.x1 != 0 && input.Previous.x1 == 0)
                if (input.Current.x1 > 0)
                {
                    if ((cursor.y += (int)Math.Ceiling(input.Current.x1)) >= playerMap.HEIGHT)
                        cursor.y = 0;
                    else if (cursor.y < 0)
                        cursor.y = playerMap.HEIGHT - 1;
                }
                else
                {
                    if ((cursor.y += (int)Math.Floor(input.Current.x1)) >= playerMap.HEIGHT)
                        cursor.y = 0;
                    else if (cursor.y < 0)
                        cursor.y = playerMap.HEIGHT - 1;
                }

            if (input.Current.y1 != 0 && input.Previous.y1 == 0)
            {
                if (input.Current.y1 > 0)
                {
                    if ((cursor.x -= (int)Math.Ceiling(input.Current.y1)) >= playerMap.HEIGHT)
                        cursor.x = 0;
                    else if (cursor.x < 0)
                        cursor.x = playerMap.HEIGHT - 1;
                }
                else
                {
                    if ((cursor.x -= (int)Math.Floor(input.Current.y1)) >= playerMap.HEIGHT)
                        cursor.x = 0;
                    else if (cursor.x < 0)
                        cursor.x = playerMap.HEIGHT - 1;
                }
            }
            #endregion


            TowerData tempT = new TowerData();
                tempT.Damage = 10;
                tempT.Range = 100;
                tempT.RateOfFire = 2000;
                tempT.Value = 10;
                if (input.Current.B && !input.Previous.B)
                    if((currentMoney - tempT.Value) >= 0 && playerMap.addTower(tempT, 0, cursor.x, cursor.y, spriteBtach))
                        currentMoney -= tempT.Value;

            playerMap.Update(gameTime);
        }

        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            playerMap.Draw(screen, spriteBtach, graphics, false, gameTime);
            int myH = spriteBtach.GraphicsDevice.Viewport.Height / (playerMap.HEIGHT + 1);
            Vector2 myPos = SplitScreenAdapter.splitConvert(screen, new Vector2(myH*cursor.y, myH*cursor.x), spriteBtach);
            GridManager.DrawRectangle(spriteBtach, new Rectangle((int)myPos.X - 1, (int)myPos.Y - 1, (myH+2)/2, (myH+2)/2), Color.FromNonPremultiplied(0,255,0,100));
            myPos = SplitScreenAdapter.splitConvert(screen, new Vector2(myH * playerMap.HEIGHT, 0), spriteBtach);
            spriteBtach.DrawString(defaultFont, "Race Name: " + playerData.raceName, myPos, Color.Black);
            spriteBtach.DrawString(defaultFont, "$$: " + currentMoney.ToString(), new Vector2(myPos.X, myPos.Y + defaultFont.MeasureString(currentMoney.ToString()).Y), Color.Black);
            spriteBtach.DrawString(defaultFont, "<3: " + currentHealth.ToString(), new Vector2(myPos.X, myPos.Y + defaultFont.MeasureString(currentMoney.ToString()).Y*2), Color.Black);
            
        }
    }

    /// <summary>
    /// Holds all controller data.
    /// </summary>
    struct InputData
    {
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public float lTrigger;
        public float rTrigger;
        public bool lShoulder;
        public bool rShoulder;
        public bool A;
        public bool B;
        public bool X;
        public bool Y;
        public bool dUp;
        public bool dDown;
        public bool dLeft;
        public bool dRight;
    }

    /// <summary>
    /// Handles controller and keyboard input.
    /// Access the data through previous and current.
    /// </summary>
    class Inputs
    {
        PlayerIndex _playerInedx;

        InputData current;
        InputData previous;

        KeyboardState KB;
        GamePadState GP;

        public InputData Current
        {
            get
            {
                return current;
            }
        }
        public InputData Previous
        {
            get
            {
                return previous;
            }
        }

        public Inputs(PlayerIndex index)
        {
            _playerInedx = index;
        }
        public Inputs(int index)
        {
            switch(index)
            {
                case 1:
                    _playerInedx = PlayerIndex.One;
                    break;
                case 2:
                    _playerInedx = PlayerIndex.Two;
                    break;
                case 3:
                    _playerInedx = PlayerIndex.Three;
                    break;
                case 4:
                    _playerInedx = PlayerIndex.Four;
                    break;
            }
        }
        
        public void Update()
        {
            previous = current;
            KB = Keyboard.GetState();
            GP = GamePad.GetState(_playerInedx);

            if (_playerInedx == PlayerIndex.One)
            {
                if (KB.IsKeyDown(Keys.W))
                    current.y1 = 1;
                else if (KB.IsKeyDown(Keys.S))
                    current.y1 = -1;
                else
                    current.y1 = 0;

                if (KB.IsKeyDown(Keys.D))
                    current.x1 = 1;
                else if (KB.IsKeyDown(Keys.A))
                    current.x1 = -1;
                else
                    current.x1 = 0;

                if (KB.IsKeyDown(Keys.Space))
                    current.A = true;
                else
                    current.A = false;

                if (KB.IsKeyDown(Keys.LeftControl))
                    current.B = true;
                else
                    current.B = false;

                if (KB.IsKeyDown(Keys.RightControl))
                    current.X = true;
                else
                    current.X = false;

                if (KB.IsKeyDown(Keys.RightShift))
                    current.Y = true;
                else
                    current.Y = false;

                if (KB.IsKeyDown(Keys.Up))
                    current.y2 = -1;
                else if (KB.IsKeyDown(Keys.Down))
                    current.y2 = -1;
                else
                    current.y2 = 0;

                if (KB.IsKeyDown(Keys.Right))
                    current.x2 = 1;
                else if (KB.IsKeyDown(Keys.Left))
                    current.x2 = -1;
                else
                    current.x2 = 0;
            }
            else
            {
                //Debug.WriteLineIf(_playerInedx == PlayerIndex.One,"KB: " + current.x1 + " " + current.y1);
                //------------------------------------------------------------
                current.y1 = GP.ThumbSticks.Left.Y;
                current.x1 = GP.ThumbSticks.Left.X;
                current.y2 = GP.ThumbSticks.Right.Y;
                current.x2 = GP.ThumbSticks.Right.X;

                //Debug.WriteLineIf(_playerInedx == PlayerIndex.One, "GP: " + current.x1 + " " + current.y1);
                if (GP.Buttons.A == ButtonState.Pressed)
                    current.A = true;
                else
                    current.A = false;

                if (GP.Buttons.B == ButtonState.Pressed)
                    current.B = true;
                else
                    current.B = false;

                if (GP.Buttons.X == ButtonState.Pressed)
                    current.X = true;
                else
                    current.X = false;

                if (GP.Buttons.Y == ButtonState.Pressed)
                    current.Y = true;
                else
                    current.Y = false;

                if (GP.DPad.Up == ButtonState.Pressed)
                    current.dUp = true;
                else
                    current.dUp = false;

                if (GP.DPad.Down == ButtonState.Pressed)
                    current.dDown = true;
                else
                    current.dDown = false;

                if (GP.DPad.Left == ButtonState.Pressed)
                    current.dLeft = true;
                else
                    current.dLeft = false;

                if (GP.DPad.Right == ButtonState.Pressed)
                    current.dRight = true;
                else
                    current.dRight = false;
            }
        }
    }
}