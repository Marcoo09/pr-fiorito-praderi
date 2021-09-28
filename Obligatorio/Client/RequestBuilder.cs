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
                    BuildCreateBuyRequest(requestFrame);
                    break;
                case Command.IndexBoughtGames:
                    break;
                case Command.CreateGame:
                    BuildCreateGameRequest(requestFrame);
                    break;
                case Command.CreateGameReview:
                    BuildCreateGameReview(requestFrame);
                    break;
                case Command.DeleteGame:
                    //Do sth
                    break;
                case Command.GetGameReviews:
                    BuildCreateGetAllReviewsRequest(requestFrame);
                    break;
                case Command.IndexGame:
                    BuildGetGameDetail(requestFrame);
                    break;
                case Command.IndexGamesCatalog:
                    break;
                case Command.SearchGames:
                    BuildSearchGames(requestFrame);
                    break;
                case Command.UpdateGame:
                    //Do sth
                    break;
                case Command.IndexUsers:
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
            GetGameDTO gameDetailDTO = new GetGameDTO();

            Console.WriteLine("Indicate the game id to show:");
            gameDetailDTO.GameId = GetIntFromConsoleApp();

            byte[] gameDetailData = gameDetailDTO.Serialize();
            requestFrame.Data = gameDetailData;
            requestFrame.DataLength = gameDetailData.Length;
        }

        private void BuildCreateBuyRequest(Frame requestFrame)
        {
            BuyGameDTO buyGameDTO = new BuyGameDTO();

            Console.WriteLine("Indicate the game id to buy:");
            buyGameDTO.GameId = GetIntFromConsoleApp();

            byte[] buyGameData = buyGameDTO.Serialize();
            requestFrame.Data = buyGameData;
            requestFrame.DataLength = buyGameData.Length;
        }

        private void BuildCreateGetAllReviewsRequest(Frame requestFrame)
        {
            AllGameReviewsDTO allGameReviewsDTO = new AllGameReviewsDTO();

            Console.WriteLine("Indicate the game id to see the reviews:");
            allGameReviewsDTO.GameId = GetIntFromConsoleApp();

            byte[] allReviewsData = allGameReviewsDTO.Serialize();
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
