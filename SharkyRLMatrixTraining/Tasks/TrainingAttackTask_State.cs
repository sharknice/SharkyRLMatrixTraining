using Sharky.MicroTasks.Attack;
using Sharky;
using System.Numerics;

namespace SharkyRLMatrixTraining
{
    public partial class TrainingAttackTask : AdvancedAttackTask
    {
        public (UnitState state, float endRward, float cooldownReward, float kiteReward, bool done) GetState()
        {
            bool done = false;
            float endRward = 0;
            var cooldownReward = 0f;
            var kiteReward = 0f;
            var state = new UnitState();
            var commander = UnitCommanders.FirstOrDefault();
            if (commander != null)
            {
                state.WeaponCooldown = commander.UnitCalculation.Unit.WeaponCooldown;
                if (commander.UnitCalculation.Unit.WeaponCooldown > 0)
                {
                    cooldownReward = 1f;
                }

                var closestEnemy = commander.UnitCalculation.NearbyEnemies.OrderBy(e => Vector2.Distance(e.Position, commander.UnitCalculation.Position)).FirstOrDefault();
                if (closestEnemy != null)
                {
                    state.DistanceToClosestEnemy = Vector2.Distance(commander.UnitCalculation.Position, closestEnemy.Position);
                    if (closestEnemy.PreviousUnitCalculation != null && commander.UnitCalculation.PreviousUnitCalculation != null)
                    {
                        state.VelocityToClosestEnemy = Vector2.Distance(commander.UnitCalculation.Position, closestEnemy.Position) - Vector2.Distance(commander.UnitCalculation.PreviousUnitCalculation.Position, closestEnemy.PreviousUnitCalculation.Position); 
                    }

                    state.AttackersCount = commander.UnitCalculation.Attackers.Count;
                    state.EnemiesInRangeCount = commander.UnitCalculation.EnemiesInRange.Count;
                    state.EnemiesInRangeOfCount = commander.UnitCalculation.EnemiesInRangeOf.Count;
                    state.EnemiesInRangeOfAvoidCount = commander.UnitCalculation.EnemiesInRangeOfAvoid.Count;
                    state.EnemiesThreateningDamageCount = commander.UnitCalculation.EnemiesThreateningDamage.Count;
                    state.NearbyAlliesCount = commander.UnitCalculation.NearbyAllies.Count;
                    state.NearbyEnemiesCount = commander.UnitCalculation.NearbyEnemies.Count;
                    state.TargetersCount = commander.UnitCalculation.Targeters.Count;

                    if (commander.UnitCalculation.Unit.WeaponCooldown > 0)
                    {
                        kiteReward = Vector2.Distance(commander.UnitCalculation.Position, closestEnemy.Position) - 3f;
                    }
                }
                else
                {
                    endRward = (commander.UnitCalculation.Unit.Health/commander.UnitCalculation.Unit.HealthMax) + ((commander.UnitCalculation.Unit.Shield/commander.UnitCalculation.Unit.ShieldMax) * .5f);
                    done = true;
                    cooldownReward = 1;
                    kiteReward = 1;
                    done = true;
                    Console.WriteLine($"Won: h:{commander.UnitCalculation.Unit.Health}, s:{commander.UnitCalculation.Unit.Shield}");
                }

                var lowestEnemy = commander.UnitCalculation.EnemiesInRange.OrderBy(e => e.SimulatedHitpoints).FirstOrDefault();
                if (lowestEnemy != null)
                {
                    state.DistanceToLowestEnemy = Vector2.Distance(commander.UnitCalculation.Position, lowestEnemy.Position);
                }
            }
            else
            {
                done = true;
                if (ActiveUnitData.EnemyUnits.Values.Any())
                {
                    Console.WriteLine($"Lost: c: {ActiveUnitData.EnemyUnits.Values.Count}, h:{ActiveUnitData.EnemyUnits.Values.Sum(e => e.SimulatedHitpoints)}");
                }
                else
                {
                    Console.WriteLine($"Tie");
                }
            }
            return (state, endRward, cooldownReward, kiteReward, done);
        }
    }
}
