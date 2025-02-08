using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War_Ships_work
{
    internal class Program
    {
        public static string name = "";

        public static int totalGuesses = 0;
        public static int validGuesses = 0;




        public struct ShipType
        {
            public string Name;
            public int Size;
        }

        const string TrainingGame = "Training.txt";

        private static void GetRowColumn(ref int Row, ref int Column)
        {
            Console.WriteLine();
            bool isvalid = false;
            int chosenColumn = 0;
            

            while (!isvalid)
            {
                Console.Write("Please enter column: ");
                Column = int.Parse(Console.ReadLine());
                try
                {
                    chosenColumn = int.Parse(Column);
                    if (Column >= 0 && Column <= 9)
                    {
                        isvalid = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input, please try again.");
                    continue;
                }
                
            }
            isvalid = true;
            
            while (!isvalid)
            {
                Console.Write("Please enter row: ");
                Row = Convert.ToInt32(Console.ReadLine());
                if (Row >= 0 && Row <= 9)
                {
                    isvalid = true;
                }
            }
            Console.WriteLine();
        }

        private static void MakePlayerMove(ref char[,] Board, ref ShipType[] Ships)
        {
            int Row = 0;
            int Column = 0;
            GetRowColumn(ref Row, ref Column);
            if (Board[Row, Column] == 'm' || Board[Row, Column] == 'h')
            {
                Console.WriteLine("Sorry, you have already shot at the square (" + Column + "," + Row + "). Please try again.");
                totalGuesses++;
                Console.WriteLine("You have had " + totalGuesses + " guesses and " + validGuesses + " valid Guesses");

            }
            else if (Board[Row, Column] == '-')
            {
                Console.WriteLine("Sorry, (" + Column + "," + Row + ") is a miss.");
                totalGuesses++;
                Console.WriteLine("You have had " + totalGuesses + " guesses and " + validGuesses + " valid Guesses");

                Board[Row, Column] = 'm';
            }
            else
            {
                Console.WriteLine("Hit at (" + Column + "," + Row + ").");
                totalGuesses++;
                validGuesses++;
                Board[Row, Column] = 'h';
            }
        }

        private static void SetUpBoard(ref char[,] Board)
        {
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {
                    Board[Row, Column] = '-';
                }
            }
        }

        private static void LoadGame(string TrainingGame, ref char[,] Board)
        {
            string Line = "";
            using (StreamReader BoardFile = new StreamReader(TrainingGame))
            {
                for (int Row = 0; Row < 10; Row++)
                {
                    Line = BoardFile.ReadLine();
                    for (int Column = 0; Column < 10; Column++)
                    {
                        Board[Row, Column] = Line[Column];
                    }
                }
            }
        }

        private static void PlaceRandomShips(ref char[,] Board, ShipType[] Ships)
        {
            Random RandomNumber = new Random();
            bool Valid;
            char Orientation = ' ';
            int Row = 0;
            int Column = 0;
            int HorV = 0;
            foreach (var Ship in Ships)
            {
                Valid = false;
                while (Valid == false)
                {
                    Row = RandomNumber.Next(0, 10);
                    Column = RandomNumber.Next(0, 10);
                    HorV = RandomNumber.Next(0, 2);
                    if (HorV == 0)
                    {
                        Orientation = 'v';
                    }
                    else
                    {
                        Orientation = 'h';
                    }
                    Valid = ValidateBoatPosition(Board, Ship, Row, Column, Orientation);
                }
                Console.WriteLine("Computer placing the " + Ship.Name + "with size: " + Ship.Size);
                PlaceShip(ref Board, Ship, Row, Column, Orientation);
            }
        }

        private static void PlaceShip(ref char[,] Board, ShipType Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    Board[Row + Scan, Column] = Ship.Name[0];
                }
            }
            else if (Orientation == 'h')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    Board[Row, Column + Scan] = Ship.Name[0];
                }
            }
        }

        private static bool ValidateBoatPosition(char[,] Board, ShipType Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v' && Row + Ship.Size > 10)
            {
                return false;
            }
            else if (Orientation == 'h' && Column + Ship.Size > 10)
            {
                return false;
            }
            else
            {
                if (Orientation == 'v')
                {
                    for (int Scan = 0; Scan < Ship.Size; Scan++)
                    {
                        if (Board[Row + Scan, Column] != '-')
                        {
                            return false;
                        }
                    }
                }
                else if (Orientation == 'h')
                {
                    for (int Scan = 0; Scan < Ship.Size; Scan++)
                    {
                        if (Board[Row, Column + Scan] != '-')
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static bool CheckWin(char[,] Board)
        {
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static void PrintBoard(char[,] Board)
        {
            Console.WriteLine();
            Console.WriteLine("The board looks like this: ");
            Console.WriteLine();
            Console.Write(" ");
            for (int Column = 0; Column < 10; Column++)
            {
                Console.Write(" " + Column + "  ");
            }
            Console.WriteLine();
            for (int Row = 0; Row < 10; Row++)
            {
                Console.Write(Row + " ");
                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == '-')
                    {
                        Console.Write(" ");
                    }
                    else if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(Board[Row, Column]);
                    }
                    if (Column != 9)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("");
            Console.WriteLine("1. Start new game");
            Console.WriteLine("2. Load training game");
            Console.WriteLine("9. Quit");
            Console.WriteLine();
        }

        private static int GetMainMenuChoice()
        {
            int Chosen;
            String Choice;
            
            while (true)
            {
                Console.Write("Please enter your choice: ");
                Choice = Console.ReadLine();

                try
                {
                    Chosen = int.Parse(Choice);
                    if (Chosen == 1 || Chosen == 2 || Chosen == 9)
                    {
                        break; 
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice! Please enter 1, 2, or 9.");
                    }
                    
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
                
            }
            


            return Chosen;
        }

        private static void PlayGame(ref char[,] Board, ref ShipType[] Ships)
        {
            bool GameWon = false;
            while (GameWon == false)
            {
                PrintBoard(Board);
                MakePlayerMove(ref Board, ref Ships);
                GameWon = CheckWin(Board);
                if (GameWon == true)
                {
                    Console.WriteLine("All ships sunk!");
                    Console.WriteLine(name + " you scored a total of: " + totalGuesses);
                    Console.WriteLine();
                }
            }
        }

        private static void SetUpShips(ref ShipType[] Ships)
        {
            Ships[0].Name = ("Aircraft Carrier");
            Ships[0].Size = (5);
            Ships[1].Name = ("Battleship");
            Ships[1].Size = (4);
            Ships[2].Name = ("Submarine");
            Ships[2].Size = (3);
            Ships[3].Name = ("Destroyer");
            Ships[3].Size = (3);
            Ships[4].Name = ("Patrol Boat");
            Ships[4].Size = (2);

        }

        private static void AskName()
        {
            Console.WriteLine("What is your name? ");
            name = Console.ReadLine();
        }

        static void Main(string[] args)
        {
            ShipType[] Ships = new ShipType[5];
            char[,] Board = new char[10, 10];
            int MenuOption = 0;
            while (MenuOption != 9)
            {
                SetUpBoard(ref Board);
                SetUpShips(ref Ships);
                AskName();

                DisplayMenu();
                MenuOption = GetMainMenuChoice();
                if (MenuOption == 1)
                {
                    PlaceRandomShips(ref Board, Ships);
                    PlayGame(ref Board, ref Ships);
                }
                if (MenuOption == 2)
                {
                    LoadGame(TrainingGame, ref Board);
                    PlayGame(ref Board, ref Ships);
                }
            }
        }
    }
}
