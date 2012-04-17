using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Path
    {
        PlayerMap mapReference;
        List<int[]> _Path;
        int[] _Rating;
        /*This constructer is deprecated as you must always pass in the pre-pathing with the start path of 5*/
        public Path(PlayerMap map)
        {
            mapReference = map;
        }
        /*public Path(PlayerMap map, Path prePathing)
        {
            mapReference = map;
            _Path = prePathing.PathData;
            _Rating = prePathing.RatingData;
            spawnPathData(_Path[_Path.Count - 1][0], _Path[_Path.Count - 1][1]);
        }*/
        public Path(PlayerMap map, List<int[]> pathData)
        {
            mapReference = map;
            _Path = pathData;
            spawnPathData(_Path[_Path.Count - 1][0], _Path[_Path.Count - 1][1]);
            int[] toCheck = _Path[_Path.Count - 1];
            if (SystemVars.DEBUG) Debug.WriteLine(toCheck.ToString());
            if (toCheck[0] != 0 && toCheck[0] != 14 && toCheck[1] != 0 && toCheck[1] != 14)
                _Path.RemoveRange(0, _Path.Count-1);
        }

        public static Path NullPath
        {
            get
            {
                return new Path(null, new List<int[]>());
            }
        }
        public override string ToString()
        {
            string myOut = "";
            if (_Path.Count > 0)
            {
                int x;
                for (x = 0; x < _Path.Count-1; x++)
                {
                    myOut += "{" + _Path[x][0] + "," + _Path[x][1] + "}, ";
                }
                myOut += "{" + _Path[x][0] + "," + _Path[x][1] + "}";
            }
            return myOut;
        }
        void spawnPathData(int X, int Y)
        {
            List<int[]> temp; int i = X, j = Y;
            while ((temp = subProccesPathData(i, j)).Count > 0)
            {
                _Path.Add(temp[0]);
                i = temp[0][0]; j = temp[0][1];
                temp.RemoveAt(0);
                while (temp.Count > 0)
                {
                    if (SystemVars.DEBUG) Debug.WriteLine("Spawning new path");
                    List<int[]> toPass = new List<int[]>();
                    for (int k = 0; k < _Path.Count - 1; k++)
                        toPass.Add(_Path[k]);
                    toPass.Add(temp[0]);
                    if (SystemVars.DEBUG) Debug.WriteLine(_Path.Count + "==" + toPass.Count);
                    mapReference.spawnNewPath(toPass);
                    toPass.RemoveAt(0);
                    temp.RemoveAt(0);


                    /*
                     * So this works adding recursily all data to _Path, not seperating them...
                    _Path.Add(temp[0]);
                    spawnPathData(temp[0][0], temp[0][1]);
                    temp.RemoveAt(0);
                     * */
                }
            }
                
        }
        
        List<int[]> subProccesPathData(int X, int Y)
        {
            List<int[]> Data = new List<int[]>();
            for (int i = X - 1; i <= X + 1; i++)
            {
                for (int j = Y - 1; j <= Y + 1; j++)
                {
                    int[] temp = { i, j };
                    if(Math.Abs(i-X) != Math.Abs(j-Y))
                        if (mapReference.getData(i, j) == 1 && !_Path.Exists((delegate(int[] k) { return (temp[0] == k[0] && temp[1] == k[1]); })))
                            Data.Add(temp);
                }
            }
            if (SystemVars.DEBUG)
                for(int x=0;x<Data.Count;x++)
                    Debug.WriteLine(Data[x][0]+","+Data[x][1]+":"+_Path.Contains(Data[x]));
            return Data;
        }
        public int PathLength
        {
            get
            {
                return _Path.Count;
            }
        }
        public List<int[]> PathData
        {
            get
            {
                return _Path;
            }
        }
        public int[] RatingData
        {
            get
            {
                return _Rating;
            }
        }
    }
}
