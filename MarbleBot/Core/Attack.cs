namespace MarbleBot.Core
{
    /// <summary> Represents a boss' attack during a Marble Siege. </summary>
    public readonly struct Attack
    {
        /// <summary> The name of the attack. </summary>
        public string Name { get; }
        /// <summary> The amount of damage dealt by the attack. </summary>
        public int Damage { get; }
        /// <summary> The chance the attack will hit each marble out of 100. </summary>
        public int Accuracy { get; }
        /// <summary> The status effect the attack inflicts. </summary>
        public StatusEffect StatusEffect { get; }

        /// <summary> An empty instance of an attack. </summary>
        public static Attack Empty => new Attack("", 0, 0, StatusEffect.None);

        /// <summary> Represents a boss' attack during a Marble Siege. </summary>
        /// <param name="name"> The name of the attack. </param>
        /// <param name="damage"> The amount of damage dealt by the attack. </param>
        /// <param name="accuracy"> The chance the attack will hit each marble out of 100. </param>
        /// <param name="statusEffect"> The status effect the attack inflicts. </param>
        public Attack(string name, int damage, int accuracy, StatusEffect statusEffect)
        {
            Name = name;
            Damage = damage;
            Accuracy = accuracy;
            StatusEffect = statusEffect;
        }
    }
}