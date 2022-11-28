using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TTW.Combat;

namespace TTW.Systems
{
    [CreateAssetMenu(fileName ="newAbility", menuName = "Ability", order = 5)]
    public class AbilityData : ScriptableObject
    {
        public string Name;
        public string Keyword;
        public string Description;
        public AbilityFX abilityFX;

        public int ChannelTime;
        public int ExhaustTime;

        [EnumPaging, OnValueChanged("SetCurrentTool")]
        public AbilityType AbilityType;

        private bool showTMode => AbilityType == AbilityType.Attack || AbilityType == AbilityType.Aid;
        private bool showAid => AbilityType == AbilityType.Aid;
        private bool attackMode => AbilityType == AbilityType.Attack;
        private bool showAccuracy => RangeType == RangeType.Ranged;

        public List<TargetingClass> TargetTypes;

        [ShowIf(nameof(showTMode))]
        public TargetScope TargetingMode;

        [ShowIf(nameof(attackMode))]
        public RangeType RangeType;

        [PropertyRange(0, 100)]
        public float Accuracy;

        public bool NoMiss;

        [ShowIf(nameof(showTMode))]
        public bool Damaging;

        public bool ChangeStance;
        [ShowIf(nameof(ChangeStance))]
        public Stance Stance;

        [ShowIf(nameof(showAid))]
        public AidType AidType;

        public bool IsMagical;
        [ShowIf(nameof(IsMagical))]
        public MagicType MagicType;

        [ShowIf(nameof(showTMode))]
        public StatusEffect StatusEffect;

        bool statusEffectChecked => StatusEffect != StatusEffect.None;

        [ShowIf(nameof(statusEffectChecked))]
        public int statusEffectDuration;

        [ShowIf(nameof(showTMode))]
        public bool ChangeStatsTarget;

        public bool Reposition;
        [ShowIf(nameof(Reposition))]
        public Movement Movement;

        [ShowIf(nameof(Reposition))]
        [PropertyRange(0, 10)]
        public int MovementDegree;

        [ShowIf(nameof(showTMode))]
        public bool TrySignal;

        [ShowIf("AbilityType", AbilityType.Attack)]
        public AttackVariants AttackVariant;

        [ShowIf("AbilityType", AbilityType.Create)]
        public Creatable Creation;

        [ShowIf("Creation", Creatable.Obstacle)]
        public ObstacleData Obstacle;

        private void SetCurrentTool()
        {
            if (AbilityType == AbilityType.Movement)
                Reposition = true;
            else
                Reposition = false;
        }
    }

    public enum AbilityType
    {
        Attack,
        Aid,
        Effect,
        Create,
        Stance,
        Movement
    }

    public enum RangeType
    {
        Melee,
        Ranged
    }

    public enum AidType
    {
        Heal,
        Revive,
        Dispel
    }

    public enum TargetingClass
    {
        Foe,
        Ally,
        Obstacle,
        Self,
        Down,
    }

    public enum TargetType{
        Combatant,
        Obstacle,
        Untargetable
    }
    public enum TargetScope
    {
        Single,
        Random,
        Area,
        Global
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
        Down, //must be revived, cannot be used in any way
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
    public enum Stance
    {
        None,
        Wait, //keeps current stance if not none, and becomes alert if none
        Alert, //will attempt to evade damage, goes off gait
        Guarding, //will attempt to block incoming damage
        Countering, //will defend with specific attack if attacked directly, is not aware of communication attempts
        Protecting, //takes damage as a proxy for whomever they are guarding
        Cloaked, //becomes untargetable, can still be hurt by area, global, or status damage
    }
    public enum Movement
    {
        //magic used in conjunction with movement will override gait limitations and result in a "warp"
        None,
        Advance,
        Retreat,
        Reposition,
        Swap
    }
    public enum Creatable
    {
        None,
        Enemy,
        Obstacle
    }
    public enum AttackVariants
    {
        None,
        Deadly, //has the potential to instantly kill
        GuardBreak, //will instantly kill a target who is guarding or protecting
        ChannelBreak, //will instantly kill a target who is channeling
        Vampire, //will heal a wounded attacker upon successful attack
    }
}
