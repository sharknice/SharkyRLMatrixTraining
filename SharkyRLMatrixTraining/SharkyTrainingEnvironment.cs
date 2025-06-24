using RLMatrix.Toolkit;
using Sharky;

namespace SharkyRLMatrixTraining
{
    [RLMatrixEnvironment]
    public partial class SharkyTrainingEnvironment
    {
        private MicroGym myEnv;
        private float[] myState;
        private float myEndReward;
        private float myCooldownReward;
        private float myKiteReward;
        private int stepCounter;
        private const int MaxSteps = 100000;
        private bool isDone;

        /// <summary>
        /// RLMatrix Sharky Training Environment
        /// </summary>
        /// <param name="myUnit">Your StarCraft 2 unit</param>
        /// <param name="enemyUnit">The enemy StarCraft 2 unit</param>
        /// <param name="stepCount">Number of game steps to take between each played frame in StarCraft 2.</param>
        public SharkyTrainingEnvironment(UnitTypes myUnit, UnitTypes enemyUnit, uint stepCount = 1)
        {
            myEnv = new MicroGym(myUnit, enemyUnit, stepCount);
            ResetEnvironment();
        }

        [RLMatrixObservation]
        public float GetWeaponCooldown() => myState[0];

        [RLMatrixObservation]
        public float GetDistanceToEnemy() => myState[1];

        [RLMatrixObservation]
        public float GetVelocityToEnemy() => myState[2];

        [RLMatrixActionDiscrete(2)]
        public void ApplyForce(int action)
        {
            if (isDone)
            {
                ResetEnvironment();
            }

            var task = myEnv.StepAsync(action);
            task.Wait();

            (float[] observation, float endReward, float cooldownReward, float kiteReward, bool done) = task.Result;

            myState = observation;
            myEndReward = endReward;
            myCooldownReward = cooldownReward;
            isDone = done;
            stepCounter++;

            if (stepCounter > MaxSteps)
            {
                isDone = true;
            }
        }

        [RLMatrixReward]
        public float CalculateEndReward()
        {
            return myEndReward;
        }

        [RLMatrixReward]
        public float CooldownReward()
        {
            return myCooldownReward;
        }

        [RLMatrixReward]
        public float KiteReward()
        {
            return myKiteReward;
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
            myState = [0, 0, 0, 0, 0, 0];
            isDone = false;
            stepCounter = 0;
        }

        public async Task CloseEnvironment()
        {
            await myEnv.CloseAsync();
        }
    }
}
