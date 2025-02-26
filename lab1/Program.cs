using System;

/* Варіант 23.
Створити консольний застосунок, що моделює таку гру. На квадратному полі випадковим чином розміщено міни та аптечки. 
Кількість мін та аптечок однакова, вона генерується випадковим чином так, що разом вони займають не більше половини поля. 
Гравець має певну кількість життів. Натрапивши на міну, він втрачає одне життя, знайшовши аптечку відновлює одне життя. 
Не можна мати від'ємну або більшу за задану кількість життів. Коли кількість життів дорівнює нулю гра завершується поразкою.
Розмір поля та кількість життів задаються користувачем. Позначити міни та аптечки можна будь-якими різними символами, 
необхідно показати їх на полі, коли гравець наткнеться на предмет. Залишок життів має відображатися на кожному кроці.
На початку гри гравець знаходиться в лівому верхньому куті поля, для перемоги йому необхідно дістатися нижнього правого кута поля. 
Гравець може переміщатися по горизонталі або по вертикалі. Гра завершується перемогою, якщо гравець дістався цілі та не втратив усі життя.
*/

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
                } while (field[x, y] != '.');

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
                    char symbol = Field.field[i, j];

                    if (symbol == 'P') Console.ForegroundColor = ConsoleColor.Blue;
                    else if (symbol == 'M') Console.ForegroundColor = ConsoleColor.Red;
                    else if (symbol == 'H') Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ResetColor();

                    Console.Write($"{symbol} ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }

    class Player
    {
        static int x = 0, y = 0;
        public static int currentHealth = Game.userHealth;

        public static void initializePlayerAndCell(char[,] field) // player initialization
        {
            field[0, 0] = 'P';
            field[Game.size -1, Game.size - 1] = 'W';
        }

        static void updatePlayerPosition(char character, int newX, int newY)
        {
            Field.field[x, y] = '.';
            x = newX; y = newY;
            Field.field[x, y] = character;
            Console.Clear();
            Field.printField();
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
                    updatePlayerPosition('P', newX, newY);

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
                        updatePlayerPosition('P', newX, newY);

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
