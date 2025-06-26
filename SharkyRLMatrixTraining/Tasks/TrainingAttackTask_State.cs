using Sharky.MicroTasks.Attack;
using Sharky;
using System.Numerics;

namespace SharkyRLMatrixTraining
{
    public partial class TrainingAttackTask : AdvancedAttackTask
    {
        public (float[] state, float endRward, float cooldownReward, float kiteReward, bool done) GetState()
        {
            bool done = false;
            float endRward = 0;
            var cooldownReward = 0f;
            var kiteReward = 0f;
            var state = new float[3];
            var commander = UnitCommanders.FirstOrDefault();
            if (commander != null)
            {
                state[0] = commander.UnitCalculation.Unit.WeaponCooldown;
                if (commander.UnitCalculation.Unit.WeaponCooldown > 0)
                {
                    cooldownReward = 1f;
                }

                var closestEnemy = commander.UnitCalculation.NearbyEnemies.OrderBy(e => Vector2.Distance(e.Position, commander.UnitCalculation.Position)).FirstOrDefault();
                if (closestEnemy != null)
                {
                    state[1] = Vector2.Distance(commander.UnitCalculation.Position, closestEnemy.Position);
                    if (closestEnemy.PreviousUnitCalculation != null && commander.UnitCalculation.PreviousUnitCalculation != null)
                    {
                        state[2] = Vector2.Distance(commander.UnitCalculation.Position, closestEnemy.Position) - Vector2.Distance(commander.UnitCalculation.PreviousUnitCalculation.Position, closestEnemy.PreviousUnitCalculation.Position); 
                    }
                    else
                    {
                        state[2] = 0;
                    }

                    if (commander.UnitCalculation.Unit.WeaponCooldown > 0)
                    {
                        kiteReward = Vector2.Distance(commander.UnitCalculation.Position, closestEnemy.Position) - 3f;
                    }
                }
                else
                {
                    state[1] = 0;
                    state[2] = 0;
                    endRward = (commander.UnitCalculation.Unit.Health/commander.UnitCalculation.Unit.HealthMax) + ((commander.UnitCalculation.Unit.Shield/commander.UnitCalculation.Unit.ShieldMax) * .5f);
                    done = true;
                    cooldownReward = 1;
                    kiteReward = 1;
                    done = true;
                    Console.WriteLine($"Won: h:{commander.UnitCalculation.Unit.Health}, s:{commander.UnitCalculation.Unit.Shield}");
                }
            }
            else
            {
                done = true;
                state[0] = 0;
                state[1] = 0;
                state[2] = 0;
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
