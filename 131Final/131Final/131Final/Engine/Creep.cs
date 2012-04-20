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
    public class CreepWave : GameObject
    {
        List<Creep> myCreeps = new List<Creep>();
        public PlayerMap mapReference;
        CreepData _cData;
        double creepWaveStartTime;
        public CreepWave(int CreepCount, CreepData cData, SpriteBatch Batch, PlayerMap map, double spawnTime)
        {
            creepWaveStartTime = spawnTime;
            mapReference = map;
            _cData = cData;
            Creep.getCreepOffset(cData);
            for (int j = 0; j < CreepCount; )
            {
                int i;
                for(i = j; (i < j + map.PathCount && i < CreepCount); i++)
                {
                    myCreeps.Add(new Creep(Batch, cData, mapReference.getPath(i - j), spawnTime + j * Creep.getCreepOffset(cData), j, this));
                }
                j = i;
            }

            /*
            for (int x = 1; x <= CreepCount; x++)
            {
                //Number of Paths possible
                //For every spawn interval calculated using CreepData's Speed. (This can be tweaked later)
                myCreeps.Add(new Creep(Batch, cData));
            }
            */
        }
        public List<Creep> creepsInRange(int Range, Vector2 tPos, GameTime gameTime)
        {
            List<Creep> inRange = new List<Creep>();
            for (int x = 0; x < myCreeps.Count; x++)
            {
                if (myCreeps[x].spawnTime < gameTime.TotalGameTime.TotalMilliseconds)
                    if ((myCreeps[x].getPos(gameTime) - tPos).LengthSquared() < (Range * Range))
                        inRange.Add(myCreeps[x]);
            }
            return inRange;
        }
        public void creepDie(Creep toRemove)
        {
            //mapReference.AddMoney(_cData.value);
            myCreeps.Remove(toRemove);
            if (myCreeps.Count < 0)
                mapReference.removeWave(this);
        }
        public override void Update(GameTime gameTime)
        {
            //Update all creeps and spawn creeps as neccecary!
            for (int x = 0; x < myCreeps.Count; x++)
                myCreeps[x].Update(gameTime);
                base.Update(gameTime);
        }
        public void Draw(GameTime gameTime, int Screen)
        {
            for (int x = 0; x < myCreeps.Count; x++)
                myCreeps[x].Draw(gameTime, Screen);
            //tell dem damn creeps to fucking draw!
            base.Draw(gameTime);
        }
    }
    public struct CreepData
    {
        public int Health;
        public int Damage;
        public int mDamage;
        public int Defense;
        public int mDefense;
        public double Speed;
        public int Value;
        //Speed = Blocks per second.
        public Texture2D mySprite;

    }
    public class Creep : GameObject
    {
        SpriteBatch _Batch;
        CreepData _cData;
        CreepWave myWave;
        int waveIndex;

        public double spawnTime;
        public Path _PathOn;
        public Vector2 myPos;
        public int Health;

        /*DONT USE ME!*/
        public Creep()
        {
            
        }
        public static double getCreepOffset(CreepData cData)
        {
            double offset = 1000/cData.Speed;
            return offset;
        }
        public Creep(SpriteBatch batch, CreepData cData, Path spawnPath, double SpawnTime, int myIndex, CreepWave wave)
        {
            if (cData.Health <= 0)
                cData.Health = 100;
            spawnTime = SpawnTime;
            _PathOn = spawnPath;
            _cData = cData;
            _Batch = batch;
            waveIndex = myIndex;
            myWave = wave;
            Health = cData.Health;
        }
        ~Creep()
        {

        }
        public void damageCreep(TowerData tData)
        {
            if (tData.Damage >= 0)
                Health -= (int)(tData.Damage * (1.0 - _cData.Defense / (_cData.Defense + SystemVars.DefenseEffect))) ;
            if (tData.mDamage >= 0)
                Health -= (int)(tData.mDamage * (1.0 - _cData.mDefense / (_cData.mDefense + SystemVars.mDefenseEffect)));
            if (tData.TrueDamage >= 0)
                Health -= tData.TrueDamage;
            if (SystemVars.DEBUG) Debug.WriteLine("Health:" + Health);
            if (Health <= 0)
            {
                myWave.mapReference.addMoney(_cData.Value);
                killCreep();
            }
        }
        void killCreep()
        {
            myWave.creepDie(this);
        }
        public override void Update(GameTime gameTime)
        {
            //Maybe Do It Here! (Calc for its v2 pos) using spawn time, path, and what not.
        }
        public Vector2 getPos(GameTime gameTime)
        {
            double offset = gameTime.TotalGameTime.TotalMilliseconds - spawnTime;
            if (offset > 0)
            {
                int mySquare = _PathOn.PathLength - (int)(offset / 1000 * _cData.Speed) - 1;
                double myProgress = 1 - ((_PathOn.PathLength - (offset / 1000 * _cData.Speed)) - mySquare);
                if (mySquare < 1)
                {
                    myWave.mapReference.damageHero(_cData, Health);   
                    myWave.creepDie(this);
                    mySquare = 1;
                    myProgress = 1.0;
                }
                int myH = _Batch.GraphicsDevice.Viewport.Height / (myWave.mapReference.HEIGHT + 1);
                return new Vector2(
                    (float)(myH * (_PathOn.PathData[mySquare][1] + (_PathOn.PathData[mySquare - 1][1] - _PathOn.PathData[mySquare][1]) * myProgress)),
                    (float)(myH * (_PathOn.PathData[mySquare][0] + (_PathOn.PathData[mySquare - 1][0] - _PathOn.PathData[mySquare][0]) * myProgress)));

            }
            return Vector2.Zero;
        }
        public void Draw(GameTime gameTime, int Screen)
        {
            if (spawnTime < gameTime.TotalGameTime.TotalMilliseconds)
            {
                myPos = SplitScreenAdapter.splitConvert(Screen, getPos(gameTime), _Batch);
                if (_cData.mySprite == null)
                {
                    GridManager.DrawRectangle(_Batch, new Rectangle((int)myPos.X + 2, (int)myPos.Y + 2, 10, 10), Color.PaleVioletRed);
                }
                else
                {
                    //_cData.mySprite.myPos = myPos;
                    //_cData.mySprite.Draw(gameTime);
                }
                float interMed = 2.0f * Health / _cData.Health;
                Color dispColor;
                dispColor = Color.Lerp(Color.Red, Color.Yellow, interMed % 1.0f);
                if (interMed >= 1.0f)
                    dispColor = Color.Lerp(Color.Yellow, Color.FromNonPremultiplied(0, 255, 0, 255), interMed - 1.0f);
                GridManager.DrawLine(_Batch, 1f,
                    dispColor,
                    myPos + new Vector2(1, 0), myPos + new Vector2(2 + 11 * (1.0f * Health / _cData.Health),0)
                    );
            }
        }
    }
}
