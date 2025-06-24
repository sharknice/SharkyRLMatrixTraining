using SC2APIProtocol;
using Sharky.DefaultBot;
using Sharky.MicroTasks.Attack;
using Sharky.MicroTasks;
using Sharky;

namespace SharkyRLMatrixTraining
{
    public class MicroGym
    {
        DefaultSharkyBot DefaultSharkyBot;
        GymGameConnection GymGameConnection;
        SharkyBot Bot;
        TrainingAttackTask TrainingAttackTask;

        UnitTypes MyUnit;
        UnitTypes EnemyUnit;

        public MicroGym(UnitTypes myUnit, UnitTypes enemyUnit, uint stepCount = 1)
        {
            MyUnit = myUnit;
            EnemyUnit = enemyUnit;

            GymGameConnection = new GymGameConnection(stepCount);
            DefaultSharkyBot = new DefaultSharkyBot(GymGameConnection);

            foreach (var task in DefaultSharkyBot.MicroTaskData.Values)
            {
                task.Disable();
            }
            DefaultSharkyBot.MicroTaskData.Remove(typeof(AttackTask).Name);

            var trainingIndividualMicroController = new TrainingIndividualMicroController(DefaultSharkyBot, DefaultSharkyBot.SharkyPathFinder, MicroPriority.LiveAndAttack, false, 2f, true);

            TrainingAttackTask = new TrainingAttackTask(DefaultSharkyBot, new EnemyCleanupService(DefaultSharkyBot.MicroController, DefaultSharkyBot.DamageService), new List<UnitTypes> { }, 2f, true, trainingIndividualMicroController);
            DefaultSharkyBot.MicroTaskData[typeof(TrainingAttackTask).Name] = TrainingAttackTask;

            DefaultSharkyBot.EnemyData.EnemyStrategies.Clear();

            DefaultSharkyBot.SharkyOptions.Debug = true; // needed to spawn units for training
            DefaultSharkyBot.SharkyOptions.LogPerformance = false;
            DefaultSharkyBot.SharkyOptions.GameStatusReportingEnabled = false;

            var buildChoices = new TrainingBuildChoices(DefaultSharkyBot);
            DefaultSharkyBot.BuildChoices[Race.Protoss] = buildChoices.MicroChoices;

            Bot = DefaultSharkyBot.CreateBot(DefaultSharkyBot.Managers, DefaultSharkyBot.DebugService);
            
            GymGameConnection.RunTraining(Bot, @"MicroTraining.SC2Map", Race.Protoss, Race.Protoss, Difficulty.VeryHard, AIBuild.Rush, 0, realTime: false).Wait();
        }

        public async Task ResetAsync()
        {
            DefaultSharkyBot.DebugService.KillAlllUnits();
            var observation = await GymGameConnection.GetObservation();
            Bot.OnFrame(observation);
            var actions = Bot.OnFrame(observation);
            await GymGameConnection.SendActions(actions);
            observation = await GymGameConnection.GetObservation();
            Bot.OnFrame(observation);

            while (!ClearBoard(observation))
            {
                observation = await GymGameConnection.GetObservation();
                Bot.OnFrame(observation);
            }

            DefaultSharkyBot.DebugService.SpawnUnit(MyUnit, new Point2D { X = DefaultSharkyBot.MapData.MapWidth / 2 - 2, Y = DefaultSharkyBot.MapData.MapHeight / 2 }, 1);
            DefaultSharkyBot.DebugService.SpawnUnit(EnemyUnit, new Point2D { X = DefaultSharkyBot.MapData.MapWidth / 2 + 2, Y = DefaultSharkyBot.MapData.MapHeight / 2 }, 2);

            observation = await GymGameConnection.GetObservation();
            actions = Bot.OnFrame(observation);
            await GymGameConnection.SendActions(actions);

            while (!ReadyToTrain(observation))
            {
                observation = await GymGameConnection.GetObservation();
                Bot.OnFrame(observation);
            }
        }

        public async Task<(float[] observation, float endReward, float cooldownReward, float kiteReward, bool done)> StepAsync(int action)
        {
            List<SC2APIProtocol.Action> actions;
            if (action == 0)
            {
                actions = TrainingAttackTask.Attack().ToList();
            }
            else
            {
                actions = TrainingAttackTask.Retreat().ToList();
            }

            await GymGameConnection.SendActions(actions);
            var observation = await GymGameConnection.GetObservation();
            Bot.OnFrame(observation);

            var state = TrainingAttackTask.GetState();
            return state;
        }

        private bool ClearBoard(ResponseObservation? observation)
        {
            if (observation.Observation.RawData.Units.Count == 0)
            {
                return true;
            }
            return false;
        }

        private bool ReadyToTrain(ResponseObservation? observation)
        {
            if (observation.Observation.RawData.Units.Count == 2)
            {
                if (TrainingAttackTask.UnitCommanders.Count == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task CloseAsync()
        {
            DefaultSharkyBot.DebugService.Quit();
            var observation = await GymGameConnection.GetObservation();
            var actions = Bot.OnFrame(observation);
            await GymGameConnection.SendActions(actions);
        }
    }
}
