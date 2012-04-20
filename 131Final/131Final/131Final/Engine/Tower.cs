using System;
using System.Diagnostics;
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
using Base;

namespace Engine
{
    public struct TowerData
    {
        //Base Things
        public int Damage;
        public int mDamage;
        public double Slow;//0 to 1.0 where 1.0 = frozen
        public int MoneyGained;
        public int RateOfFire;//In MilliSeconds Between shots
        public int Range;

        //Later:
        public int Poison;
        public int PoisonDuration;
        public int PoisonTPS;//Ticks Per Second
        public int DefenseAdjust;//Plus Or Minus can be detect with IF for Enemy lower or Friendly Raise
        public int mDefenseAdjust;//What he said above me.
        public int ChainCount;//Amount it chains to
        public Texture2D TowerSprite;
    }

    public class Tower : GameObject
    {
        /*Needs levels, build time, and what not*/
        PlayerMap mapReference;
        TowerData _tData;
        SpriteBatch _Batch;
        Vector2 _myPos;
        Color myColor = Color.Blue;
        double fireTime;

        public Tower(PlayerMap map, SpriteBatch Batch, TowerData tData, int[] myPos)
        {
            mapReference = map;
            _tData = tData;
            _Batch = Batch;
            int myH = _Batch.GraphicsDevice.Viewport.Height / (PlayerMap.HEIGHT + 1);
            _myPos = new Vector2(myH * myPos[1]/*y*/, myH * myPos[0]/*x*/);
            fireTime = 0;
        }
        public void fireBullet(GameTime gameTime)
        {
            List<Creep> temp = mapReference.getCreepsInRange(_myPos, _tData.Range, gameTime);
            if (temp.Count > 0)
            {
                if (SystemVars.DEBUG) Debug.WriteLine("Creep Found!");
                temp[0].damageCreep(_tData.Damage, _tData.mDamage, 0, _tData.Slow);
                fireTime = gameTime.TotalGameTime.TotalMilliseconds + _tData.RateOfFire;
                myColor = Color.Red;
            }
            else
            {
                fireTime = 0.0;
            }
        
        }
        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > fireTime)
                fireBullet(gameTime);
            else
                myColor = Color.Blue;
        }
        public void Draw(GameTime gameTime, int Screen)
        {
            Vector2 myPos = SplitScreenAdapter.splitConvert(Screen, _myPos, _Batch);
            if (_tData.TowerSprite == null)
            {
                GridManager.DrawRectangle(_Batch, new Rectangle((int)myPos.X + 2, (int)myPos.Y + 2, 10, 10), myColor);
            }
            else
            {
                //_cData.mySprite.myPos = myPos;
                //_cData.mySprite.Draw(gameTime);
            }
        }
    }
}
