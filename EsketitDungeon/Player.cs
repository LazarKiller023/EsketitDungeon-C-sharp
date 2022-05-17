using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsketitDungeon
{
    [Serializable]  
    public class Player
    {
        //Random rand = new Random();
        
        public string name;
        public int id;
        public int coins = 0;
        public int level = 1;
        public int xp = 0;
        public int health = 10;
        public int damage = 1;
        public int armorValue = 0;
        public int potion = 5;
        public int weaponValue = 1;

        public int mods = 0;  

        public enum PlayerClass {Mage, Archer, Warrior}; //player classes
        public PlayerClass currentClass = PlayerClass.Warrior;

        public int GetHealth()
        {
            int upper = (2 * mods + 7);
            int lower = (mods + 2);
            return Program.rnd.Next(lower, upper);
        }
        public int GetPower()
        {
            int upper = (2 * mods + 2);
            int lower = (mods + 1);
            return Program.rnd.Next(lower, upper);
        }
        public int GetCoins()
        {
            int upper = (15 * mods + 50);
            int lower = (10 * mods + 10);
            return Program.rnd.Next(lower, upper);
        }

        public int GetXP()
        {
            int upper = (20 * mods + 50);
            int lower = (15 * mods + 10);
            return Program.rnd.Next(lower, upper);
        }

        public int GetLevelUpValue()
        {
            return 100 * level + 400; 
        }
        public bool CanLevelUp()
        {
            if (xp >= GetLevelUpValue())
                return true;
            else
                return false;
        }
        public void LevelUp()
        {
            while(CanLevelUp())
            {
                xp -= GetLevelUpValue();
                level++; 
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen; //text color
            Program.Print("Good job! Your level increased to: " + level, 20);
            Console.ResetColor(); 
        }
    }
}
