using SC2APIProtocol;
using Sharky;
using System.Diagnostics;

namespace SharkyRLMatrixTraining
{
    public partial class GymGameConnection : GameConnection
    {
        public async Task SetupTraining(ISharkyBot bot, uint playerId, string opponentID, string botName = "")
        {
            var gameInfoReq = new Request();
            gameInfoReq.GameInfo = new RequestGameInfo();

            var gameInfoResponse = await Proxy.SendRequest(gameInfoReq);

            var gameDataRequest = new Request();
            gameDataRequest.Data = new RequestData();
            gameDataRequest.Data.UnitTypeId = true;
            gameDataRequest.Data.AbilityId = true;
            gameDataRequest.Data.BuffId = true;
            gameDataRequest.Data.EffectId = true;
            gameDataRequest.Data.UpgradeId = true;

            var dataResponse = await Proxy.SendRequest(gameDataRequest);

            var pingResponse = await Ping();

            var start = true;

            var observationRequest = new Request
            {
                Observation = new RequestObservation()
            };

            var stepRequest = new Request
            {
                Step = new RequestStep { Count = 1 }
            };

            while (true)
            {
                var beginTotal = Stopwatch.GetTimestamp();

                if (!start)
                {
                    await Proxy.SendRequest(stepRequest);
                }
                var begin = Stopwatch.GetTimestamp();
                var response = await Proxy.SendRequest(observationRequest);

                if (start)
                {
                    foreach (var playerInfo in gameInfoResponse.GameInfo.PlayerInfo)
                    {
                        if (string.IsNullOrEmpty(playerInfo.PlayerName))
                        {
                            if (playerInfo.PlayerId == playerId)
                            {
                                playerInfo.PlayerName = botName;
                            }
                            else
                            {
                                playerInfo.PlayerName = opponentID;
                            }
                        }
                    }
                    Console.WriteLine($"Started at {DateTime.UtcNow} UTC");
                    start = false;
                    bot.OnStart(gameInfoResponse.GameInfo, dataResponse.Data, pingResponse, response.Observation, playerId, opponentID);
                }

                var actions = bot.OnFrame(response.Observation);

                var actionRequest = new Request();
                actionRequest.Action = new RequestAction();
                actionRequest.Action.Actions.AddRange(actions);

                if (actionRequest.Action.Actions.Count > 0)
                {
                    await Proxy.SendRequest(actionRequest);
                }

                return;
            }
        }
    }
}
