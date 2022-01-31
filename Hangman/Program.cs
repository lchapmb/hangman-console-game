using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Hangman
{
    class Program
    {

        public static bool isPlaying = true;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hangman!");

            while(isPlaying)
            {
                string endMsg = Game();
                Console.WriteLine(endMsg);
            }


            

            // Console clear message
            //Console.Clear

        }

        public static string returnRandomWord()
        {

            string html = string.Empty;
            string url = @"https://random-word-api.herokuapp.com/word";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            html = html.Replace("[\"", "");
            html = html.Replace("\"]", "");

            return html;
        }

        public static string Game()
        {
            // generating the word
            // C:\Users\lewis.chapman-barker\source\repos\Hangman\words\lotr.txt
            string answer = returnRandomWord();
            Console.WriteLine(answer);

            //using (var sr = new StreamReader("C:/Users/lewis.chapman-barker/source/repos/Hangman/words/lotr.txt"))
            //{
            //    string[] wordsArr = sr.ReadLine().Split(" ");
            //    Random random = new Random();
            //    answer = wordsArr[random.Next(0, wordsArr.Length)].ToLower();
            //}

            // creating the guess tiles
            string[] guessTiles = new string[answer.Length];
            for (int i = 0; i < answer.Length; i++)
            {
                guessTiles[i] = " _";
            }
            int lettersRemaining = guessTiles.Count(t => t == " _");

            // setting lives (9)
            int playerLives = 9;

            // list of letters guess
            List<string> previousGuesses = new List<string>();

            // starting loop while lives are greate than 0
            while (playerLives > 0 && lettersRemaining > 0)
            {
                Console.WriteLine($"You have {playerLives} lives!");
                string playingBoard = string.Join("", guessTiles);
                Console.WriteLine($"Your word to guess is: {playingBoard}");

                if (previousGuesses.Count > 0)
                {
                    string previousGuessesString = "";

                    foreach (string guess in previousGuesses)
                    {
                        previousGuessesString += guess + " ";
                    }

                    Console.WriteLine($"So far you have guessed: {previousGuessesString}");
                }
                Console.WriteLine($"");

                // getting the guess
                // start assuming only one letter per guess
                // throw error if more than one letter, don't take guess
                Console.WriteLine("What is your guess?");
                string playerGuess = Console.ReadLine();
                Console.WriteLine($"You guessed {playerGuess}.");

                // comparing with previous guesses
                if (!previousGuesses.Contains(playerGuess))
                {

                    // comparing with the answer
                    if (answer.Contains(playerGuess))
                    {
                        Console.WriteLine("Woohoo, that's right!");
                        for (int i = 0; i < answer.Length; i++)
                        {
                            if (answer[i].ToString() == playerGuess)
                                guessTiles[i] = playerGuess;
                        }
                        // add guess to previousGuesses
                        previousGuesses.Add(playerGuess);
                    }
                    else
                    {
                        Console.WriteLine("Sorry, that letter isn't in the answer. Try again!");
                        playerLives--;
                        previousGuesses.Add(playerGuess);
                    }
                    lettersRemaining = guessTiles.Count(t => t == " _");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($"You have already guessed {playerGuess}, try again...");
                }
            }

            string endGameMsg = "Something went wrong, sorry...";
            if (playerLives < 1)
            {
                Console.WriteLine($"The answer was {answer}");
                endGameMsg = "~~~~~GAME OVER!~~~~~";
            }
            else if (lettersRemaining < 1)
            {
                Console.WriteLine($"The answer was {answer}");
                endGameMsg = "----- You won! -----";
            }



            return endGameMsg;
        }
    }

}

