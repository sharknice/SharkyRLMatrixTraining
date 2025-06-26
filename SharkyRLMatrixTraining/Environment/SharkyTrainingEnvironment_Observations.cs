using RLMatrix.Toolkit;

namespace SharkyRLMatrixTraining
{
    public partial class SharkyTrainingEnvironment
    {
        [RLMatrixObservation]
        public float GetWeaponCooldown() => myState[0];

        [RLMatrixObservation]
        public float GetDistanceToClosestEnemy() => myState[1];

        [RLMatrixObservation]
        public float GetVelocityToClosestEnemy() => myState[2];
    }
}
