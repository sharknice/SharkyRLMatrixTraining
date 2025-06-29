using RLMatrix.Toolkit;
using Sharky.MicroTasks;

namespace SharkyRLMatrixTraining
{
    [RLMatrixEnvironment]
    public partial class SharkyTrainingEnvironment
    {
        private MicroGym myEnv;
        private UnitState myState;
        private float myEndReward;
        private float myCooldownReward;
        private float myKiteReward;
        private int stepCounter;
        private const int MaxSteps = 100000;
        private bool isDone;

        /// <summary>
        /// RLMatrix Sharky Training Environment
        /// </summary>
        /// <param name="myUnits">Your StarCraft 2 units</param>
        /// <param name="enemyUnits">The enemy StarCraft 2 units</param>
        /// <param name="stepCount">Number of game steps to take between each played frame in StarCraft 2.</param>
        public SharkyTrainingEnvironment(List<DesiredUnitsClaim> myUnits, List<DesiredUnitsClaim> enemyUnits, uint stepCount = 1)
        {
            myEnv = new MicroGym(myUnits, enemyUnits, stepCount);
            ResetEnvironment();
        }

        [RLMatrixActionDiscrete(2)]
        public void TakeAction(int action)
        {
            if (isDone)
            {
                ResetEnvironment();
            }

            var task = myEnv.StepAsync(action);
            task.Wait();

            (UnitState unitState, float endReward, float cooldownReward, float kiteReward, bool done) = task.Result;

            myState = unitState;
            myEndReward = endReward;
            myCooldownReward = cooldownReward;
            isDone = done;
            stepCounter++;

            if (stepCounter > MaxSteps)
            {
                isDone = true;
            }
        }

        [RLMatrixDone]
        public bool IsEpisodeFinished()
        {
            return isDone;
        }

        [RLMatrixReset]
        public void ResetEnvironment()
        {
            if (!isDone)
            {
                Console.WriteLine($"Lost: exceeded step limit");
            }

            var task = myEnv.ResetAsync();
            task.Wait();
            myKiteReward = 0;
            myCooldownReward = 0;
            myKiteReward = 0;
            myState = new UnitState();
            isDone = false;
            stepCounter = 0;
        }

        public async Task CloseEnvironment()
        {
            await myEnv.CloseAsync();
        }
    }
}
