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

namespace Engine
{
    public struct TowerData
    {
        public int Damage;
        public int mDamage;
        public int Poison;
        public int PoisonDuration;
        public int PoisonTPS;//Ticks Per Second
        public int Slow;
        public int DefenseAdjust;//Plus Or Minus can be detect with IF for Enemy lower or Friedly Raise
        public int mDefenseAdjust;//What he said above me.
        public int ChainCount;//Ammount it chains to
        public int MoneyGained;
        public int RateOfFire;//In MilliSeconds
    }
    public class Tower
    {
    }
}
