using SC2APIProtocol;
using Sharky;

namespace SharkyRLMatrixTraining
{
    public partial class GymGameConnection : GameConnection
    {
        /// <summary>
        /// Number of game steps to take between each played frame in StarCraft 2.
        /// </summary>
        public uint StepCount { get; set; }

        public GymGameConnection(uint stepCount) : base()
        {
            StepCount = stepCount;
        }

        public async Task RunTraining(ISharkyBot bot, string map, Race myRace, Race opponentRace, Difficulty opponentDifficulty, AIBuild aIBuild, int randomSeed = -1, string opponentID = "test", bool realTime = false, string botName = "bot")
        {
            readSettings();
            StartSC2Instance(5678);
            await Connect(5678);
            await CreateGame(map, opponentRace, opponentDifficulty, aIBuild, randomSeed, realTime);
            var playerId = await JoinGame(myRace);
            await SetupTraining(bot, playerId, opponentID, botName);
        }

        public async Task<ResponseObservation?> GetObservation()
        {
            var stepRequest = new Request
            {
                Step = new RequestStep { Count = StepCount }
            };

            await Proxy.SendRequest(stepRequest);

            var observationRequest = new Request
            {
                Observation = new RequestObservation()
            };
            var response = await Proxy.SendRequest(observationRequest);

            return response.Observation;
        }

        public async Task SendActions(IEnumerable<SC2APIProtocol.Action> actions)
        {
            var actionRequest = new Request();
            actionRequest.Action = new RequestAction();
            actionRequest.Action.Actions.AddRange(actions);

            if (actionRequest.Action.Actions.Count > 0)
            {
                await Proxy.SendRequest(actionRequest);
            }
        }
    }
}
