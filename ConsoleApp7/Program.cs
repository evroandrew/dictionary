using System;
using System.Collections.Generic;
using ClassLibrary1;

namespace ConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string question;
            for (; ; )
            {
                Console.WriteLine("Enter word");
                question = Console.ReadLine();
                string answer;
                if (question.ToLower().CompareTo("quit") == 0) break;
                if (dictionary.TryGetValue(question.ToLower(), out answer))
                {
                    Console.WriteLine($"Translate: {answer}");
                }
                else
                {
                    Console.Write($"Word {question} not found, enter answer =>");
                    answer = Console.ReadLine();
                    dictionary.Add(question, answer);
                }
            }
        }
    }
}
