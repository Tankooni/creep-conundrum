using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Engine
{
    /* Class Added April 20th -- Michael Elser */

    /* CAUTION TO READER OF THIS CLASS */
    //You are about to look at the most mother-fucking ugliest class you've ever seen in your life.
    //By reading this block comment you agree to not hold the creator of this code liable under any circumstances for any/all situations that may occur by laying eyes on this class
    //Enjoy.
    /* END OF CAUTION STATEMENT */

    public static class DataParser
    {
        public static PlayerData getRaceData(string raceNameIN)
        {
            PlayerData pData = new PlayerData();
            pData.raceName = raceNameIN;
            pData = getBasicData(pData, pData.raceName);
            pData.playerMap = getMapData(raceNameIN);
            pData.towerList = getAllTowers(raceNameIN);
            pData.creepList = getAllMinions(raceNameIN);
            return pData;
        }

        private static PlayerData getBasicData(PlayerData pData, string raceNameIN)
        {
            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            if (line == "RName:" + raceNameIN)
                            {
                                while (line != "<Towers>")
                                {
                                    line = stream.ReadLine();
                                    int temp;
                                    if ((temp = line.IndexOf(':')) != -1)
                                    {
                                        switch (line.Substring(0, temp))
                                        {
                                            case "Gold":
                                                pData.startMoney = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                break;
                                            case "Health":
                                                pData.startHealth = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return pData;
        }

        private static List<TowerData> getAllTowers(string raceNameIN)
        {
            List<TowerData> allTD = new List<TowerData>();
            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                       line = stream.ReadLine();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            if (line == "RName:" + raceNameIN)
                            {
                                while (line != "</Race>")
                                {
                                    line = stream.ReadLine();
                                    if (line == "<Towers>")//Now we're in tower data
                                    {
                                        string group = "null";
                                        while (line != "</Towers>")
                                        {
                                            line = stream.ReadLine();
                                            int temp;
                                            if ((temp = line.IndexOf(':')) != -1)
                                            {
                                                switch (line.Substring(0, temp))
                                                {
                                                    case "GROUP":
                                                        group = (line.Substring(temp + 1, (line.Length - temp) - 1));
                                                        break;
                                                }
                                            }
                                            if (line == "<TOWER>")//Now we're grabbing a tower
                                            {
                                                TowerData tempTower = new TowerData();
                                                while (line != "</TOWER>")
                                                {
                                                    line = stream.ReadLine();
                                                    if ((temp = line.IndexOf(':')) != -1)
                                                    {
                                                        switch (line.Substring(0, temp))
                                                        {
                                                            case "Name":
                                                                tempTower.name = line.Substring(temp + 1, (line.Length - temp) - 1);
                                                                break;
                                                            case "Slow":
                                                                tempTower.Slow = double.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "Cost":
                                                                tempTower.Value = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "Damage":
                                                                tempTower.Damage = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "mDamage":
                                                                tempTower.mDamage = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "ROF":
                                                                tempTower.RateOfFire = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "Range":
                                                                tempTower.Range = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                        }
                                                    }
                                                }
                                                tempTower.group = group;
                                                allTD.Add(tempTower);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return allTD;
        }

        private static List<CreepData> getAllMinions(string raceNameIN)
        {
            List<CreepData> allMD = new List<CreepData>();
            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            if (line == "RName:" + raceNameIN)
                            {
                                while (line != "</Race>")
                                {
                                    line = stream.ReadLine();
                                    if (line == "<Minions>")//Now we're in tower data
                                    {
                                        string group = "null";
                                        while (line != "</Minions>")
                                        {
                                            line = stream.ReadLine();
                                            int temp;
                                            if ((temp = line.IndexOf(':')) != -1)
                                            {
                                                switch (line.Substring(0, temp))
                                                {
                                                    case "GROUP":
                                                        group = (line.Substring(temp + 1, (line.Length - temp) - 1));
                                                        break;
                                                }
                                            }
                                            if (line == "<MINION>")//Now we're grabbing a tower
                                            {
                                                CreepData tempCreep = new CreepData();
                                                while (line != "</MINION>")
                                                {
                                                    line = stream.ReadLine();
                                                    if ((temp = line.IndexOf(':')) != -1)
                                                    {
                                                        switch (line.Substring(0, temp))
                                                        {
                                                            case "Name":
                                                                tempCreep.name = (line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "Speed":
                                                                tempCreep.Speed = double.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "Cost":
                                                                tempCreep.Value = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "Damage":
                                                                tempCreep.Damage = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "mDamage":
                                                                tempCreep.mDamage = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                            case "Health":
                                                                tempCreep.Health = int.Parse(line.Substring(temp + 1, (line.Length - temp) - 1));
                                                                break;
                                                        }
                                                    }
                                                }
                                                tempCreep.group = group;
                                                Console.WriteLine(tempCreep.Damage);
                                                allMD.Add(tempCreep);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return allMD;
        }

        private static byte[,] getMapData(string raceNameIN)
        {
            byte[,] mapData = { { 0 } };
            //Schuyer was here, and he totaly fondeled this code >:D
            //He also noticed how it wasnt RaceData.CCD
            //But he forgives you for that.

            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            if (line == "RName:" + raceNameIN)
                            {
                                while (line != "</Race>")
                                {
                                    line = stream.ReadLine();
                                    if (line == "<MapData>")
                                    {
                                        while (line != "</MapData>")
                                        {
                                            line = stream.ReadLine();
                                            if (line == "<ARRAY>") //Check to see if we're looking at an ARRAY group
                                            {
                                                line = stream.ReadLine();
                                                line = line.Trim();
                                                int size = line.Length;
                                                mapData = new byte[size, size];
                                                while (line != "</ARRAY>")
                                                {
                                                    for (int y = 0; y < size; y++)
                                                    {
                                                        for (int x = 0; x < line.Length; x++)
                                                        {
                                                            byte temp = byte.Parse("" + line[x]);//Grab the line and make it a numbaaaahhhh
                                                            mapData[y, x] = temp;
                                                        }
                                                        line = stream.ReadLine();
                                                        line = line.Trim();
                                                    }
                                                }
                                                return mapData;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return mapData;
        }
    }
}
