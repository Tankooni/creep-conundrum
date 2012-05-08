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

/*Data log:
 * 4/11 - Schuyler - Added splitConvert()
 * 
 * */

namespace Engine
{
    static class SplitScreenAdapter
    {
        /*Assuming that this is all 2d, and were not using seperate worlds and cameras...*/
        static public Vector2 splitConvert(int Screen, Vector2 myPoint, SpriteBatch gRep)
        {
            Vector2 point = new Vector2(myPoint.X, myPoint.Y);
            point.X /= 2f; point.Y /= 2f;
            if(Screen == 2 || Screen == 4)
                point.X += gRep.GraphicsDevice.Viewport.Width / 2f;
            if (Screen == 3 || Screen == 4)
                point.Y += gRep.GraphicsDevice.Viewport.Height / 2f;
            return point;
        }
        static public void drawLines(SpriteBatch batch)
        {
            int myH = batch.GraphicsDevice.Viewport.Height / (15 + 1) / 2;
            GridManager.DrawLine(batch, myH/5f, Color.Black,
                new Vector2(0, batch.GraphicsDevice.Viewport.Height / 2f - myH/3f),
                new Vector2(batch.GraphicsDevice.Viewport.Width,batch.GraphicsDevice.Viewport.Height / 2f - myH/3f));
            GridManager.DrawLine(batch, myH/5f, Color.Black,
                new Vector2(batch.GraphicsDevice.Viewport.Width / 2f,0),
                new Vector2(batch.GraphicsDevice.Viewport.Width / 2f, batch.GraphicsDevice.Viewport.Height));
        }
        static public Rectangle splitConvert(int Screen, Rectangle myPoint, SpriteBatch gRep)
        {
           // if (Screen < 1 || Screen > 4) return Rectangle.Empty;
            Vector2 temp = splitConvert(Screen, new Vector2(myPoint.X, myPoint.Y), gRep);
            Vector2 temp2 = splitConvert(Screen, new Vector2(myPoint.Width + myPoint.X, myPoint.Height + myPoint.Y), gRep) - temp;
            //Rectangle point = new Rectangle((int)temp.X, (int)temp.Y, (int)temp2.X, (int)temp2.Y);
            return new Rectangle((int)temp.X, (int)temp.Y, (int)temp2.X, (int)temp2.Y);
        }
    }
}
