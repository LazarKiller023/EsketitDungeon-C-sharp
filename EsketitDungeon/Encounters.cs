using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsketitDungeon
{
    public class Encounters
    {
        static Random rand = new Random();
        //Encounter Generic

        
        //Encounters
        public static void FirstEncounter()
        {
            Console.WriteLine("You slam open the door and quickly grab a rusty sword, while rushing towards your captor");
            Console.WriteLine("He quickly turns...");
            Console.ReadKey();
            Combat(false, "Human Foe", 1, 4);
        } 
        public static void BasicFightEncounter()
        {
            Console.Clear();
            Console.WriteLine("You turn the corner and there you see a fearsome beast...");
            Console.ReadKey();
            Combat(true, "", 0, 0);
        }
        public static void WizardEncounter()
        {
            Console.Clear();
            Console.WriteLine("The old rusty door slowly creaks open as you venture into the dark room. You see a tall man with a ");
            Console.WriteLine("long beard looking at a large tome.");
            Console.ReadKey();
            Combat(false, "Dark Wizard", 10, 3);
        }
        public static void PuzzleOneCounter()
        {
            Console.Clear();
            Console.WriteLine("While you were venturing down a long hall, you noticed that the floor is covered in sophisticated runes."); 
            List<char> chars = new char[]{ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }.ToList();
            List<int> positions = new List<int>();
            char c = chars[Program.rnd.Next(0, 10)]; //hallway graphics
            chars.Remove(c);
            for (int y = 0; y < 4; y++)
            {
                int pos = Program.rnd.Next(0, 4);
                positions.Add(pos);
                for (int x = 0; x < 4; x++)
                {
                       if (x == pos)
                           Console.Write(c);
                       else
                           Console.Write(chars[Program.rnd.Next(0, 8)]);
                }
                Console.Write("\n");

            }
            Console.WriteLine("Choose your path: (Type the position of the rune you want to stand on, not the number: left to right)");
            Console.ReadKey();
            for (int i = 0; i < 4; i++)
            {
                while(true)
                {
                    if(int.TryParse(Console.ReadLine(), out int input) && input < 5 && input > 0) //input from 1-4
                    {
                        if (positions[i] == input -1)
                            break;
                        else
                        { 
                            Console.WriteLine("Out of nowhere, poisonous arrows fly out of the walls! You take 2 damage.");
                            Program.currentPlayer.health -= 2;
                            if (Program.currentPlayer.health <= 0)
                            {
                                //death code
                                Console.WriteLine("You start to feel nauseous. Poison from the arrows slowly kills you.");
                                Console.ReadKey();
                                System.Environment.Exit(0);
                            }
                            break;
                        }
                    }
                    else
                        Console.WriteLine("Invalid Input: Whole numbers 1-4 only");
                    
                }
            }
            Console.WriteLine("You succeeded in crossing the hallway!");
            Console.ReadKey();

        }
        
        //Encounter Tools
        public static void RandomEncounter() //list of encounters
        {
            switch(Program.rnd.Next(0, 3))
            {
                case 0:
                    BasicFightEncounter();
                    break;
                case 1:
                    WizardEncounter();
                    break;
                case 2:
                    PuzzleOneCounter();
                    break;
            }
        }
        public static void Combat(bool random, string name, int power, int health)
        {
            string n = "";
            int p = 0;
            int h = 0;
            if(random)
            {
                n = GetName();
                p = Program.currentPlayer.GetPower();
                h = Program.currentPlayer.GetHealth();
            }
            else
            {
                n = name;
                p = power;
                h = health;
            }
            while(h > 0)
            {
                Console.Clear();
                Console.WriteLine(n);
                Console.WriteLine(p + "/" + h);
                Console.WriteLine("========================");
                Console.WriteLine("| (A)ttack (D)efend |");
                Console.WriteLine("| (R)un (H)eal |");
                Console.WriteLine("========================");
                Console.WriteLine(" Potions: "+Program.currentPlayer.potion+" Health: "+Program.currentPlayer.health);
                string input = Console.ReadLine();
                if (input.ToLower() == "a" || input.ToLower() == "attack")
                {
                    //Attack
                    Console.WriteLine("Without fear, you strike your enemy with your sword! After you struck, the "+n+" strikes you back!");
                    int damage = p - Program.currentPlayer.armorValue;
                    if(damage < 0)
                        damage = 0;
                    int attack = Program.rnd.Next(0, Program.currentPlayer.weaponValue) + Program.rnd.Next(1, 4) + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior)? 2:0);
                    Console.WriteLine("You lose " + damage + " health, and you deal " + attack + " damage");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                }
                else if (input.ToLower() == "d" || input.ToLower() == "defend")
                {
                    //Defend
                    Console.WriteLine("As the " + n + " prepares to strike, you take a position to defend yourself with your sword");
                    int damage = (p/4) - Program.currentPlayer.armorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = Program.rnd.Next(0, Program.currentPlayer.weaponValue) / 2;
                    Console.WriteLine("You lose " + damage + " health, and you deal " + attack + " damage");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                }
                else if (input.ToLower() == "r" || input.ToLower() == "run")
                {
                    //Run
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Archer && Program.rnd.Next(0, 2) == 0) //if the class is not an archer and random number = 0, fails, goes to else
                    {
                        Console.WriteLine("As you run away from the " + n + ", its strike hits you in the back, smashing you onto the ground.");
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("You lose " + damage + " health and are unable to espace.");
                        Program.currentPlayer.health -= damage;
                    }
                    else
                    {
                        Console.WriteLine("You've sucessfully evaded the " +n+ "");
                        Console.ReadKey();
                        Shop.LoadShop(Program.currentPlayer); 

                    }
                }
                else if (input.ToLower() == "h" || input.ToLower() == "heal")
                {
                    //Heal
                    if (Program.currentPlayer.potion == 0)
                    {
                        Console.WriteLine("You checked your bag, and found no potions that you can use!");
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("The " + n + " strikes you with a mighty blow and you lose " + damage + " health!");
                    }
                    else
                    {
                        Console.WriteLine("You checked your bag and found a potion that you can use! You drink the potion in a hurry.");
                        int potionValue = 5 + ((Program.currentPlayer.currentClass == Player.PlayerClass.Mage)? + 3:0);
                        Console.WriteLine("You gain " + potionValue + " health");
                        Program.currentPlayer.health += potionValue;
                        Program.currentPlayer.potion--;
                        Console.WriteLine("As you were busy, the "+ n + " ran quickly towards you and struck.");
                        int damage = (p / 2) - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("You lose " + damage + " health.");
                    }
                }
                if(Program.currentPlayer.health <= 0)
                {
                    //death code
                    Console.WriteLine("As the " + n + " stands fearless and strikes you. You die from the injuries caused by the " + n);
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                Console.ReadKey();

            }
            int c = Program.currentPlayer.GetCoins();
            int x = Program.currentPlayer.GetXP();
            Console.WriteLine("You've successfully defeated the " + n + ", its body dissolves into "+c+" gold coins! You have gained " +x+" XP");
            Program.currentPlayer.coins += c;
            Program.currentPlayer.xp += x;

            if (Program.currentPlayer.CanLevelUp())
                Program.currentPlayer.LevelUp();
            
            Console.ReadKey();
        }

        public static string GetName()
        {
            switch (Program.rnd.Next(0, 11))
            {
                case 0:
                    return "Undead Warrior";
                case 1:
                    return "Demon";
                case 2:
                    return "Skeleton";
                case 3:
                    return "Marauder";
                case 4:
                    return "Toxic Slime";
                case 5:
                    return "Skull Raider";
                case 6:
                    return "Dark Magician";
                case 7:
                    return "Wretch";
                case 8:
                    return "Bone Stealer";
                case 9:
                    return "Skin Stealer";
                case 10:
                    return "Soul Demolisher";
            }
            return "Human Foe"; 
        }
    }
}
