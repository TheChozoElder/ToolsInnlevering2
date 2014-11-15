namespace innlevering2.Model
{
    public class StatsObject
    {
        public string Name { get; set; }

        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float ScaleZ { get; set; }

        public float MaxHealth { get; set; }
        public float Health { get; set; }
        public float RegenerateSpeed { get; set; }
        public bool Invincible { get; set; }

        public float MovementSpeed { get; set; }
        public float TurningSpeed { get; set; }
        public float AimingSpeed { get; set; }
    }
}