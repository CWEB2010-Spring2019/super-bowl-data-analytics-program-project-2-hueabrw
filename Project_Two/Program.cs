using System;
using System.Collections.Generic;
using System.IO;

namespace Project_Two
{
    class Program
    {
        static void Main(string[] args)
        {
            //this is the list that the csv file reads into (line by line)
            List<string> rows = new List<string>();
            using (StreamReader reader = new StreamReader(@"..\..\..\Super_Bowl_Project.csv")) {
                while (!reader.EndOfStream)
                {
                    reader.ReadLine();
                    rows.Add(reader.ReadLine());
                }
            }
            //sends each line from the csv file into a list of new superbowl objects.
            List<Superbowl> superbowls = new List<Superbowl>();
            foreach (string row in rows)
            {
                superbowls.Add(new Superbowl(row.Split(','))); //passed through as an array of strings
            }

            //creates a new query object passing the superbowl objects through
            SuperbowlQuerries queries = new SuperbowlQuerries(superbowls);

            //displays the UI
            DisplayPrompt();

            //ends program
            Console.Clear();







            //=======METHODS=======//

            //Displays a prompt
            void DisplayPrompt()
            {
                Console.SetWindowSize(70, 10);
                Console.SetCursorPosition(10, 2);
                Console.WriteLine("Welcome to the superbowl analysis file displayer");
                Console.SetCursorPosition(5, 3);
                Console.WriteLine("Would you like the information in a text file or HTML file?");

                interact();
            }

            //allows user to interactively select an option
            void interact()
            {
                //a value to help determine which option is highlighted.
                int highlighted = 0;
                ConsoleKey userKey = new ConsoleKey();

                //the interactive part of the code only runs until the user hits enter/selects an option
                while (userKey != ConsoleKey.Enter)
                {
                    DisplayOptions(highlighted);
                    userKey = Console.ReadKey(true).Key;
                    switch (userKey)
                    {
                        case ConsoleKey.A:
                            highlighted = 0;
                            break;
                        case ConsoleKey.D:
                            highlighted = 1;
                            break;
                        case ConsoleKey.RightArrow:
                            highlighted = 1;
                            break;
                        case ConsoleKey.LeftArrow:
                            highlighted = 0;
                            break;
                        default:
                            break;
                    }
                    
                }
                //once enter is pressed this means the user selected an option,
                //so this runs whichever option they highlighted
                if (highlighted == 1)
                {
                    queries.GenerateHTMLFile();
                }
                else
                {
                    queries.GenerateTextFile();
                }
            }

            //displays the options highlighting the one they are over.
            void DisplayOptions(int highlighted)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(7, 5);
                if (highlighted == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("Text File".PadLeft(20));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("HTML File".PadLeft(20));
                }
                else
                {
                    Console.Write("Text File".PadLeft(20));
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("HTML File".PadLeft(20));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
