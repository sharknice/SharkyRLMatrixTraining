using RLMatrix.Agents.Common;
using RLMatrix;
using SharkyRLMatrixTraining;
using Sharky;
using Sharky.MicroTasks;

Console.WriteLine("Starting Sharky Micro Training");

var learningSetup = new PPOAgentOptions(
    batchSize: 32,
    ppoEpochs: 16,
    memorySize: 1000,
    gamma: 0.99f,
    width: 128,
    entropyCoefficient: 0.01f,
    lr: 1E-02f
);

// Create our environment
var myUnits = new List<DesiredUnitsClaim> { new DesiredUnitsClaim(UnitTypes.PROTOSS_STALKER, 1) };
var enemyUnits = new List<DesiredUnitsClaim> { new DesiredUnitsClaim(UnitTypes.ZERG_ROACH, 1) };
var environment = new SharkyTrainingEnvironment(myUnits, enemyUnits, 3).RLInit(maxStepsSoft: 1200, maxStepsHard: 1200);
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