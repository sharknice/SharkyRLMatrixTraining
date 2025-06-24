using RLMatrix.Agents.Common;
using RLMatrix;
using SharkyRLMatrixTraining;
using Sharky;

Console.WriteLine("Starting Sharky Micro Training");

var learningSetup = new PPOAgentOptions(
    batchSize: 8,
    ppoEpochs: 8,
    memorySize: 1000,
    gamma: 0.99f,
    width: 128,
    entropyCoefficient: 0.01f,
    lr: 1E-02f
);

// Create our environment
var environment = new SharkyTrainingEnvironment(UnitTypes.PROTOSS_STALKER, UnitTypes.ZERG_ROACH, 3).RLInit(maxStepsSoft: 1200, maxStepsHard: 1200);
var env = new List<IEnvironmentAsync<float[]>> {
    environment
};

// Create our learning agent
var agent = new LocalDiscreteRolloutAgent<float[]>(learningSetup, env);

// Let it learn!
for (int i = 0; i < 100000; i++)
{
    await agent.Step();
}

// Ends the StarCraft 2 game and saves the replay
await environment.CloseEnvironment();

Console.WriteLine("\nTraining complete!");
Console.ReadLine();