using RLMatrix.Toolkit;

namespace SharkyRLMatrixTraining
{
    public partial class SharkyTrainingEnvironment
    {
        [RLMatrixReward]
        public float CalculateEndReward()
        {
            return TrainingState.EndReward;
        }

        [RLMatrixReward]
        public float CooldownReward()
        {
            return TrainingState.CooldownReward;
        }

        [RLMatrixReward]
        public float KiteReward()
        {
            return TrainingState.KiteReward;
        }
    }
}
