using SC2APIProtocol;
using Sharky.MicroTasks.Attack;
using Sharky;

namespace SharkyRLMatrixTraining
{
    public partial class TrainingAttackTask : AdvancedAttackTask
    {
        public IEnumerable<SC2APIProtocol.Action> AttackClosest()
        {
            var actions = new List<SC2APIProtocol.Action>();

            foreach (var commander in UnitCommanders)
            {
                actions.AddRange(TrainingIndividualMicroController.AttackClosest(commander, MacroData.Frame));
                actions.Add(GetCameraAction(commander));
            }

            return actions;
        }

        public IEnumerable<SC2APIProtocol.Action> RetreatFromClosest()
        {
            var actions = new List<SC2APIProtocol.Action>();

            foreach (var commander in UnitCommanders)
            {
                actions.AddRange(TrainingIndividualMicroController.RetreatFromClosest(commander, MacroData.Frame));
                actions.Add(GetCameraAction(commander));
            }

            return actions;
        }

        public IEnumerable<SC2APIProtocol.Action> MoveToClosest()
        {
            var actions = new List<SC2APIProtocol.Action>();

            foreach (var commander in UnitCommanders)
            {
                actions.AddRange(TrainingIndividualMicroController.MoveToClosest(commander, MacroData.Frame));
                actions.Add(GetCameraAction(commander));
            }

            return actions;
        }

        public IEnumerable<SC2APIProtocol.Action> AttackLowestHitpoints()
        {
            var actions = new List<SC2APIProtocol.Action>();
            foreach (var commander in UnitCommanders)
            {
                actions.AddRange(TrainingIndividualMicroController.AttackLowestHitpoints(commander, MacroData.Frame));
                actions.Add(GetCameraAction(commander));
            }
            return actions;
        }

        public IEnumerable<SC2APIProtocol.Action> MoveToLowestHitpoints()
        {
            var actions = new List<SC2APIProtocol.Action>();
            foreach (var commander in UnitCommanders)
            {
                actions.AddRange(TrainingIndividualMicroController.MoveToLowestHitpoints(commander, MacroData.Frame));
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
    }
}
