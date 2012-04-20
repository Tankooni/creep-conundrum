/*
 * 4/19/2012 Player Class started
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        Inputs input;


        public Player(SpriteBatch SB, /*, OverLord OV*/PlayerIndex PI, int SN, PlayerMap PM)
        {
            spriteBtach = SB;
            screen = SN;
            playerMap = new PlayerMap();
            cursor = new Cursor();
            cursor.x = 0;
            cursor.y = 0;
            input = new Inputs(PI);
            playerMap = PM;
        }

        public void Update(GameTime gameTime)
        {
            input.Update();
            if (input.Current.x1 != 0 && input.Previous.x1 == 0)
                cursor.y += (int)Math.Ceiling(input.Current.x1);
            if (input.Current.y1 != 0 && input.Previous.y1 == 0)
                cursor.x -= (int)Math.Ceiling(input.Current.y1);


            Console.WriteLine(input.Current.y1 + " " + input.Previous.y1);


            playerMap.Update(gameTime);
        }

        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            playerMap.Draw(screen, spriteBtach, graphics, true, gameTime);
            int myH = spriteBtach.GraphicsDevice.Viewport.Height / (PlayerMap.HEIGHT + 1);
            Vector2 myPos = SplitScreenAdapter.splitConvert(screen, new Vector2(myH*cursor.y, myH*cursor.x), spriteBtach);
            GridManager.DrawRectangle(spriteBtach, new Rectangle((int)myPos.X - 1, (int)myPos.Y - 1, (myH+2)/2, (myH+2)/2), Color.FromNonPremultiplied(0,255,0,100));

            
            
        }
    }

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

        //public override string ToString()
        //{
        //    return "{ x1:" + x1 + ", y1" + y1 + ", " + x2 + ", " + x1 + ", " + x1 + ", " + x1 + ", " + x1 + ", " +
        //}
        //public void Init()
        //{
        //    x1 = 0;
        //    y1 = 0;
        //    x2 = 0;
        //    y2 = 0;
        //    lTrigger = 0;
        //    rTrigger = 0;
        //    lShoulder = false;
        //    rShoulder = false;
        //    A = false;

        //}
    }

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
        
        public void Update()
        {
            previous = current;
            KB = Keyboard.GetState();

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

            //------------------------------------------------------------
            //current.y1 = GP.ThumbSticks.Left.Y;
            //current.x1 = GP.ThumbSticks.Left.X;
            //current.y2 = GP.ThumbSticks.Right.Y;
            //current.x2 = GP.ThumbSticks.Right.X;

            //if (GP.Buttons.A == ButtonState.Pressed)
            //    current.A = true;
            //else
            //    current.A = false;

            //if (GP.Buttons.B == ButtonState.Pressed)
            //    current.B = true;
            //else
            //    current.B = false;

            //if (GP.Buttons.X == ButtonState.Pressed)
            //    current.X = true;
            //else
            //    current.X = false;

            //if (GP.Buttons.Y == ButtonState.Pressed)
            //    current.Y = true;
            //else
            //    current.Y = false;

            //if (GP.DPad.Up == ButtonState.Pressed)
            //    current.dUp = true;
            //else
            //    current.dUp = false;

            //if (GP.DPad.Down == ButtonState.Pressed)
            //    current.dDown = true;
            //else
            //    current.dDown = false;

            //if (GP.DPad.Left == ButtonState.Pressed)
            //    current.dLeft = true;
            //else
            //    current.dLeft = false;

            //if (GP.DPad.Right == ButtonState.Pressed)
            //    current.dRight = true;
            //else
            //    current.dRight = false;
        }
    }
}