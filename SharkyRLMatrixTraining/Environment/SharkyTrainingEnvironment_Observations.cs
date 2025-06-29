using RLMatrix.Toolkit;

namespace SharkyRLMatrixTraining
{
    public partial class SharkyTrainingEnvironment
    {
        [RLMatrixObservation]
        public float WeaponCooldown() => TrainingState.UnitState.WeaponCooldown;

        [RLMatrixObservation]
        public float DistanceToClosestEnemy() => TrainingState.UnitState.DistanceToClosestEnemy;

        [RLMatrixObservation]
        public float VelocityToClosestEnemy() => TrainingState.UnitState.VelocityToClosestEnemy;

        [RLMatrixObservation]
        public float DistanceToLowestEnemy() => TrainingState.UnitState.DistanceToLowestEnemy;

        [RLMatrixObservation]
        public float EnemiesInRangeCount() => TrainingState.UnitState.EnemiesInRangeCount;

        [RLMatrixObservation]
        public float EnemiesInRangeOfCount() => TrainingState.UnitState.EnemiesInRangeOfCount;

        [RLMatrixObservation]
        public float EnemiesInRangeOfAvoidCount() => TrainingState.UnitState.EnemiesInRangeOfAvoidCount;

        [RLMatrixObservation]
        public float EnemiesThreateningDamageCount() => TrainingState.UnitState.EnemiesThreateningDamageCount;

        [RLMatrixObservation]
        public float NearbyAlliesCount() => TrainingState.UnitState.NearbyAlliesCount;

        [RLMatrixObservation]
        public float NearbyEnemiesCount() => TrainingState.UnitState.NearbyEnemiesCount;

        [RLMatrixObservation]
        public float AttackersCount() => TrainingState.UnitState.AttackersCount;

        [RLMatrixObservation]
        public float TargetersCount() => TrainingState.UnitState.TargetersCount;
    }
}
