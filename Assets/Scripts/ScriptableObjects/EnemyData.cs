using UnityEngine;

namespace TTW.Systems
{

    [CreateAssetMenu(fileName = "newEnemy", menuName = "EnemyData", order = 6)]
    public class EnemyData : ScriptableObject
    {
        public string Name;
        public EnemyType EnemyType;
        public DecisionData FirstDecision;
        public float Speed;
        public float Size;
        public float Strength;
        public float Magic;
        public float Intellect;
        public float Sight;
        public float Hearing;
        public float Smell;
        public float MagicResistance;
        public float MentalResistance;
        public float PhysicalResistance;
        public float ResistanceToTraps;
        public float ResistanceToWeather;
        public float ResistanceToGroundEffects;
        public float ResistanceToWaterEffects;
        public float ResistanceToAirEffects;
        public float ResistanceToLampLight;
        public float ResistanceToSunLight;
        public float ResistanceToMoonLight;
        public float ResistanceToElectricLight;
    }

    public enum EnemyType
    {
        Shaytan,
        Human,
        Machine
    }
}
