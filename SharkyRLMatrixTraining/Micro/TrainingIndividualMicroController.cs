using Sharky.DefaultBot;
using Sharky.MicroControllers;
using Sharky.Pathing;
using Sharky;
using System.Numerics;
using Sharky.Extensions;

namespace SharkyRLMatrixTraining
{
    public class TrainingIndividualMicroController : IndividualMicroController
    {
        public TrainingIndividualMicroController(DefaultSharkyBot defaultSharkyBot, IPathFinder pathFinder, MicroPriority microPriority, bool groupUpEnabled, float avoidDamageDistance = 2, bool ignoreDistractions = true) : base(defaultSharkyBot, pathFinder, microPriority, groupUpEnabled, avoidDamageDistance, ignoreDistractions)
        {
        }

        public List<SC2APIProtocol.Action> AttackClosest(UnitCommander commander, int frame)
        {
            UnitCalculation? enemy = GetClosestEnemy(commander);
            if (enemy != null)
            {
                return commander.Order(frame, Abilities.ATTACK, targetTag: enemy.Unit.Tag, allowSpam: true);
            }

            return commander.Order(frame, Abilities.ATTACK, TargetingData.AttackPoint);
        }

        public List<SC2APIProtocol.Action> RetreatFromClosest(UnitCommander commander, int frame)
        {
            var enemy = GetClosestEnemy(commander);
            if (enemy != null)
            {
                var avoidPoint = GetGroundAvoidPoint(commander, commander.UnitCalculation.Unit.Pos, enemy.Unit.Pos, TargetingData.AttackPoint, TargetingData.ForwardDefensePoint, enemy.Range + enemy.Unit.Radius + commander.UnitCalculation.Unit.Radius + 4);
                return commander.Order(frame, Abilities.MOVE, avoidPoint);
            }

            return commander.Order(frame, Abilities.MOVE, TargetingData.ForwardDefensePoint);
        }

        public List<SC2APIProtocol.Action>MoveToClosest(UnitCommander commander, int frame)
        {
            UnitCalculation? enemy = GetClosestEnemy(commander);
            if (enemy != null)
            {
                return commander.Order(frame, Abilities.MOVE, enemy.Position.ToPoint2D(), allowSpam: true);
            }

            return commander.Order(frame, Abilities.MOVE, TargetingData.AttackPoint);
        }

        private static UnitCalculation? GetClosestEnemy(UnitCommander commander)
        {
            return commander.UnitCalculation.NearbyEnemies.OrderBy(e => Vector2.Distance(e.Position, commander.UnitCalculation.Position)).FirstOrDefault();
        }

        private static UnitCalculation? GetLowestHitpointsEnemy(UnitCommander commander)
        {
            return commander.UnitCalculation.NearbyEnemies.OrderBy(e => e.SimulatedHitpoints).FirstOrDefault();
        }

        public List<SC2APIProtocol.Action> AttackLowestHitpoints(UnitCommander commander, int frame)
        {
            UnitCalculation? enemy = GetLowestHitpointsEnemy(commander);
            if (enemy != null)
            {
                return commander.Order(frame, Abilities.ATTACK, targetTag: enemy.Unit.Tag, allowSpam: true);
            }

            return commander.Order(frame, Abilities.ATTACK, TargetingData.AttackPoint);
        }

        public List<SC2APIProtocol.Action> MoveToLowestHitpoints(UnitCommander commander, int frame)
        {
            UnitCalculation? enemy = GetLowestHitpointsEnemy(commander);
            if (enemy != null)
            {
                return commander.Order(frame, Abilities.MOVE, enemy.Position.ToPoint2D(), allowSpam: true);
            }
            return commander.Order(frame, Abilities.MOVE, TargetingData.AttackPoint);
        }
    }
}
