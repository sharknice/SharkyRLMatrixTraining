using RLMatrix.Toolkit;

namespace SharkyRLMatrixTraining
{
    public partial class SharkyTrainingEnvironment
    {
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
    }
}
