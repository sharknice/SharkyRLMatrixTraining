using RLMatrix.Toolkit;
using Sharky.MicroTasks;

namespace SharkyRLMatrixTraining
{
    [RLMatrixEnvironment]
    public partial class SharkyTrainingEnvironment
    {
        private MicroGym MicroGym;
        private TrainingState TrainingState;
        private int StepCounter;
        private const int MaxSteps = 100000;

        /// <summary>
        /// RLMatrix Sharky Training Environment
        /// </summary>
        /// <param name="myUnits">Your StarCraft 2 units</param>
        /// <param name="enemyUnits">The enemy StarCraft 2 units</param>
        /// <param name="stepCount">Number of game steps to take between each played frame in StarCraft 2.</param>
        public SharkyTrainingEnvironment(List<DesiredUnitsClaim> myUnits, List<DesiredUnitsClaim> enemyUnits, uint stepCount = 1)
        {
            MicroGym = new MicroGym(myUnits, enemyUnits, stepCount);
            TrainingState = new TrainingState();
            ResetEnvironment();
        }

        [RLMatrixActionDiscrete(2)]
        public void TakeAction(int action)
        {
            if (TrainingState.Done)
            {
                ResetEnvironment();
            }

            var task = MicroGym.StepAsync(action);
            task.Wait();

            TrainingState = task.Result;
            StepCounter++;

            if (StepCounter > MaxSteps)
            {
                TrainingState.Done = true;
            }
        }

        [RLMatrixDone]
        public bool IsEpisodeFinished()
        {
            return TrainingState.Done;
        }

        [RLMatrixReset]
        public void ResetEnvironment()
        {
            if (!TrainingState.Done)
            {
                Console.WriteLine($"Lost: exceeded step limit");
            }

            var task = MicroGym.ResetAsync();
            task.Wait();
            TrainingState = new TrainingState();
            StepCounter = 0;
        }

        public async Task CloseEnvironment()
        {
            await MicroGym.CloseAsync();
        }
    }
}
