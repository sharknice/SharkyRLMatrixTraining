using RLMatrix.Toolkit;

namespace SharkyRLMatrixTraining
{
    public partial class SharkyTrainingEnvironment
    {
        [RLMatrixObservation]
        public float WeaponCooldown() => myState.WeaponCooldown;

        [RLMatrixObservation]
        public float DistanceToClosestEnemy() => myState.DistanceToClosestEnemy;

        [RLMatrixObservation]
        public float VelocityToClosestEnemy() => myState.VelocityToClosestEnemy;

        [RLMatrixObservation]
        public float DistanceToLowestEnemy() => myState.DistanceToLowestEnemy;

        [RLMatrixObservation]
        public float EnemiesInRangeCount() => myState.EnemiesInRangeCount;

        [RLMatrixObservation]
        public float EnemiesInRangeOfCount() => myState.EnemiesInRangeOfCount;

        [RLMatrixObservation]
        public float EnemiesInRangeOfAvoidCount() => myState.EnemiesInRangeOfAvoidCount;

        [RLMatrixObservation]
        public float EnemiesThreateningDamageCount() => myState.EnemiesThreateningDamageCount;

        [RLMatrixObservation]
        public float NearbyAlliesCount() => myState.NearbyAlliesCount;

        [RLMatrixObservation]
        public float NearbyEnemiesCount() => myState.NearbyEnemiesCount;

        [RLMatrixObservation]
        public float AttackersCount() => myState.AttackersCount;

        [RLMatrixObservation]
        public float TargetersCount() => myState.TargetersCount;
    }
}
