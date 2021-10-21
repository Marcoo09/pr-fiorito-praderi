using System;
using System.Text.RegularExpressions;
using Client.Connections;
using Protocol;

namespace Client
{
    public class ClientUI
    {
        private ConnectionsHandler _connectionsHandler;
        private ServerService _serverService;
        private ServerDeserializer _serverDeserializer;
        private bool _clientAuthenticated;

        public ClientUI()
        {
            _connectionsHandler = new ConnectionsHandler();
            _serverService = new ServerService();
            _serverDeserializer = new ServerDeserializer();
            _clientAuthenticated = false;
        }

        public void Init()
        {
            _connectionsHandler.ConnectToServer();

            while (_connectionsHandler.IsClientStateUp())
            {
                if (_clientAuthenticated)
                {
                    int chosenOption = ShowMenu();
                    if (chosenOption == -1)
                        _connectionsHandler.ShutDown();
                    else
                    {
                        Frame requestFrame = _serverService.BuildRequest((short)chosenOption);
                        Frame response = _connectionsHandler.SendRequest(requestFrame);

                        if (response != null)
                        {
                            string interpretedResponse = _serverDeserializer.DeserializeResponse(response);
                            Console.WriteLine("\n" + interpretedResponse);
                        }
                    }
                }
                else
                {
                    int chosenOption = LoginMenu();
                    if (chosenOption == -1)
                        _connectionsHandler.ShutDown();
                    else
                    {
                        Frame requestFrame = _serverService.BuildRequest((short)CommandConstants.Login);
                        Frame response = _connectionsHandler.SendRequest(requestFrame);

                        if (response != null)
                        {
                            string interpretedResponse = _serverDeserializer.DeserializeResponse(response);
                            Console.WriteLine("\n" + interpretedResponse);
                        }
                        _clientAuthenticated = true;
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
                string menuString = GetMenuString((CommandConstants)commands.GetValue(i));
                if (menuString != "")
                {
                    Console.WriteLine($"{i + 1} - {menuString}");
                }
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

        private int LoginMenu()
        {
            int option = -1;
            Array commands = CommandConstants.GetValues(typeof(CommandConstants));

            Console.WriteLine("\n\nChoose an option:");
            Console.WriteLine("0 - Disconnect from server\n1- Login");

            if (Int32.TryParse(Console.ReadLine(), out int chosenOption) && chosenOption != -1 && chosenOption <= 1)
            {
                option = chosenOption - 1;
            }
            else
            {
                Console.WriteLine("Invalid option, please enter a new one");
                option = LoginMenu();
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
