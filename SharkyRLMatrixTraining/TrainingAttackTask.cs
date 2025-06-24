using SC2APIProtocol;
using Sharky.DefaultBot;
using Sharky.MicroTasks.Attack;
using Sharky;
using System.Numerics;

namespace SharkyRLMatrixTraining
{
    public class TrainingAttackTask : AdvancedAttackTask
    {
        TrainingIndividualMicroController TrainingIndividualMicroController;
        MacroData MacroData;
        ActiveUnitData ActiveUnitData;

        public TrainingAttackTask(DefaultSharkyBot defaultSharkyBot, EnemyCleanupService enemyCleanupService, List<UnitTypes> mainAttackerTypes, float priority, bool enabled, TrainingIndividualMicroController trainingIndividualMicroController) : base(defaultSharkyBot, enemyCleanupService, mainAttackerTypes, priority, enabled)
        {
            TrainingIndividualMicroController = trainingIndividualMicroController;
            MacroData = defaultSharkyBot.MacroData;
            ActiveUnitData = defaultSharkyBot.ActiveUnitData;
        }

        public override IEnumerable<SC2APIProtocol.Action> PerformActions(int frame)
        {
            return new List<SC2APIProtocol.Action>();
        }


        public IEnumerable<SC2APIProtocol.Action> Attack()
        {
            var actions = new List<SC2APIProtocol.Action>();

            foreach (var commander in UnitCommanders)
            {
                actions.AddRange(TrainingIndividualMicroController.Attack(commander, MacroData.Frame));
                actions.Add(GetCameraAction(commander));
            }

            return actions;
        }

        private SC2APIProtocol.Action GetCameraAction(UnitCommander commander)
        {
            return new SC2APIProtocol.Action
            {
                ActionRaw = new ActionRaw
                {
                    CameraMove = new ActionRawCameraMove()
                    {
                        CenterWorldSpace = commander.UnitCalculation.Unit.Pos
                    }
                }
            };
        }

        public IEnumerable<SC2APIProtocol.Action> Retreat()
        {
            var actions = new List<SC2APIProtocol.Action>();

            foreach (var commander in UnitCommanders)
            {
                actions.AddRange(TrainingIndividualMicroController.Retreat(commander, MacroData.Frame));
                actions.Add(GetCameraAction(commander));
            }

            return actions;
        }

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

                var enemy = commander.UnitCalculation.NearbyEnemies.FirstOrDefault();
                if (enemy != null)
                {
                    state[1] = Vector2.Distance(commander.UnitCalculation.Position, enemy.Position);
                    if (enemy.PreviousUnitCalculation != null && commander.UnitCalculation.PreviousUnitCalculation != null)
                    {
                        state[2] = Vector2.Distance(commander.UnitCalculation.Position, enemy.Position) - Vector2.Distance(commander.UnitCalculation.PreviousUnitCalculation.Position, enemy.PreviousUnitCalculation.Position); 
                    }
                    else
                    {
                        state[2] = 0;
                    }

                    if (commander.UnitCalculation.Unit.WeaponCooldown > 0)
                    {
                        kiteReward = Vector2.Distance(commander.UnitCalculation.Position, enemy.Position) - 3f;
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
                var enemy = ActiveUnitData.EnemyUnits.Values.FirstOrDefault();
                if (enemy != null)
                {
                    Console.WriteLine($"Lost: h:{enemy.Unit.Health}");
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
