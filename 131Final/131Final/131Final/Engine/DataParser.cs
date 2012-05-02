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

        public static List<List<TowerData>> getAllTowerData(string raceNameIN)
        {
            //^Are you fucking serious?
            List<List<TowerData>> totalTowerData = new List<List<TowerData>>();
            Console.WriteLine("List of TowerData created");

            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                line = line.Trim();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        line = line.Trim();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            line = line.Trim();
                            if (line == "<RaceName>") //Check to see if we're looking at a <RaceName> tag
                            {
                                line = stream.ReadLine();
                                line = line.Trim();
                                if (line == raceNameIN) //Check to see if it's the race we're looking for
                                {
                                    //Good now we can grab data!
                                    while (line != "</Race>") //Check to make sure we're still in the same race tag
                                    {
                                        line = stream.ReadLine();
                                        line = line.Trim();
                                        if (line == "<Towers>") //We're now grabbing all the towers for the race
                                        {

                                            while (line != "</Towers>") //Check to make sure we're still in the same towers tag
                                            {
                                                line = stream.ReadLine();
                                                line = line.Trim();
                                                if (line == "<TowerGroup>") //Check to see if we're looking at group info
                                                {
                                                    List<TowerData> groupTowerData = new List<TowerData>();
                                                    Console.WriteLine("Group Created");
                                                    string towerGroup;
                                                    towerGroup = "Temp";
                                                    while (line != "</TowerGroup>") //Check to make sure we're still in the same tower group
                                                    {
                                                        line = stream.ReadLine();
                                                        line = line.Trim();
                                                        if (line == "<GroupName>")
                                                        {
                                                            line = stream.ReadLine();
                                                            line = line.Trim();
                                                            towerGroup = line;
                                                        }
                                                        if (line == "<TowerData>")
                                                        {
                                                            TowerData currentCreep = new TowerData();
                                                            Console.WriteLine("Tower Created");
                                                            while (line != "</TowerData>")
                                                            {
                                                                line = stream.ReadLine();
                                                                line = line.Trim();
                                                                if (line == "<Name>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.name = line;
                                                                }
                                                                else if (line == "<slow>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.Slow = double.Parse(line);
                                                                }
                                                                else if (line == "<cost>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.cost = int.Parse(line);
                                                                }
                                                                else if (line == "<damage>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.Damage = int.Parse(line);
                                                                }
                                                                else if (line == "<mDamage>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.mDamage = int.Parse(line);
                                                                }
                                                                else if (line == "<rateOfFire>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.RateOfFire = int.Parse(line);
                                                                }
                                                            }
                                                            currentCreep.group = towerGroup;
                                                            groupTowerData.Add(currentCreep);
                                                            Console.WriteLine("Tower Added to Group");
                                                        }
                                                    }
                                                    Console.WriteLine("Group added to List of TowerData");
                                                    totalTowerData.Add(groupTowerData);
                                                }
                                            }
                                            stream.Close();
                                            stream.Dispose();
                                            Console.WriteLine("List of TowerData returned");
                                            return totalTowerData;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    //We're not in a proper raceData file...
                    stream.Close();
                    stream.Dispose();
                }
            }
            return totalTowerData;
        }

        public static List<TowerData> getGroupTowerData(string raceNameIN, string groupNameIN)
        {
            List<TowerData> groupTowerData = new List<TowerData>();
            Console.WriteLine("List of TowerData created");

            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                line = line.Trim();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        line = line.Trim();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            line = line.Trim();
                            if (line == "<RaceName>") //Check to see if we're looking at a <RaceName> tag
                            {
                                line = stream.ReadLine();
                                line = line.Trim();
                                if (line == raceNameIN) //Check to see if it's the race we're looking for
                                {
                                    //Good now we can grab data!
                                    while (line != "</Race>") //Check to make sure we're still in the same race tag
                                    {
                                        line = stream.ReadLine();
                                        line = line.Trim();
                                        if (line == "<Towers>") //We're now grabbing all the towers for the race
                                        {
                                            while (line != "</Towers>") //Check to make sure we're still in the same towers tag
                                            {
                                                line = stream.ReadLine();
                                                line = line.Trim();
                                                if (line == "<TowerGroup>") //Check to see if we're looking at group info
                                                {
                                                    line = stream.ReadLine();
                                                    line = line.Trim();
                                                    if (line == "<GroupName>")// We're now grabbing a tower group
                                                    {
                                                        line = stream.ReadLine();
                                                        line = line.Trim();
                                                        if (line == groupNameIN) //If it's the group we're looking for we're going to start grabbing all the towers for that group
                                                        {
                                                            while (line != "</TowerGroup>") //Check to make sure we're still in the same tower group
                                                            {
                                                                TowerData currentCreep = new TowerData();
                                                                Console.WriteLine("Tower Created");
                                                                while (line != "</TowerData>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    if (line == "<Name>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.name = line;
                                                                    }
                                                                    else if (line == "<slow>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.Slow = double.Parse(line);
                                                                    }
                                                                    else if (line == "<cost>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.cost = int.Parse(line);
                                                                    }
                                                                    else if (line == "<damage>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.Damage = int.Parse(line);
                                                                    }
                                                                    else if (line == "<mDamage>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.mDamage = int.Parse(line);
                                                                    }
                                                                    else if (line == "<rateOfFire>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.RateOfFire = int.Parse(line);
                                                                    }
                                                                }
                                                                currentCreep.group = groupNameIN;
                                                                groupTowerData.Add(currentCreep);
                                                                Console.WriteLine("Tower Added to Group");
                                                                line = stream.ReadLine();
                                                                line = line.Trim();
                                                            }
                                                        }
                                                    }
                                                    //Console.WriteLine("Group added to List of TowerData");
                                                    //groupTowerData.Add(groupTowerData);
                                                }
                                            }
                                            stream.Close();
                                            stream.Dispose();
                                            Console.WriteLine("List of TowerData returned");
                                            return groupTowerData;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    //We're not in a proper raceData file...
                    stream.Close();
                    stream.Dispose();
                }
            }
            return groupTowerData;
        }

        public static TowerData getSingleTowerData(string raceNameIN, string groupNameIN, string towerNameIN)
        {
            TowerData singleTowerData = new TowerData();
            Console.WriteLine("Tower Created");

            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                line = line.Trim();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        line = line.Trim();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            line = line.Trim();
                            if (line == "<RaceName>") //Check to see if we're looking at a <RaceName> tag
                            {
                                line = stream.ReadLine();
                                line = line.Trim();
                                if (line == raceNameIN) //Check to see if it's the race we're looking for
                                {
                                    //Good now we can grab data!
                                    while (line != "</Race>") //Check to make sure we're still in the same race tag
                                    {
                                        line = stream.ReadLine();
                                        line = line.Trim();
                                        if (line == "<Towers>") //We're now grabbing all the towers for the race
                                        {
                                            while (line != "</Towers>") //Check to make sure we're still in the same towers tag
                                            {
                                                line = stream.ReadLine();
                                                line = line.Trim();
                                                if (line == "<TowerGroup>") //Check to see if we're looking at group info
                                                {
                                                    line = stream.ReadLine();
                                                    line = line.Trim();
                                                    if (line == "<GroupName>")// We're now grabbing a tower group
                                                    {
                                                        line = stream.ReadLine();
                                                        line = line.Trim();
                                                        if (line == groupNameIN) //If it's the group we're looking for we're going to start grabbing all the towers for that group
                                                        {
                                                            while (line != "</TowerGroup>") //Check to make sure we're still in the same tower group
                                                            {
                                                                line = stream.ReadLine();
                                                                line = line.Trim();
                                                                if (line == "<Name>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    if (line == towerNameIN) //If it's the name of the tower we're looking for we'll grab all the tower data for it
                                                                    {
                                                                        Console.WriteLine(line);
                                                                        singleTowerData.name = line;
                                                                        while (line != "</TowerData>")
                                                                        {
                                                                            line = stream.ReadLine();
                                                                            line = line.Trim();
                                                                            if (line == "<slow>")
                                                                            {
                                                                                line = stream.ReadLine();
                                                                                line = line.Trim();
                                                                                Console.WriteLine(line);
                                                                                singleTowerData.Slow = double.Parse(line);
                                                                            }
                                                                            else if (line == "<cost>")
                                                                            {
                                                                                line = stream.ReadLine();
                                                                                line = line.Trim();
                                                                                Console.WriteLine(line);
                                                                                singleTowerData.cost = int.Parse(line);
                                                                            }
                                                                            else if (line == "<damage>")
                                                                            {
                                                                                line = stream.ReadLine();
                                                                                line = line.Trim();
                                                                                Console.WriteLine(line);
                                                                                singleTowerData.Damage = int.Parse(line);
                                                                            }
                                                                            else if (line == "<mDamage>")
                                                                            {
                                                                                line = stream.ReadLine();
                                                                                line = line.Trim();
                                                                                Console.WriteLine(line);
                                                                                singleTowerData.mDamage = int.Parse(line);
                                                                            }
                                                                            else if (line == "<rateOfFire>")
                                                                            {
                                                                                line = stream.ReadLine();
                                                                                line = line.Trim();
                                                                                Console.WriteLine(line);
                                                                                singleTowerData.RateOfFire = int.Parse(line);
                                                                            }
                                                                        }
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine("Tower Returned");
                                                                        return singleTowerData;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            stream.Close();
                                            stream.Dispose();

                                            return singleTowerData;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    //We're not in a proper raceData file...
                    stream.Close();
                    stream.Dispose();
                }
            }
            return singleTowerData;
        }

        public static List<List<CreepData>> getAllMinionData(string raceNameIN)
        {
            //^Are you fucking serious?
            List<List<CreepData>> totalCreepData = new List<List<CreepData>>();
            Console.WriteLine("List of CreepData created");

            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                line = line.Trim();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        line = line.Trim();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            line = line.Trim();
                            if (line == "<RaceName>") //Check to see if we're looking at a <RaceName> tag
                            {
                                line = stream.ReadLine();
                                line = line.Trim();
                                if (line == raceNameIN) //Check to see if it's the race we're looking for
                                {
                                    //Good now we can grab data!
                                    while (line != "</Race>") //Check to make sure we're still in the same race tag
                                    {
                                        line = stream.ReadLine();
                                        line = line.Trim();
                                        if (line == "<Minions>") //We're now grabbing all the towers for the race
                                        {

                                            while (line != "</Minions>") //Check to make sure we're still in the same towers tag
                                            {
                                                line = stream.ReadLine();
                                                line = line.Trim();
                                                if (line == "<MinionGroup>") //Check to see if we're looking at group info
                                                {
                                                    List<CreepData> groupCreepData = new List<CreepData>();
                                                    Console.WriteLine("Group Created");
                                                    string creepGroup;
                                                    creepGroup = "Temp";
                                                    while (line != "</MinionGroup>") //Check to make sure we're still in the same tower group
                                                    {
                                                        line = stream.ReadLine();
                                                        line = line.Trim();
                                                        if (line == "<GroupName>")
                                                        {
                                                            line = stream.ReadLine();
                                                            line = line.Trim();
                                                            creepGroup = line;
                                                        }
                                                        if (line == "<MinionData>")
                                                        {
                                                            CreepData currentCreep = new CreepData();
                                                            Console.WriteLine("Creep Created");
                                                            while (line != "</MinionData>")
                                                            {
                                                                line = stream.ReadLine();
                                                                line = line.Trim();
                                                                if (line == "<Name>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.name = line;
                                                                }
                                                                else if (line == "<speed>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.Speed = double.Parse(line);
                                                                }
                                                                else if (line == "<cost>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.cost = int.Parse(line);
                                                                }
                                                                else if (line == "<damage>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.Damage = int.Parse(line);
                                                                }
                                                                else if (line == "<mDamage>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    Console.WriteLine(line);
                                                                    currentCreep.mDamage = int.Parse(line);
                                                                }
                                                            }
                                                            currentCreep.group = creepGroup;
                                                            groupCreepData.Add(currentCreep);
                                                            Console.WriteLine("Creep Added to Group");
                                                        }
                                                    }
                                                    Console.WriteLine("Group added to List of CreepData");
                                                    totalCreepData.Add(groupCreepData);
                                                }
                                            }
                                            stream.Close();
                                            stream.Dispose();
                                            Console.WriteLine("List of CreepData returned");
                                            return totalCreepData;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    //We're not in a proper raceData file...
                    stream.Close();
                    stream.Dispose();
                }
            }
            return totalCreepData;
        }


        public static List<CreepData> getGroupMinionData(string raceNameIN, string groupNameIN)
        {
            List<CreepData> groupCreepData = new List<CreepData>();
            Console.WriteLine("List of CreepData created");

            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                line = line.Trim();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        line = line.Trim();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            line = line.Trim();
                            if (line == "<RaceName>") //Check to see if we're looking at a <RaceName> tag
                            {
                                line = stream.ReadLine();
                                line = line.Trim();
                                if (line == raceNameIN) //Check to see if it's the race we're looking for
                                {
                                    //Good now we can grab data!
                                    while (line != "</Race>") //Check to make sure we're still in the same race tag
                                    {
                                        line = stream.ReadLine();
                                        line = line.Trim();
                                        if (line == "<Minions>") //We're now grabbing all the towers for the race
                                        {
                                            while (line != "</Minions>") //Check to make sure we're still in the same towers tag
                                            {
                                                line = stream.ReadLine();
                                                line = line.Trim();
                                                if (line == "<MinionGroup>") //Check to see if we're looking at group info
                                                {
                                                    line = stream.ReadLine();
                                                    line = line.Trim();
                                                    if (line == "<GroupName>")// We're now grabbing a tower group
                                                    {
                                                        line = stream.ReadLine();
                                                        line = line.Trim();
                                                        if (line == groupNameIN) //If it's the group we're looking for we're going to start grabbing all the towers for that group
                                                        {
                                                            while (line != "</MinionGroup>") //Check to make sure we're still in the same tower group
                                                            {
                                                                CreepData currentCreep = new CreepData();
                                                                Console.WriteLine("Creep Created");
                                                                while (line != "</MinionData>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    if (line == "<Name>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.name = line;
                                                                    }
                                                                    else if (line == "<speed>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.Speed = double.Parse(line);
                                                                    }
                                                                    else if (line == "<cost>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.cost = int.Parse(line);
                                                                    }
                                                                    else if (line == "<damage>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.Damage = int.Parse(line);
                                                                    }
                                                                    else if (line == "<mDamage>")
                                                                    {
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine(line);
                                                                        currentCreep.mDamage = int.Parse(line);
                                                                    }
                                                                }
                                                                currentCreep.group = groupNameIN;
                                                                groupCreepData.Add(currentCreep);
                                                                Console.WriteLine("Tower Added to Group");
                                                                line = stream.ReadLine();
                                                                line = line.Trim();
                                                            }
                                                        }
                                                    }
                                                    //Console.WriteLine("Group added to List of MinionData");
                                                    //groupCreepData.Add(groupCreepData);
                                                }
                                            }
                                            stream.Close();
                                            stream.Dispose();
                                            Console.WriteLine("List of MinionData returned");
                                            return groupCreepData;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    //We're not in a proper raceData file...
                    stream.Close();
                    stream.Dispose();
                }
            }
            return groupCreepData;
        }

        public static CreepData getSingleMinionData(string raceNameIN, string groupNameIN, string minionNameIN)
        {
            CreepData singleCreepData = new CreepData();
            Console.WriteLine("Creep Created");

            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                line = line.Trim();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        line = line.Trim();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            line = line.Trim();
                            if (line == "<RaceName>") //Check to see if we're looking at a <RaceName> tag
                            {
                                line = stream.ReadLine();
                                line = line.Trim();
                                if (line == raceNameIN) //Check to see if it's the race we're looking for
                                {
                                    //Good now we can grab data!
                                    while (line != "</Race>") //Check to make sure we're still in the same race tag
                                    {
                                        line = stream.ReadLine();
                                        line = line.Trim();
                                        if (line == "<Minions>") //We're now grabbing all the towers for the race
                                        {
                                            while (line != "</Minions>") //Check to make sure we're still in the same towers tag
                                            {
                                                line = stream.ReadLine();
                                                line = line.Trim();
                                                if (line == "<MinionGroup>") //Check to see if we're looking at group info
                                                {
                                                    line = stream.ReadLine();
                                                    line = line.Trim();
                                                    if (line == "<GroupName>")// We're now grabbing a tower group
                                                    {
                                                        line = stream.ReadLine();
                                                        line = line.Trim();
                                                        if (line == groupNameIN) //If it's the group we're looking for we're going to start grabbing all the towers for that group
                                                        {
                                                            while (line != "</MinionGroup>") //Check to make sure we're still in the same tower group
                                                            {
                                                                line = stream.ReadLine();
                                                                line = line.Trim();
                                                                if (line == "<Name>")
                                                                {
                                                                    line = stream.ReadLine();
                                                                    line = line.Trim();
                                                                    if (line == minionNameIN) //If it's the name of the tower we're looking for we'll grab all the tower data for it
                                                                    {
                                                                        Console.WriteLine(line);
                                                                        singleCreepData.name = line;
                                                                        while (line != "</MinionData>")
                                                                        {
                                                                            line = stream.ReadLine();
                                                                            line = line.Trim();
                                                                            if (line == "<speed>")
                                                                            {
                                                                                line = stream.ReadLine();
                                                                                line = line.Trim();
                                                                                Console.WriteLine(line);
                                                                                singleCreepData.Speed = double.Parse(line);
                                                                            }
                                                                            else if (line == "<cost>")
                                                                            {
                                                                                line = stream.ReadLine();
                                                                                line = line.Trim();
                                                                                Console.WriteLine(line);
                                                                                singleCreepData.cost = int.Parse(line);
                                                                            }
                                                                            else if (line == "<damage>")
                                                                            {
                                                                                line = stream.ReadLine();
                                                                                line = line.Trim();
                                                                                Console.WriteLine(line);
                                                                                singleCreepData.Damage = int.Parse(line);
                                                                            }
                                                                            else if (line == "<mDamage>")
                                                                            {
                                                                                line = stream.ReadLine();
                                                                                line = line.Trim();
                                                                                Console.WriteLine(line);
                                                                                singleCreepData.mDamage = int.Parse(line);
                                                                            }
                                                                        }
                                                                        line = stream.ReadLine();
                                                                        line = line.Trim();
                                                                        Console.WriteLine("Creep Returned");
                                                                        return singleCreepData;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            stream.Close();
                                            stream.Dispose();

                                            return singleCreepData;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    //We're not in a proper raceData file...
                    stream.Close();
                    stream.Dispose();
                }
            }
            return singleCreepData;
        }

        /* Get Spell Data Here */

        public static byte[,] getMapData(string raceNameIN)
        {
            byte[,] mapData = { {0} };
            //Schuyer was here, and he totaly fondeled this code >:D
            //He also noticed how it wasnt RaceData.CCD
            //But he forgives you for that.
            Console.WriteLine("Map Created");

            using (StreamReader stream = new StreamReader("RaceData.txt"))
            {
                string line;
                line = stream.ReadLine();
                line = line.Trim();
                if (line == "<BEGIN>") //If we see this -- we're DEFINETLY in the proper file
                {
                    while (line != "<END>") //While we're not at the end of the file
                    {
                        line = stream.ReadLine();
                        line = line.Trim();
                        if (line == "<Race>") //Check to see if we're looking at a <Race> tag
                        {
                            line = stream.ReadLine();
                            line = line.Trim();
                            if (line == "<RaceName>") //Check to see if we're looking at a <RaceName> tag
                            {
                                line = stream.ReadLine();
                                line = line.Trim();
                                if (line == raceNameIN) //Check to see if it's the race we're looking for
                                {
                                    //Good now we can grab data!
                                    while (line != "</Race>") //Check to make sure we're still in the same race tag
                                    {
                                        line = stream.ReadLine();
                                        line = line.Trim();
                                        if (line == "<MapData>") //We're now grabbing all the towers for the race
                                        {
                                            while (line != "</MapData>") //Check to make sure we're still in the same towers tag
                                            {
                                                line = stream.ReadLine();
                                                line = line.Trim();
                                                if (line == "<ARRAY>") //Check to see if we're looking at an ARRAY group
                                                {
                                                    Console.WriteLine("In MapData");
                                                    while (line != "</ARRAY>")
                                                    {
                                                        line = stream.ReadLine();
                                                        line = line.Trim();
                                                        if (line == "</ARRAY>")
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            for (int x = 0; x < line.Length; x++)
                                                            {
                                                                int temp = int.Parse("" + line[x]);//Grab the line and make it a numbaaaahhhh
                                                                Console.WriteLine(temp);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            stream.Close();
                                            stream.Dispose();

                                            return mapData;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    //We're not in a proper raceData file...
                    stream.Close();
                    stream.Dispose();
                }
            }
            return mapData;
        }
    }
}
