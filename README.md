# SharkyRLMatrixTraining
Advanced reinforcement learning for StarCraft 2 unit micro using C# .NET
A Sharky https://github.com/sharknice/Sharky implementation of RL_Matrix https://github.com/asieradzk/RL_Matrix

## Setup
1. Install StarCraft 2 https://starcraft2.blizzard.com/en-us/
2. Clone this repo https://github.com/sharknice/SharkyRLMatrixTraining
3. Copy MicroTraining.SC2Map from the Maps folder into your StarCraft 2 maps folder (C:\Program Files (x86)\StarCraft II\Maps). Create the Maps folder if it doesn't already exist.
4. (Optional) Run RLMatrix.Dashboard https://github.com/asieradzk/RL_Matrix to display training progress in your web browser
5. Build and run SharkyRLMatrixTraining.  It will launch StarCraft 2 and begin training. Refresh your web browser to connect to the session if running RLMatrix.Dashboard

The default setup trains a Protoss Stalker to kite against a Zerg Roach. It should maximize firing rate and distance from the roach.
