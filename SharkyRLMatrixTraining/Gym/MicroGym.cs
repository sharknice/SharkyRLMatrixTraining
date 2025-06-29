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

        List<DesiredUnitsClaim> MyUnits;
        List<DesiredUnitsClaim> EnemyUnits;

        public MicroGym(List<DesiredUnitsClaim> myUnits, List<DesiredUnitsClaim> enemyUnits, uint stepCount = 1)
        {
            MyUnits = myUnits;
            EnemyUnits = enemyUnits;

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

            foreach (var unit in MyUnits)
            {
                DefaultSharkyBot.DebugService.SpawnUnits(unit.UnitType, new Point2D { X = DefaultSharkyBot.MapData.MapWidth / 2 - 5, Y = DefaultSharkyBot.MapData.MapHeight / 2 }, 1, unit.Count);
            }

            foreach (var unit in EnemyUnits)
            {
                DefaultSharkyBot.DebugService.SpawnUnits(unit.UnitType, new Point2D { X = DefaultSharkyBot.MapData.MapWidth / 2 + 5, Y = DefaultSharkyBot.MapData.MapHeight / 2 }, 2, unit.Count);
            }

            observation = await GymGameConnection.GetObservation();
            actions = Bot.OnFrame(observation);
            await GymGameConnection.SendActions(actions);

            while (!ReadyToTrain(observation))
            {
                observation = await GymGameConnection.GetObservation();
                Bot.OnFrame(observation);
            }
        }

        public async Task<(UnitState unitState, float endReward, float cooldownReward, float kiteReward, bool done)> StepAsync(int action)
        {
            var actions = TrainingAttackTask.PerformAction((ActionTypes)action);
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
            if (observation.Observation.RawData.Units.Count == MyUnits.Sum(u => u.Count) + EnemyUnits.Sum(u => u.Count))
            {
                if (TrainingAttackTask.UnitCommanders.Count == MyUnits.Sum(u => u.Count))
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
