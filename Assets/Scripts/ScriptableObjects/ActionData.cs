using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
// using TTW.Combat;

namespace TTW.Systems
{
    [CreateAssetMenu(fileName ="newAction", menuName = "Action", order = 5)]
    public class ActionData : ScriptableObject
    {
        public string Name;
        public string Keyword;
        public Archetype Archetype;
        public StatusEffect StatusEffect;
        public string Description;
        // public AbilityFX AbilityFX;
        public int Duration;
        public bool Damaging;
        public float BaseLikelihood;
        public bool SpeedFactor;
        public bool SizeFactor;
        public bool StrengthFactor;
        public bool MagicFactor;
        public bool IntellectFactor;
        public bool SightFactor;
        public bool HearingFactor;
        public bool SmellFactor;
        public bool MagicResFactor;
        public bool MentalResFactor;
        public bool PhysResFactor;
        public bool TrapResFactor;
        public bool WeatherResFactor;
        public bool GroundResFactor;
        public bool WaterResFactor;
        public bool AirResFactor;
        public bool LampResFactor;
        public bool SunResFactor;
        public bool MoonResFactor;
        public bool ElecResFactor;


        // [EnumPaging, OnValueChanged("SetCurrentTool")]
        // public ActionType AbilityType;

        // private bool showTMode => AbilityType == AbilityType.Attack || AbilityType == AbilityType.Aid;
        // private bool showAid => AbilityType == AbilityType.Aid;
        // private bool attackMode => AbilityType == AbilityType.Attack;
        // private bool showAccuracy => RangeType == RangeType.Ranged;

        // public List<TargetingClass> TargetTypes;

        // [ShowIf(nameof(showTMode))]
        // public TargetScope TargetingMode;

        // [ShowIf(nameof(attackMode))]
        // public RangeType RangeType;

        // [PropertyRange(0, 100)]
        // public float Accuracy;

        // public bool NoMiss;

        // [ShowIf(nameof(showTMode))]
        // public bool Damaging;

        // public bool ChangeStance;
        // [ShowIf(nameof(ChangeStance))]
        // public Stance Stance;

        // [ShowIf(nameof(showAid))]
        // public AidType AidType;

        // public bool IsMagical;
        // [ShowIf(nameof(IsMagical))]
        // public MagicType MagicType;

        // [ShowIf(nameof(showTMode))]
        // public StatusEffect StatusEffect;

        // bool statusEffectChecked => StatusEffect != StatusEffect.None;

        // [ShowIf(nameof(statusEffectChecked))]
        // public int statusEffectDuration;

        // [ShowIf(nameof(showTMode))]
        // public bool ChangeStatsTarget;

        // public bool Reposition;
        // [ShowIf(nameof(Reposition))]
        // public Movement Movement;

        // [ShowIf(nameof(showTMode))]
        // public bool TrySignal;

        // [ShowIf("AbilityType", AbilityType.Attack)]
        // public AttackVariants AttackVariant;

        // [ShowIf("AbilityType", AbilityType.Create)]
        // public Creatable Creation;

        // [ShowIf("Creation", Creatable.Obstacle)]
        // public ObstacleData Obstacle;

        // private void SetCurrentTool()
        // {
        //     if (AbilityType == AbilityType.Movement)
        //         Reposition = true;
        //     else
        //         Reposition = false;
        // }
    }

    public enum ActionType
    {
        Offensive,
        Defensive,
        Aid,
        Passive,
        Social,
        Creation
    }

    public enum TargetingClass
    {
        Enemy,
        Ally,
        Self,
        AllAllies,
        AllEnemies
    }
    public enum MagicType
    {
        None,
        Sunlight,
        Moonlight,
        Lamplight,
        Electriclight,
        Umbra
    }
    public enum StatusEffect
    {
        None,
        Berserk, //can't change targets
        Stunned, //can't move or take an action
        Burned, //can't heal past wounded
        Wounded, //unable to perform certain actions
        Madness, //performs abilities randomly
        Blind, //abilities are more likely to miss
        Bound, //can perform abilities but cannot move
        Asleep, //cannot perform abilities or move until awoken either by chance or being hit
        Bubble, //protected from an instance of damage
        Genera, //auto heals wounds at start of turn
        Mirror, //reflects magic back to the caster
        Angel, //is automatically brought back to life once
        Trance, //special state, evasion, damage block, no cast times
        Mute, //cannot signal, cannot cast magic
        Phase, //cannot be targeted (can still be trapped, caught in area of effect or global attacks, hurt through status
        IronClad, //cannot be hurt or killed, can still sustain damage effects, traps, etc.
        Blur, //evasion is raised;
        Isolated, //cannot be signalled to
        Disarmed, //reduced to base abilities
        Atrophy, //cooldowns and channels are longer
        Dhyana, //always alert
        Tailwind, //fast cooldowns
        Homing, //won't miss
        Behemoth //melee attacks become deadly
    }

    public enum Creatable
    {
        None,
        Obstacle,
        Trap,
        Weather,
        Environment
    }
}
