using System;
using DTOs.Request;
using Protocol;

namespace Client
{
    public class ServerService
    {
        public ServerService()
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

            switch ((CommandConstants)chosenOption)
            {
                case CommandConstants.BuyGame:
                    BuildCreateBuyRequest(requestFrame);
                    break;
                case CommandConstants.IndexBoughtGames:
                    break;
                case CommandConstants.CreateGame:
                    BuildCreateGameRequest(requestFrame);
                    break;
                case CommandConstants.CreateGameReview:
                    BuildCreateGameReview(requestFrame);
                    break;
                case CommandConstants.DeleteGame:
                    BuildCreateDeleteRequest(requestFrame);
                    break;
                case CommandConstants.GetGameReviews:
                    BuildCreateGetAllReviewsRequest(requestFrame);
                    break;
                case CommandConstants.GetGame:
                    BuildGetGameDetail(requestFrame);
                    break;
                case CommandConstants.IndexGamesCatalog:
                    break;
                case CommandConstants.SearchGames:
                    BuildSearchGames(requestFrame);
                    break;
                case CommandConstants.UpdateGame:
                    BuildUpdateGameRequest(requestFrame);
                    break;
                case CommandConstants.IndexUsers:
                    break;
                case CommandConstants.Login:
                    BuildLoginRequest(requestFrame);
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

        private void BuildLoginRequest(Frame requestFrame)
        {
            LoginDTO loginDTO = new LoginDTO();

            Console.WriteLine("Indicate the user name:");
            loginDTO.UserName = Console.ReadLine();

            byte[] loginData = loginDTO.Serialize();
            requestFrame.Data = loginData;
            requestFrame.DataLength = loginData.Length;
        }

        private void BuildUpdateGameRequest(Frame requestFrame)
        {
            UpdateGameDTO updateGameDTO = new UpdateGameDTO();
            Console.WriteLine("Indicate the Id of the game to update");
            updateGameDTO.Id = GetIntFromConsoleApp();
            Console.WriteLine("Indicate the new name for the game");
            updateGameDTO.Title = Console.ReadLine();
            Console.WriteLine("Indicate the new synopsis for the game");
            updateGameDTO.Synopsis = Console.ReadLine();
            Console.WriteLine("Indicate the new gender for the game");
            updateGameDTO.Gender = Console.ReadLine();

            byte[] updateGameData = updateGameDTO.Serialize();
            requestFrame.Data = updateGameData;
            requestFrame.DataLength = updateGameData.Length;
        }

        private void BuildSearchGames(Frame requestFrame)
        {

            Console.Write("Indicate the search metric you want to use:" + "\n1- Title\n2- Gender\n3 - Rating\n");
            int searchMetric = GetIntFromConsoleApp();
            SearchMetricDTO searchMetricDTO = new SearchMetricDTO();
            if (searchMetric == 1)
            {
                Console.WriteLine("Indicate the title to search:");
                searchMetricDTO.Title = Console.ReadLine();
            }
            else if(searchMetric == 2)
            {
                Console.WriteLine("Indicate the gender to search:");
                searchMetricDTO.Gender = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Indicate the Rating to search:");
                searchMetricDTO.Rating = GetIntFromConsoleApp();
            }

            byte[] searchMetricData = searchMetricDTO.Serialize();
            requestFrame.Data = searchMetricData;
            requestFrame.DataLength = searchMetricData.Length;
        }

        private void BuildGetGameDetail(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();

            Console.WriteLine("Indicate the game id to show:");
            basicGameRequestDTO.GameId = GetIntFromConsoleApp();

            byte[] gameDetailData = basicGameRequestDTO.Serialize();
            requestFrame.Data = gameDetailData;
            requestFrame.DataLength = gameDetailData.Length;
        }

        private void BuildCreateBuyRequest(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();

            Console.WriteLine("Indicate the game id to buy:");
            basicGameRequestDTO.GameId = GetIntFromConsoleApp();

            byte[] buyGameData = basicGameRequestDTO.Serialize();
            requestFrame.Data = buyGameData;
            requestFrame.DataLength = buyGameData.Length;
        }

        private void BuildCreateDeleteRequest(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();

            Console.WriteLine("Indicate the game id to delete:");
            basicGameRequestDTO.GameId = GetIntFromConsoleApp();

            byte[] buyGameData = basicGameRequestDTO.Serialize();
            requestFrame.Data = buyGameData;
            requestFrame.DataLength = buyGameData.Length;
        }

        private void BuildCreateGetAllReviewsRequest(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();

            Console.WriteLine("Indicate the game id to see the reviews:");
            basicGameRequestDTO.GameId = GetIntFromConsoleApp();

            byte[] allReviewsData = basicGameRequestDTO.Serialize();
            requestFrame.Data = allReviewsData;
            requestFrame.DataLength = allReviewsData.Length;
        }

        private void BuildCreateGameReview(Frame requestFrame)
        {
            ReviewDTO reviewDTO = new ReviewDTO();

            Console.WriteLine("Indicate the game id to review:");
            reviewDTO.GameId = GetIntFromConsoleApp();
            Console.WriteLine("Indicate the rating between the range 1-5:");
            reviewDTO.Rating = GetIntFromConsoleApp();
            Console.WriteLine("Indicate a brief description about your experience");
            reviewDTO.Description = Console.ReadLine();

            byte[] reviewData = reviewDTO.Serialize();
            requestFrame.Data = reviewData;
            requestFrame.DataLength = reviewData.Length;
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
