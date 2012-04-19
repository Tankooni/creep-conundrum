﻿using System;
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
/*Data Log
 * 4/11 - Schuyler - Created (WIDTH,HEIGHT,myMap,drawMap)
 * 
 * */

namespace Engine
{
    public class PlayerMap : GameObject
    {
        public const int HEIGHT = 15;
        List<Path> mapPaths = new List<Path>();
        List<Tower> myTowers = new List<Tower>();
        List<CreepWave> myCreepWaves = new List<CreepWave>();
        //The following will actually not be defined in here!
        //0 = tower pos
        //1 = path
        //2 = noTowers
        //3 = noCrossing or towers
        //5 = nexus
        //6 = towerAlready
        byte[,] myMap ={
        {0,1,1,1,1,1,1,0,0,1,0,0,0,0,0},
        {0,1,0,0,0,0,1,0,0,1,0,0,0,0,0},
        {0,1,0,0,0,0,1,0,0,1,0,0,0,0,0},
        {0,1,0,0,0,0,1,0,0,1,0,0,0,0,0},
        {0,1,1,1,0,0,1,1,1,1,0,0,0,0,0},
        {0,0,0,1,0,0,0,0,0,2,0,1,0,0,0},
        {0,0,0,1,0,0,0,0,0,2,0,0,0,0,0},
        {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1},
        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {0,0,0,1,0,0,0,3,1,3,0,0,0,0,0},
        {0,0,0,1,0,0,0,3,1,3,0,0,0,0,0},
        {0,0,0,1,0,0,3,3,1,3,3,0,0,0,0},
        {0,0,0,1,0,3,3,3,1,3,3,3,0,0,0},
        {0,0,0,5,0,3,3,3,3,3,3,3,0,0,0}};



        public PlayerMap()
        {
            initMapPaths();
            if (SystemVars.DEBUG)
            {
                for (int x = 0; x < PathCount; x++)
                {
                    Debug.WriteLine(mapPaths[x].ToString());
                }
            }
        }
        ~PlayerMap()
        {

        }

        public void addTower(TowerData tData, double buildTime, int X, int Y, SpriteBatch _Batch)
        {
            /*Build time isnt implemented*/
            if (getData(X, Y) == 0)
            {
                myTowers.Add(new Tower(this, _Batch, tData, new int[] { X, Y }));
                myMap[X, Y] = 6;
            }

        }

        /*Creep Stuff*/
        public void addCreepWave(CreepData cData, double spawnTime, int creepCount, SpriteBatch _Batch)
        {
            myCreepWaves.Add(new CreepWave(creepCount, cData, _Batch, this, spawnTime));
        }
        public void removeWave(CreepWave myWave)
        {
            myCreepWaves.Remove(myWave);
        }
        public List<Creep> getCreepsInRange(Vector2 tPos, int Range, GameTime gameTime)
        {
            List<Creep> inRange = new List<Creep>();
            for (int x = 0; x < myCreepWaves.Count; x++)
            {
                inRange.AddRange(myCreepWaves[x].creepsInRange(Range, tPos, gameTime));
                    //.Concat(myCreepWaves[x].creepsInRange(Range, tPos, gameTime));
                if (SystemVars.DEBUG) Debug.WriteLine(inRange.Count);
            }
            return inRange;
        }
        /// <summary>
        /// Updates all componenets that fall under MAP
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            for (int x = 0; x < myCreepWaves.Count; x++)
                myCreepWaves[x].Update(gameTime);
            for (int x = 0; x < myTowers.Count; x++)
                myTowers[x].Update(gameTime);
        }
        //Draws the current map usign the split screen adapter and also the paths
        public void Draw(int Screen, SpriteBatch batch, GraphicsDeviceManager graphics, bool Grid, GameTime gameTime){
            //Map Drawing:
            int myH = batch.GraphicsDevice.Viewport.Height / (HEIGHT+1);
            for (int x = 0; x < HEIGHT; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    Color color = Color.Gray;
                    if(myMap[y, x] == 1) color = Color.SandyBrown;
                    if(myMap[y, x] == 2) color = Color.Black;
                    if(myMap[y, x] == 3) color = Color.Purple;
                    if(myMap[y, x] == 5) color = Color.Blue;
                    //if(myMap[y, x] == 6) color = Color.Black;
                    GridManager.DrawSquare(batch,
                        SplitScreenAdapter.splitConvert(Screen, new Vector2(myH * x, myH * y), batch),
                        color,
                        Grid);
                    
                }
            }
            //Path Drawing:
            /*
            for (int y = 0; y < mapPaths.Count; y++)
            {
                for (int x = 0; x < mapPaths[y].PathLength; x++)
                {
                    Color color = Color.Black;
                    GridManager.DrawSquare(batch,
                        SplitScreenAdapter.splitConvert(Screen, new Vector2(myH * mapPaths[y].PathData[x][1], myH * mapPaths[y].PathData[x][0]), batch),
                        color,
                        Grid);
                }
            }*/
            //DraCreep Waves!
            for (int x = 0; x < myCreepWaves.Count; x++)
                myCreepWaves[x].Draw(gameTime, Screen);
            for (int x = 0; x < myTowers.Count; x++)
                myTowers[x].Draw(gameTime, Screen);
        }
        /*Path stuff*/
        //Used by the Path class to tree branch
        public void spawnNewPath(List<int[]> currentPathData)
        {
            mapPaths.Add(new Path(this, currentPathData));
        }
        //Gets the map data at X and Y
        public int getData(int X, int Y)
        {
            if (SystemVars.DEBUG) Debug.WriteLine("X:" + X + ",Y:" + Y);
            if (X >= HEIGHT || Y >= HEIGHT || X < 0 || Y < 0)
                return 0;
            return myMap[X, Y];
        }
        //Returns the Number of paths that goes with map data
        public int PathCount
        {
            get
            {
                return mapPaths.Count;
            }
        }
        public void removePath(Path toRemove)
        {
            for (int x = 0; x < mapPaths.Count; x++)
            {
                Debug.WriteLine(mapPaths[x].ToString() + " == " + toRemove.ToString());
                if (mapPaths[x].ToString() == toRemove.ToString())
                {
                    mapPaths.RemoveAt(x);
                }
            }
        }
        /// <summary>
        /// Returns the correct path indexed to pathNumber
        /// </summary>
        /// <param name="pathNumber">int pathNumber</param>
        /// <returns>Path</returns>
        public Path getPath(int pathNumber)
        {
            if (pathNumber < 0 || pathNumber >= PathCount)
                return Path.NullPath;
            return mapPaths[pathNumber];
        }
        /// <summary>
        /// DEPRECATED - No longer necessary.
        /// </summary>
        /// <param name="doesContain"></param>
        /// <returns></returns>
        public bool containsPath(Path doesContain)
        {
            bool toReturn = false;
            for (int x = 0; x < PathCount; x++)
                if (mapPaths[x].ToString().Equals(doesContain.ToString()))
                    toReturn = true;
            return toReturn;
        }
        //Begins to genereate the Paths from the map data
        void initMapPaths()
        {
            int[] myPos = { 0, 0 };
            while (myMap[myPos[0], myPos[1]] != 5 || (myPos[0] == myPos[1] && myPos[0] == HEIGHT))
            {
                if (++myPos[0] >= HEIGHT) { myPos[0] = 0; myPos[1]++; }
            }
            if (SystemVars.DEBUG) Debug.WriteLine("MyPos:" + myPos[0] + "," + myPos[1]);
            spawnNewPath(new List<int[]>(new int[][] { myPos, myPos }));
            for (int x = 0; x < mapPaths.Count; x++)
                if (mapPaths[x].PathLength == 0)
                    mapPaths.RemoveAt(x);
        }
    }
}
