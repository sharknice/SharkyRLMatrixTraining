using Sharky.DefaultBot;
using Sharky.MicroTasks.Attack;
using Sharky;

namespace SharkyRLMatrixTraining
{
    public partial class TrainingAttackTask : AdvancedAttackTask
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

        public IEnumerable<SC2APIProtocol.Action> PerformAction(ActionTypes action)
        {
            switch (action)
            {
                case ActionTypes.AttackClosest:
                    return AttackClosest();
                case ActionTypes.RetreatFromClosest:
                    return RetreatFromClosest();
                case ActionTypes.MoveToClosest:
                    return MoveToClosest();
                case ActionTypes.AttackLowestHitpoints:
                    return AttackLowestHitpoints();
                case ActionTypes.MoveToLowestHitpoints:
                    return MoveToLowestHitpoints();
            }
            return AttackClosest();
        }
    }
}
