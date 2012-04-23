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

namespace Base
{
    class SpriteSheet : BaseSprite
    {
        Vector2 frameDivisions;
        int currentFrame = 0;
        public SpriteSheet(ContentManager GC, SpriteBatch GSB, String Texture, Vector2 Pos, Color color, Vector2 frameInfo)
            : base(GC, GSB, Texture, Pos, color)
        {
            frameDivisions = frameInfo;
        }
        public Rectangle getFrame
        {
            get
            {
                return new Rectangle(
                    (int)(_SpriteTexture.Width / frameDivisions.X * (currentFrame % (int)(frameDivisions.X))),
                    (int)(_SpriteTexture.Height / frameDivisions.Y * (currentFrame / (int)(frameDivisions.X))),
                    (int)(_SpriteTexture.Width / frameDivisions.X),
                    (int)(_SpriteTexture.Height / frameDivisions.Y));
            }
        }
        public int myFrame
        {
            set
            {
                if (value >= 0 && value < frameDivisions.X * frameDivisions.Y)
                    currentFrame = value;
                else
                {
                    if (value < 0)
                        currentFrame = (int)(frameDivisions.X * frameDivisions.Y)-1;
                    else
                        currentFrame = 0;
                }
            }
            get
            {
                return currentFrame;
            }
        }
        public override Vector2 Size
        {
            get
            {
                return new Vector2(base.Size.X / frameDivisions.X, base.Size.Y / frameDivisions.Y);
            }
        }
        public void nextFrame()
        {
            currentFrame++;
            if (currentFrame >= frameDivisions.X * frameDivisions.Y) currentFrame = 0;
        }
        void UpdateSprite()
        {
            gameSpriteBatch.Draw(_SpriteTexture, _SpritePos, getFrame , _SpriteTint);
        }
        public override void Draw(GameTime gameTime)
        {
            UpdateSprite();
        }
        public Rectangle myRec
        {
            get
            {
                return new Rectangle((int)X, (int)Y, (int)Size.X, (int)Size.Y);
            }
        }
    }
}
