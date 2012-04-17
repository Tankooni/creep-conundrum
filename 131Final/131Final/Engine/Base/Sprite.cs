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
    public class BaseSprite : GameObject
    {
        /*Sprite Vars:*/
        protected String _TextureName;
        protected Texture2D _SpriteTexture;
        protected Vector2 _SpritePos;
        protected Color _SpriteTint;
        /*Game Vars*/
        protected ContentManager gameContent;
        protected SpriteBatch gameSpriteBatch;

        /*Constructer*/
        public BaseSprite(ContentManager GC, SpriteBatch GSB, String Texture, Vector2 Pos, Color color)
        {
            Tint = color;
            myPos = Pos;
            _TextureName = Texture;
            gameContent = GC;
            gameSpriteBatch = GSB;
            Load();
        }
        /*Personal Methods*/
        public override void Draw(GameTime gameTime)
        {
            gameSpriteBatch.Draw(_SpriteTexture, _SpritePos, _SpriteTint);
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        private void Load()
        {
            _SpriteTexture = gameContent.Load<Texture2D>(_TextureName);
        }
        /*Manipulators*/
        public Color Tint
        {
            set
            {
                _SpriteTint = value;
            }
            get
            {
                return _SpriteTint;
            }
        }
        public float X
        {
            set
            {
                _SpritePos.X = value;
            }
            get
            {
                return _SpritePos.X;
            }
        }
        public float Y
        {
            set
            {
                _SpritePos.Y = value;
            }
            get
            {
                return _SpritePos.Y;
            }
        }
        public Vector2 myPos
        {
            set
            {
                _SpritePos = value;
            }
            get
            {
                return _SpritePos;
            }
        }
        public virtual Vector2 Size
        {
            get
            {
                return new Vector2(_SpriteTexture.Width, _SpriteTexture.Height);
            }
        }
    }
}
