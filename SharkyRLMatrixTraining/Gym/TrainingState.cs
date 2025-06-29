namespace SharkyRLMatrixTraining
{
    public class TrainingState
    {
        public TrainingState() 
        {
            UnitState = new UnitState();
            EndReward = 0f;
            CooldownReward = 0f;
            KiteReward = 0f;
            Done = false;
        }

        public UnitState UnitState { get; set; }
        public float EndReward { get; set; }
        public float CooldownReward { get; set; }
        public float KiteReward { get; set; }
        public bool Done { get; set; }
    }
}
