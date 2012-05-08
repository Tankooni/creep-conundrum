using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Engine
{
    public static class GridManager
    {

        static Texture2D baseGrid;
        static Texture2D baseTexture;
        static Texture2D blank;
        public static void InitLineDrawer(GraphicsDevice GraphicsDevice)
        {
            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
        }
        public static void InitSquareDrawer(GraphicsDevice GraphicsDevice, int mapHeight)
        {
            int width = GraphicsDevice.Viewport.Height / (mapHeight+1) / 2;
            Color[] temp = new Color[width*width];
            baseGrid = new Texture2D(GraphicsDevice, width, width, false, SurfaceFormat.Color);
            baseTexture = new Texture2D(GraphicsDevice, width, width, false, SurfaceFormat.Color);
            Random RNG = new Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    temp[x * width + y] = new Color(1.0f, 1.0f, 1.0f, (float)RNG.NextDouble()/5+0.75f);
                }
            }
            baseTexture.SetData(temp);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    temp[x * width + y] = Color.Transparent;
                }
            }
            /*for (int x = 0; x < width; x++)
            {
                temp[x+width] = Color.Black;
                temp[x * width+1] = Color.Black;
                temp[(x + 1) * width - 2] = Color.Black;
                temp[width * (width-1) - x - 1] = Color.Black;
            }*/
            temp[2 * width + 0] = Color.Black;
            temp[1 * width + 1] = Color.Black;
            temp[0 * width + 2] = Color.Black;

            temp[3 * width - 1] = Color.Black;
            temp[2 * width - 2] = Color.Black;
            temp[1 * width - 3] = Color.Black;

            temp[width * width - 3] = Color.Black;
            temp[(width-1) * width - 2] = Color.Black;
            temp[(width-2) * width - 1] = Color.Black;

            temp[(width - 1) * width + 2] = Color.Black;
            temp[(width - 2) * width + 1] = Color.Black;
            temp[(width - 3) * width + 0] = Color.Black;
            /*temp[width-1] = Color.Black;
            temp[width * (width - 1)] = Color.Black;
            temp[width * width - 1] = Color.Black;*/
            baseGrid.SetData(temp);
        }
        public static void DrawSquare(SpriteBatch spriteBatch, Vector2 Point, Color toDraw, bool Grid, PlayerMap mapReference)
        {
            if (baseTexture == null) InitSquareDrawer(spriteBatch.GraphicsDevice, mapReference.HEIGHT);
            spriteBatch.Draw(baseTexture, Point, toDraw);
            if (Grid) spriteBatch.Draw(baseGrid, Point, Color.White);
        }
        public static void DrawLine(SpriteBatch spriteBatch,
           float width, Color color, Vector2 point1, Vector2 point2)
        {
            if (blank == null)
            {
                InitLineDrawer(spriteBatch.GraphicsDevice);
            }

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
        /*
        public static void DrawLine(SpriteBatch spriteBatch,
           float width, Color color, Vector2 point1, Vector2 point2)
        {
            if (blank == null)
            {
                InitLineDrawer(spriteBatch.GraphicsDevice);
            }

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
        public static void DrawOutline(SpriteBatch spriteBatch, Rectangle rec, Color color)
        {
            DrawLine(spriteBatch, 3.0f, Color.Red,
                new Vector2(rec.X, rec.Y), new Vector2(rec.X + rec.Width, rec.Y));
            DrawLine(spriteBatch, 3.0f, Color.Red,
                new Vector2(rec.X + rec.Width, rec.Y), new Vector2(rec.X + rec.Width, rec.Y + rec.Height));
            DrawLine(spriteBatch, 3.0f, Color.Red,
                new Vector2(rec.X + rec.Width, rec.Y + rec.Height), new Vector2(rec.X, rec.Y + rec.Height));
            DrawLine(spriteBatch, 3.0f, Color.Red,
                new Vector2(rec.X, rec.Y + rec.Height), new Vector2(rec.X, rec.Y));
        }*/
        public static void DrawRectangle(SpriteBatch spriteBatch,
            Rectangle rec, Color color)
        {
            if (blank == null)
            {
                //InitLineDrawer(spriteBatch.GraphicsDevice, PlayerMap.HEIGHT);
            }
            DrawLine(spriteBatch, rec.Width, color,
                new Vector2(rec.X + (rec.Width), rec.Y),
                new Vector2(rec.X + (rec.Width), rec.Y + rec.Height));
        }

    }


}
