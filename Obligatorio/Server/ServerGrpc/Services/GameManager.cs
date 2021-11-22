using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTOs.Request;
using DTOs.Response;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Protocol;
using Protocol.Serialization;
using Protocol.SerializationInterfaces;
using ServerGrpc.Implementations;

namespace ServerGrpc.Services
{
    public class GameManager: GameAdminService.GameAdminServiceBase
    {
        private ServiceRouter _serviceRouter;
        private readonly IDeserializer _deserializer;

        public GameManager(ServiceRouter serviceRouter)
        {
            _serviceRouter = serviceRouter;
            _deserializer = new Deserializer();
        }

        public override async Task<Message> AddReview(Review request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.CreateGameReview,
                Data = new ReviewDTO()
                {
                    GameId = request.GameId,
                    Description = request.Description,
                    Rating = request.Rating
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);

            MessageDTO messageDto = new MessageDTO();
            messageDto.Deserialize(response.Data);

            return new Message()
            {
                Value = messageDto.Message
            };
        }

        public override async Task<GameBasicInfoResponse> CreateGame(CreateGameRequest request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.CreateGame,
                Data = new CreateGameDTO()
                {
                    Title = request.Title,
                    Gender = request.Gender,
                    Synopsis = request.Synopsis,
                    CoverName = request.CoverName,
                    //Data = request.Data.ToByteArray(),
                    //FileSize = request.FileSize,
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);

            GameBasicInfoDTO gameBasicInfoDTO = new GameBasicInfoDTO();
            gameBasicInfoDTO.Deserialize(response.Data);

            return new GameBasicInfoResponse()
            {
                Id = gameBasicInfoDTO.Id,
                Title = gameBasicInfoDTO.Title
            };
        }

        public override async Task<Message> DeleteGame(BasicGameRequest request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.DeleteGame,
                Data = new BasicGameRequestDTO()
                {
                    GameId = request.GameId
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);

            MessageDTO messageDto = new MessageDTO();
            messageDto.Deserialize(response.Data);

            return new Message()
            {
                Value = messageDto.Message
            };
        }

        public override async Task<IndexReviewResponse> GetAllReviews(BasicGameRequest request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.GetGameReviews,
                Data = new BasicGameRequestDTO()
                {
                    GameId = request.GameId
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);
            if (response.IsSuccessful())
            {
                List<IDeserializable> entities = _deserializer.DeserializeArrayOfEntities(response.Data, typeof(ReviewDetailDTO));
                List<ReviewDetailDTO> reviews = entities.Cast<ReviewDetailDTO>().ToList();

                IndexReviewResponse mappedResponse = new IndexReviewResponse() { Ok = true };
                reviews.ForEach(r => mappedResponse.Reviews.Add(new ReviewDetail()
                {
                    Description = r.Description,
                    Rating = r.Rating
                }));

                return mappedResponse;
            }
            else
            {

                MessageDTO messageDto = new MessageDTO();
                messageDto.Deserialize(response.Data);

                return new IndexReviewResponse()
                {
                    Ok = false,
                    Error = new Error()
                    {
                        Message = messageDto.Message
                    }
                };
            }
        }

        public override async Task<SearchGameResponse> SearchGameByTitle(SearchTitleMetric request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.SearchGames,
                Data = new SearchMetricDTO()
                {
                    Title = request.Title,
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);
            if (response.IsSuccessful())
            {
                List<IDeserializable> entities = _deserializer.DeserializeArrayOfEntities(response.Data, typeof(GameDetailDTO));
                List<GameDetailDTO> games = entities.Cast<GameDetailDTO>().ToList();

                SearchGameResponse mappedResponse = new SearchGameResponse() { Ok = true };
                games.ForEach(g => mappedResponse.Games.Add(new GameDetail()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Gender = g.Gender,
                    Path = g.Path,
                    Synopsis = g.Synopsis,
                }));

                return mappedResponse;
            }
            else
            {

                MessageDTO messageDto = new MessageDTO();
                messageDto.Deserialize(response.Data);

                return new SearchGameResponse()
                {
                    Ok = false,
                    Error = new Error()
                    {
                        Message = messageDto.Message
                    }
                };
            }
        }

        public override async Task<SearchGameResponse> SearchGameByRating(SearchRatingMetric request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.SearchGames,
                Data = new SearchMetricDTO()
                {
                    Rating = request.Rating,
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);
            if (response.IsSuccessful())
            {
                List<IDeserializable> entities = _deserializer.DeserializeArrayOfEntities(response.Data, typeof(GameDetailDTO));
                List<GameDetailDTO> games = entities.Cast<GameDetailDTO>().ToList();

                SearchGameResponse mappedResponse = new SearchGameResponse() { Ok = true };
                games.ForEach(g => mappedResponse.Games.Add(new GameDetail()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Gender = g.Gender,
                    Path = g.Path,
                    Synopsis = g.Synopsis,
                }));

                return mappedResponse;
            }
            else
            {

                MessageDTO messageDto = new MessageDTO();
                messageDto.Deserialize(response.Data);

                return new SearchGameResponse()
                {
                    Ok = false,
                    Error = new Error()
                    {
                        Message = messageDto.Message
                    }
                };
            }
        }

        public override async Task<SearchGameResponse> SearchGameByGender(SearchGenderMetric request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.SearchGames,
                Data = new SearchMetricDTO()
                {
                    Gender = request.Gender,
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);
            if (response.IsSuccessful())
            {
                List<IDeserializable> entities = _deserializer.DeserializeArrayOfEntities(response.Data, typeof(GameDetailDTO));
                List<GameDetailDTO> games = entities.Cast<GameDetailDTO>().ToList();

                SearchGameResponse mappedResponse = new SearchGameResponse() { Ok = true };
                games.ForEach(g => mappedResponse.Games.Add(new GameDetail()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Gender = g.Gender,
                    Path = g.Path,
                    Synopsis = g.Synopsis,
                }));

                return mappedResponse;
            }
            else
            {

                MessageDTO messageDto = new MessageDTO();
                messageDto.Deserialize(response.Data);

                return new SearchGameResponse()
                {
                    Ok = false,
                    Error = new Error()
                    {
                        Message = messageDto.Message
                    }
                };
            }
        }


        public override async Task<ShowGameResponse> ShowGame(BasicGameRequest request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.GetGame,
                Data = new BasicGameRequestDTO()
                {
                    GameId = request.GameId
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);

            if (response.IsSuccessful())
            {
                EnrichedGameDetailDTO enrichedGameDetailDTO = new EnrichedGameDetailDTO();
                enrichedGameDetailDTO.Deserialize(response.Data);

                return new ShowGameResponse()
                {
                    Ok = true,
                    Game = new EnrichGameDetail()
                    {
                        Id = enrichedGameDetailDTO.Id,
                        Title = enrichedGameDetailDTO.Title,
                        Gender = enrichedGameDetailDTO.Gender,
                        //Path = enrichedGameDetailDTO.Path,
                        Synopsis = enrichedGameDetailDTO.Synopsis,
                        //FileSize = enrichedGameDetailDTO.FileSize,
                        RatingAverage = enrichedGameDetailDTO.RatingAverage,
                        CoverName = enrichedGameDetailDTO.CoverName,
                        //Data = ByteString.CopyFrom(enrichedGameDetailDTO.Data)
                    }

                };
            }
            else
            {

                MessageDTO messageDto = new MessageDTO();
                messageDto.Deserialize(response.Data);

                return new ShowGameResponse()
                {
                    Ok = false,
                    Error = new Error()
                    {
                        Message = messageDto.Message
                    }
                };
            }


        }

        public override async Task<IndexGameResponse> ShowGames(Empty request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.IndexGamesCatalog,

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);
            if (response.IsSuccessful())
            {
                List<IDeserializable> entities = _deserializer.DeserializeArrayOfEntities(response.Data, typeof(GameDetailDTO));
                List<GameDetailDTO> games = entities.Cast<GameDetailDTO>().ToList();

                IndexGameResponse mappedResponse = new IndexGameResponse() { Ok = true };
                games.ForEach(g => mappedResponse.Games.Add(new GameDetail()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Gender = g.Gender,
                    Path = g.Path,
                    Synopsis = g.Synopsis,
                }));

                return mappedResponse;
            }
            else
            {

                MessageDTO messageDto = new MessageDTO();
                messageDto.Deserialize(response.Data);

                return new IndexGameResponse()
                {
                    Ok = false,
                    Error = new Error()
                    {
                        Message = messageDto.Message
                    }
                };
            }
        }

        public override async Task<UpdateGameResponse> UpdateGame(UpdateGameRequest request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.UpdateGame,
                Data = new UpdateGameDTO()
                {
                    Id = request.Id,
                    Title = request.Title,
                    Gender = request.Gender,
                    Synopsis = request.Synopsis,
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);

            if (response.IsSuccessful())
            {
                GameBasicInfoDTO gameBasicInfoDTO = new GameBasicInfoDTO();
                gameBasicInfoDTO.Deserialize(response.Data);

                return new UpdateGameResponse()
                {
                    Ok = true,
                    Game = new GameBasicInfoResponse()
                    {
                        Id = gameBasicInfoDTO.Id,
                        Title = gameBasicInfoDTO.Title
                    }
                };
            }
            else
            {

                MessageDTO messageDto = new MessageDTO();
                messageDto.Deserialize(response.Data);

                return new UpdateGameResponse()
                {
                    Ok = false,
                    Error = new Error()
                    {
                        Message = messageDto.Message
                    }
                };
            }
        }
    }
}
