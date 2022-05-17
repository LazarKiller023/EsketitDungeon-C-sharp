using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


namespace EsketitDungeon
{
    public class Program
    {
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;
        public static Random rnd = new Random();

        static void Main(string[] args)
        {
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }
            currentPlayer = Load(out bool newP);
            if (newP)
                Encounters.FirstEncounter();
            while (mainLoop)
            {
                Encounters.RandomEncounter();
            }

        }

        static Player NewStart(int i)
        {
            Console.Clear();
            Player p = new Player();
            Console.WriteLine("Esketit Dungeon");
            Console.WriteLine("Input name:");
            p.name = Console.ReadLine();
            Print("==========================================", 10);
            Print("Class: |  Mage  |  Archer  |  Warrior  |", 10);
            Print("==========================================", 10);
            bool flag = false;
            while(flag == false)
            {
                flag = true;
                string input = Console.ReadLine().ToLower();
                if (input == "mage")
                    p.currentClass = Player.PlayerClass.Mage;
                else if (input == "archer")
                    p.currentClass = Player.PlayerClass.Archer;
                else if (input == "warrior")
                    p.currentClass = Player.PlayerClass.Warrior;
                else
                {
                    Console.WriteLine("Invalid class, chosee an existing class!");
                    flag = false;
                }
            }
            p.id = i;
            Console.Clear();
            Console.WriteLine("You have finally woken up, in a freezing dark room. You lost all your memory,");
            if (p.name == "")
                Console.WriteLine("You don't even remember your name...");
            else
                Console.WriteLine("You remember your name: " + p.name);
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You move around in freezing room enveloped by darkness, until you find a door handle. You feel some resistance as");
            Console.WriteLine("you turn the handle, but the rusty lock breaks with little effort. You see the person who captured you");
            Console.WriteLine("standing with his back right in front of you.");
            return p;
        }

        public static void Quit()
        {
            Save();
            Environment.Exit(0);
        }

        public static void Save()
        {
            BinaryFormatter binForm = new BinaryFormatter();
            string path = "saves/" + currentPlayer.id.ToString() + ".level";
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            binForm.Serialize(file, currentPlayer);
            file.Close();
        }
        public static Player Load(out bool newP)
        {
            newP = false;
            Console.Clear();
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = new List<Player>();
            int idCount = 0;

            BinaryFormatter binForm = new BinaryFormatter();
            foreach (string p in paths)
            {
                FileStream file = File.Open(p, FileMode.Open);
                Player player = (Player)binForm.Deserialize(file);
                file.Close();
                players.Add(player);

            }

            idCount = players.Count;
            
            while(true)
            {
                Console.Clear();
                Print("Choose your save:",60);

                foreach (Player p in players)
                {
                    Console.WriteLine(p.id + ": " + p.name);
                }
                
                Print("Input your player name or id (id:# or playername) Or 'create' will start a new save.");
                string[] data = Console.ReadLine().Split(':');

                try
                {
                    if (data[0] == "id")
                    {
                        if (int.TryParse(data[1], out int id))
                        {
                            foreach (Player player in players)
                            {
                                if(player.id == id)
                                {
                                    return player;  
                                }
                            }
                            Console.WriteLine("There is no save with that id!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("id can only be a number! Press a key to continue.");
                            Console.ReadKey();
                        }
                    }
                    else if (data[0] == "create")
                    {
                        Player newPlayer = NewStart(idCount);
                        newP = true;
                        return newPlayer;
                       
                    }
                    else
                    {
                        foreach (Player player in players)
                        {
                            if(player.name == data[0])
                            {
                                return player;
                            }
                        }
                        Console.WriteLine("There is no save with that id!");
                        Console.ReadKey();
                    }
                }
                catch(IndexOutOfRangeException)
                {
                    Console.WriteLine("id can only be a number! Press a key to continue.");
                    Console.ReadKey();
                }
            }

        }
        public static void Print(string text, int speed = 40)
        {       
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
        }

        public static void ProgressBar(string fillerChar, string backgroundChar, decimal value, int size) //progress bar
        {
            int dif = (int)(value * size); //differentiator value
            for (int i = 0; i < size; i++)
            {
                if (i < dif)
                    Console.Write(fillerChar);
                else
                    Console.Write(backgroundChar);
            }
        }

    }
}
