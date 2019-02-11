using System;
using System.Collections.Generic;
using System.IO;

namespace Project_Two
{
    class Program
    {
        static void Main(string[] args)
        {
            /**Your application should allow the end user to pass end a file path for output 
            * or guide them through generating the file.
            **/

            List<string> rows = new List<string>();
            using (StreamReader reader = new StreamReader(@"..\..\..\Super_Bowl_Project.csv")) {
                while (!reader.EndOfStream)
                {
                    reader.ReadLine();
                    rows.Add(reader.ReadLine());
                }
            }
            List<Superbowl> superbowls = new List<Superbowl>();
            foreach (string row in rows)
            {
                superbowls.Add(new Superbowl(row.Split(',')));
            }
            SuperbowlQuerries queries = new SuperbowlQuerries(superbowls);
            DisplayPrompt();
            Console.ReadKey();




            void DisplayPrompt()
            {
                Console.SetWindowSize(70, 10);
                Console.SetCursorPosition(10, 2);
                Console.WriteLine("Welcome to the superbowl analysis file displayer");
                Console.SetCursorPosition(5, 3);
                Console.WriteLine("Would you like the information in a text file or HTML file?");
                interact();
            }

            void interact()
            {
                bool highlighted = false;
                ConsoleKey userKey = new ConsoleKey();
                while (userKey != ConsoleKey.Enter)
                {
                    DisplayOptions(highlighted);
                    userKey = Console.ReadKey(true).Key;
                    switch (userKey)
                    {
                        case ConsoleKey.A:
                            highlighted = false;
                            break;
                        case ConsoleKey.D:
                            highlighted = true;
                            break;
                        case ConsoleKey.RightArrow:
                            highlighted = true;
                            break;
                        case ConsoleKey.LeftArrow:
                            highlighted = false;
                            break;
                        default:
                            break;
                    }
                    
                }
                if (highlighted)
                {
                    queries.GenerateHTMLFile();
                }
                else
                {
                    queries.GenerateTextFile();
                }
            }

            void DisplayOptions(bool highlighted)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(7, 5);
                if (!highlighted)
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
