using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace _131Final.Engine
{
    class Menu
    {
        private int myScreen;
        private static List<Menu> instances = new List<Menu>();
        private PlayerData myPlayerData;
        private string myData;

        public int Screen
        {
            get { return myScreen; }
        }
        public string Data
        {
            get { return myData; }
        }

        /// <summary>
        /// Every time a menu is created, it checks to see if a menu of that type already exists. If so, it will delete both.
        /// 
        /// </summary>
        /// <param name="Screen">Tells it which screen it is going to draw on.</param>
        /// <param name="Data">Tower, Minion, Menu, TowerGroup, MinionGroup</param>
        /// <returns></returns>
        public Menu(int Screen, string Data, PlayerData PD)
        {
            myPlayerData = PD;
            for(int i = 0; i < instances.Count; i++ )
            {
                if(Data == instances[i].Data)
                {
                    instances.Remove(instances[i]);
                    return;
                }
            }
            instances.Add(this);
        }
    }
}
