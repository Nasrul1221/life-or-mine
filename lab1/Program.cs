using System;

namespace Project
{
    class Game
    {
        public static int size, userHealth;

        // main method
        static void Main(string[] args)
        {
            Console.WriteLine("------- Game -------");
            Console.WriteLine("W - Up, S - Down, A - Left, D - Right");
            Console.WriteLine("Press any key to start");
            Console.ReadKey(true); Console.Clear();

            do
            {
                Console.Write("Enter the field size: ");
            } while (!int.TryParse(Console.ReadLine(), out size) || (size <= 3 || size > 20));


            do
            {
                Console.Write("Enter the number of health: ");
            } while (!int.TryParse(Console.ReadLine(), out userHealth) || userHealth < 1);


            Console.Clear();
            Field.FieldCreate();
            Console.WriteLine("Press any key to start");
            Player.movePlayer();

        }
    }

    class Field
    {
        public static char[,] field;

        public static void FieldCreate() // visualize field
        {
            field = new char[Game.size, Game.size];

            for (int i = 0; i < Game.size; i++)
            {
                for (int j = 0; j < Game.size; j++)
                {
                    field[i, j] = '.';
                }
            }

            Player.initializePlayerAndCell(field);

            Random rand = new Random();
            int totalItems = (Game.size * Game.size) / 2;
            int mines = totalItems / 2; int health = totalItems - mines; // mines and health

            // mines
            for (int i = 0; i < mines; i++)
            {
                int x, y;

                do
                {
                    x = rand.Next(Game.size);
                    y = rand.Next(Game.size);
                } while ((field[x, y] != '.'));

                field[x, y] = 'M';
            }

            //health
            for (int i = 0; i < health; i++)
            {
                int x, y;

                do
                {
                    x = rand.Next(Game.size);
                    y = rand.Next(Game.size);
                } while ((field[x, y] != '.'));

                field[x, y] = 'H';
            }
            printField();
        }
        public static void printField()
        {
            for (int i = 0; i < Game.size; i++)
            {
                for (int j = 0; j < Game.size; j++)
                {
                    if (field[i, j] == 'W')
                    {
                        Console.ForegroundColor = ConsoleColor.Green; Console.Write(field[i, j]);
                        Console.ResetColor();
                    }
                    else if (field[i, j] == 'P')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue; Console.Write(field[i, j]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(field[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }
    }

        class Player
    {
        public static int x = 0, y = 0;
        public static int currentHealth = Game.userHealth;

        public static void initializePlayerAndCell(char[,] field) // player initialization
        {
            field[0, 0] = 'P';
            field[Game.size -1, Game.size - 1] = 'W';
        }

        public static void movePlayer() // player movement
        {
            int newX = 0, newY = 0;

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                newX = x; newY = y;

                switch (keyInfo.Key) // movement keys
                {
                    case ConsoleKey.W: newX--; break;
                    case ConsoleKey.S: newX++; break;
                    case ConsoleKey.A: newY--; break;
                    case ConsoleKey.D: newY++; break;
                    case ConsoleKey.Escape: return;
                }

                if ((newX == Game.size - 1) && (newY == Game.size - 1)) // win condition
                {
                    Field.field[x, y] = '.';
                    x = newX; y = newY;
                    Field.field[x, y] = 'P';
                    Console.Clear(); 
                    Field.printField();

                    Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("You won");
                    Console.ResetColor(); // change and reset color
                    break;
                }

                if (newX < 0 || newX >= Game.size || newY < 0 || newY >= Game.size) // out of bounds
                {
                    continue;
                }

                if (Field.field[newX, newY] == 'M') // mine hit
                {
                    currentHealth--;

                    if (currentHealth == 0)
                    {
                        Field.field[x, y] = '.';
                        x = newX; y = newY;
                        Field.field[x, y] = 'P';
                        Console.Clear();
                        Field.printField();

                        Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Game Over!");
                        Console.ResetColor(); // change and reset color
                        break;
                    }
                }
                else if (Field.field[newX, newY] == 'H') // health pickup
                {
                    if (currentHealth < Game.userHealth)
                    {
                        currentHealth++;

                    }
                }

                Field.field[x, y] = '.'; // player movement refresh
                x = newX;
                y = newY;
                Field.field[x, y] = 'P';

                Console.Clear();
                Field.printField();

                Console.WriteLine("Lives left: " + Player.currentHealth);
            }
        }
    }

}
