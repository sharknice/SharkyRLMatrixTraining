namespace SharkyRLMatrixTraining
{
    public class UnitState
    {
        public UnitState() 
        {
            WeaponCooldown = 0f;
            DistanceToClosestEnemy = 0f;
            VelocityToClosestEnemy = 0f;
            DistanceToLowestEnemy = 0f;
            EnemiesInRangeCount = 0;
            EnemiesInRangeOfCount = 0;
            EnemiesInRangeOfAvoidCount = 0;
            EnemiesThreateningDamageCount = 0;
            NearbyAlliesCount = 0;
            NearbyEnemiesCount = 0;
            AttackersCount = 0;
            TargetersCount = 0;
        }

        public float WeaponCooldown { get; set; }

        public float DistanceToClosestEnemy { get; set; }

        public float VelocityToClosestEnemy { get; set; }

        public float DistanceToLowestEnemy { get; set; }

        public int EnemiesInRangeCount { get; set; }

        public int EnemiesInRangeOfCount { get; set; }

        public int EnemiesInRangeOfAvoidCount { get; set; }

        public int EnemiesThreateningDamageCount { get; set; }

        public int NearbyAlliesCount { get; set; }

        public int NearbyEnemiesCount { get; set; }

        public int AttackersCount { get; set; }

        public int TargetersCount { get; set; }
    }
}
