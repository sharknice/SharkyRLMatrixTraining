using Sharky.DefaultBot;
using Sharky.MicroControllers;
using Sharky.Pathing;
using Sharky;

namespace SharkyRLMatrixTraining
{
    public class TrainingIndividualMicroController : IndividualMicroController
    {
        public TrainingIndividualMicroController(DefaultSharkyBot defaultSharkyBot, IPathFinder pathFinder, MicroPriority microPriority, bool groupUpEnabled, float avoidDamageDistance = 2, bool ignoreDistractions = true) : base(defaultSharkyBot, pathFinder, microPriority, groupUpEnabled, avoidDamageDistance, ignoreDistractions)
        {
        }

        public List<SC2APIProtocol.Action> Attack(UnitCommander commander, int frame)
        {
            var enemy = commander.UnitCalculation.NearbyEnemies.FirstOrDefault();
            if (enemy != null)
            {
                return commander.Order(frame, Abilities.ATTACK, targetTag: enemy.Unit.Tag, allowSpam: true);
            }

            return commander.Order(frame, Abilities.ATTACK, TargetingData.AttackPoint);
        }


        public List<SC2APIProtocol.Action> Retreat(UnitCommander commander, int frame)
        {
            var enemy = commander.UnitCalculation.NearbyEnemies.FirstOrDefault();
            if (enemy != null)
            {
                var avoidPoint = GetGroundAvoidPoint(commander, commander.UnitCalculation.Unit.Pos, enemy.Unit.Pos, TargetingData.AttackPoint, TargetingData.ForwardDefensePoint, enemy.Range + enemy.Unit.Radius + commander.UnitCalculation.Unit.Radius + 4);
                return commander.Order(frame, Abilities.MOVE, avoidPoint);
            }

            return commander.Order(frame, Abilities.MOVE, TargetingData.ForwardDefensePoint);
        }
    }
}
