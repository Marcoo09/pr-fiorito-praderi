using System;
using DTOs.Request;
using Protocol;

namespace Client
{
    public class RequestBuilder
    {
        public RequestBuilder()
        {
        }

        public Frame BuildRequest(short chosenOption)
        {
            Frame requestFrame = new Frame()
            {
                ChosenHeader = (short)Header.Request,
                ChosenCommand = chosenOption
            };
            Console.WriteLine("");

            // Some commands do not need extra configuration
            switch ((Command)chosenOption)
            {
                case Command.BuyGame:
                    //Do sth
                    break;
                case Command.CreateGame:
                    BuildCreateGameRequest(requestFrame);
                    break;
                case Command.CreateGameReview:
                    //Do sth
                    break;
                case Command.DeleteGame:
                    //Do sth
                    break;
                case Command.GetGameReviews:
                    //Do sth
                    break;
                case Command.IndexGame:
                    //Do sth
                    break;
                case Command.IndexGamesCatalog:
                    //Do sth
                    break;
                case Command.SearchGames:
                    //Do sth
                    break;
                case Command.UpdateGame:
                    //Do sth
                    break;
            }
            return requestFrame;
        }

        private int GetIntFromConsoleApp()
        {
            bool isValid = false;
            int intFromConsole = 0;

            while (!isValid)
            {
                if (Int32.TryParse(Console.ReadLine(), out intFromConsole))
                    isValid = true;
                else
                    Console.WriteLine("Please enter a number: ");
            }

            return intFromConsole;
        }

        private void BuildCreateGameRequest(Frame requestFrame)
        {
            CreateGameDTO createGameDto = new CreateGameDTO();

            Console.WriteLine("Indicate the new game title:");
            createGameDto.Title = Console.ReadLine();
            Console.WriteLine("Indicate the new game synopsis:");
            createGameDto.Synopsis = Console.ReadLine();
            Console.WriteLine("Indicate the new game gender:");
            createGameDto.Gender = Console.ReadLine();

            Console.WriteLine("Indicate the full path of the cover:");
            string path = Console.ReadLine();

            while (!createGameDto.IsValidPath(path))
            {
                Console.WriteLine("Please indicate a path that exists");
                path = Console.ReadLine();
            }

            createGameDto.ReadFile(path);
            byte[] createGameData = createGameDto.Serialize();
            requestFrame.Data = createGameData;
            requestFrame.DataLength = createGameData.Length;
        }



    }
}
