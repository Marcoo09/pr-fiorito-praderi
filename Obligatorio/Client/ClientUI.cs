using System;
using System.Text.RegularExpressions;
using Client.Connections;
using Protocol;

namespace Client
{
    public class ClientUI
    {
        private ConnectionsHandler _connectionsHandler;
        private RequestBuilder _requestBuilder;
        private ResponseInterpreter _responseInterpreter;

        public ClientUI()
        {
            _connectionsHandler = new ConnectionsHandler();
            _requestBuilder = new RequestBuilder();
            _responseInterpreter = new ResponseInterpreter();
        }

        public void Init()
        {
            _connectionsHandler.ConnectToServer();
            Console.WriteLine("Connected to server...");

            while (_connectionsHandler.IsClientStateUp())
            {
                int chosenOption = ShowMenu();
                if (chosenOption == -1)
                    _connectionsHandler.ShutDown();
                else
                {
                    Frame requestFrame = _requestBuilder.BuildRequest((short)chosenOption);
                    Frame response = _connectionsHandler.SendRequest(requestFrame);

                    if (response != null)
                    {
                        string interpretedResponse = _responseInterpreter.InterpretResponse(response);
                        Console.WriteLine("\n" + interpretedResponse);
                    }
                }
            }
        }

        private int ShowMenu()
        {
            int option = -1;
            Array commands = CommandConstants.GetValues(typeof(CommandConstants));

            Console.WriteLine("\n\nChoose an option:");
            Console.WriteLine("0 - Disconnect from server");
            for (int i = 0; i < commands.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {GetMenuString((CommandConstants)commands.GetValue(i))}");
            }

            if (Int32.TryParse(Console.ReadLine(), out int chosenOption) && chosenOption != -1 && chosenOption <= commands.Length)
            {
                option = chosenOption - 1;
            }
            else
            {
                Console.WriteLine("Invalid option, please enter a new one");
                option = ShowMenu();
            }

            return option;
        }

        public string GetMenuString(CommandConstants command)
        {
            string menuString = "";
            switch (command)
            {
                case CommandConstants.BuyGame:
                    menuString = "Buy a game";
                    break;
                case CommandConstants.IndexBoughtGames:
                    menuString = "Show my bought games";
                    break;
                case CommandConstants.CreateGame:
                    menuString = "Create a game";
                    break;
                case CommandConstants.CreateGameReview:
                    menuString = "Give a review";
                    break;
                case CommandConstants.DeleteGame:
                    menuString = "Delete a game";
                    break;
                case CommandConstants.GetGameReviews:
                    menuString = "Get game reviews";
                    break;
                case CommandConstants.GetGame:
                    menuString = "Get game information";
                    break;
                case CommandConstants.IndexGamesCatalog:
                    menuString = "Get all games";
                    break;
                case CommandConstants.SearchGames:
                    menuString = "Search games by title, gender or rating";
                    break;
                case CommandConstants.UpdateGame:
                    menuString = "Update game info";
                    break;
                case CommandConstants.IndexUsers:
                    menuString = "Get server users";
                    break;
            }

            return menuString;
        }
    }
}
