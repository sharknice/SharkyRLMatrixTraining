using SC2APIProtocol;
using Sharky.Builds;
using Sharky.DefaultBot;

namespace SharkyRLMatrixTraining
{
    public class TrainingBuildChoices
    {
        public BuildChoices MicroChoices { get; private set; }

        public TrainingBuildChoices(DefaultSharkyBot defaultSharkyBot)
        {
            var microLearningBuild = new EmptyBuild(defaultSharkyBot);

            var protossBuilds = new Dictionary<string, ISharkyBuild>
            {
                [microLearningBuild.Name()] = microLearningBuild
            };

            var microSequences = new List<List<string>>
            {
                new() { microLearningBuild.Name() },
            };

            var microBuildSequences = new Dictionary<string, List<List<string>>>
            {
                [Race.Terran.ToString()] = microSequences,
                [Race.Zerg.ToString()] = microSequences,
                [Race.Protoss.ToString()] = microSequences,
                [Race.Random.ToString()] = microSequences,
                ["Transition"] = microSequences
            };
            MicroChoices = new BuildChoices { Builds = protossBuilds, BuildSequences = microBuildSequences };
        }
    }
}
