using Sharky.MicroTasks.Attack;
using Sharky;
using System.Numerics;

namespace SharkyRLMatrixTraining
{
    public partial class TrainingAttackTask : AdvancedAttackTask
    {
        public TrainingState GetState()
        {
            var trainingState = new TrainingState();
            var commander = UnitCommanders.FirstOrDefault();
            if (commander != null)
            {
                trainingState.UnitState.WeaponCooldown = commander.UnitCalculation.Unit.WeaponCooldown;
                if (commander.UnitCalculation.Unit.WeaponCooldown > 0)
                {
                    trainingState.CooldownReward = 1f;
                }

                var closestEnemy = commander.UnitCalculation.NearbyEnemies.OrderBy(e => Vector2.Distance(e.Position, commander.UnitCalculation.Position)).FirstOrDefault();
                if (closestEnemy != null)
                {
                    trainingState.UnitState.DistanceToClosestEnemy = Vector2.Distance(commander.UnitCalculation.Position, closestEnemy.Position);
                    if (closestEnemy.PreviousUnitCalculation != null && commander.UnitCalculation.PreviousUnitCalculation != null)
                    {
                        trainingState.UnitState.VelocityToClosestEnemy = Vector2.Distance(commander.UnitCalculation.Position, closestEnemy.Position) - Vector2.Distance(commander.UnitCalculation.PreviousUnitCalculation.Position, closestEnemy.PreviousUnitCalculation.Position); 
                    }

                    trainingState.UnitState.AttackersCount = commander.UnitCalculation.Attackers.Count;
                    trainingState.UnitState.EnemiesInRangeCount = commander.UnitCalculation.EnemiesInRange.Count;
                    trainingState.UnitState.EnemiesInRangeOfCount = commander.UnitCalculation.EnemiesInRangeOf.Count;
                    trainingState.UnitState.EnemiesInRangeOfAvoidCount = commander.UnitCalculation.EnemiesInRangeOfAvoid.Count;
                    trainingState.UnitState.EnemiesThreateningDamageCount = commander.UnitCalculation.EnemiesThreateningDamage.Count;
                    trainingState.UnitState.NearbyAlliesCount = commander.UnitCalculation.NearbyAllies.Count;
                    trainingState.UnitState.NearbyEnemiesCount = commander.UnitCalculation.NearbyEnemies.Count;
                    trainingState.UnitState.TargetersCount = commander.UnitCalculation.Targeters.Count;

                    if (commander.UnitCalculation.Unit.WeaponCooldown > 0)
                    {
                        trainingState.KiteReward = Vector2.Distance(commander.UnitCalculation.Position, closestEnemy.Position) - 3f;
                    }
                }
                else
                {
                    trainingState.EndReward = (commander.UnitCalculation.Unit.Health/commander.UnitCalculation.Unit.HealthMax) + ((commander.UnitCalculation.Unit.Shield/commander.UnitCalculation.Unit.ShieldMax) * .5f);
                    trainingState.Done = true;
                    trainingState.CooldownReward = 1;
                    trainingState.KiteReward = 1;
                    Console.WriteLine($"Won: h:{commander.UnitCalculation.Unit.Health}, s:{commander.UnitCalculation.Unit.Shield}");
                }

                var lowestEnemy = commander.UnitCalculation.EnemiesInRange.OrderBy(e => e.SimulatedHitpoints).FirstOrDefault();
                if (lowestEnemy != null)
                {
                    trainingState.UnitState.DistanceToLowestEnemy = Vector2.Distance(commander.UnitCalculation.Position, lowestEnemy.Position);
                }
            }
            else
            {
                trainingState.Done = true;
                if (ActiveUnitData.EnemyUnits.Values.Any())
                {
                    Console.WriteLine($"Lost: c: {ActiveUnitData.EnemyUnits.Values.Count}, h:{ActiveUnitData.EnemyUnits.Values.Sum(e => e.SimulatedHitpoints)}");
                }
                else
                {
                    Console.WriteLine($"Tie");
                }
            }
            return trainingState;
        }
    }
}
