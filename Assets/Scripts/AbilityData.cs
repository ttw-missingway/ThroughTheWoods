using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;
using TTW.Combat;

namespace TTW.Systems
{
    [CreateAssetMenu(fileName ="newAbility", menuName = "Ability", order = 5)]
    public class AbilityData : ScriptableObject
    {
        public string Name;
        public string Description;
        public AbilityFX abilityFX;

        public int ChannelTime;
        public int ExhaustTime;

        [EnumPaging, OnValueChanged("SetCurrentTool")]
        public AbilityType AbilityType;

        private bool showTType => AbilityType != AbilityType.Create && AbilityType != AbilityType.Stance;
        private bool showTMode => AbilityType == AbilityType.Attack || AbilityType == AbilityType.Aid;
        private bool showAid => AbilityType == AbilityType.Aid;

        [ShowIf(nameof(showTType))]
        public List<TargetType> TargetTypes;

        [ShowIf(nameof(showTMode))]
        public TargetingMode TargetingMode;

        [ShowIf(nameof(showTType))]
        [PropertyRange(0, 10)]
        public int RangeMin;

        [ShowIf(nameof(showTType))]
        [PropertyRange(0, 10)]
        public int RangeMax;

        [ShowIf(nameof(showTType))]
        [PropertyRange(0, 100)]
        public float Accuracy;

        [ShowIf(nameof(showTType))]
        public bool NoMiss;

        public bool ChangeNeutralState;
        [ShowIf(nameof(ChangeNeutralState))]
        public Stance NeutralState;

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

        public bool ChangeStatsSelf;

        [ShowIf(nameof(ChangeStatsSelf))]
        public Stat StatSelf;
        [ShowIf(nameof(ChangeStatsSelf))]
        public int StatSelfDegree;

        [ShowIf(nameof(showTMode))]
        public bool ChangeStatsTarget;

        [ShowIf(nameof(ChangeStatsTarget))]
        public Stat StatTarget;
        [ShowIf(nameof(ChangeStatsTarget))]
        public int StatTargetDegree;

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

        [ShowIf("Creation", Creatable.Trap)]
        public TrapData Trap;

        [ShowIf("Creation", Creatable.Enemy)]
        public EnemyData Enemy;

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
        Signal,
        Create,
        Stance,
        Movement
    }

    public enum AidType
    {
        Heal,
        Revive,
        Dispel
    }
    public enum TargetType
    {
        None,
        Enemy,
        Actor,
        Obstacle,
        Self,
        Down,
        Untargetable
    }
    public enum TargetingMode
    {
        None,
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
        Enraged, //can't change targets
        Stunned, //can't move or take an action
        Burned, //chance of taking wound every action
        Wounded, //unable to perform certain actions
        Down, //must be revived, cannot be used in any way
        Madness, //performs abilities randomly
        Blind, //abilities are more likely to miss
        Trapped, //can perform abilities but cannot move
        Asleep, //cannot perform abilities or move until awoken either by chance or being hit
        Bubble, //protected from damage until bubble pops
        Genera, //chance of healing player every action
        Mirror, //reflects magic back to the caster
        Angel, //is automatically brought back to life once
        Trance, //magic is cast without channel time
        Mute, //cannot signal, cannot cast magic
        Phase, //cannot be targeted (can still be trapped, caught in area of effect or global attacks, hurt through status
        Invulnerable, //cannot be hurt or killed, can still sustain damage effects, traps, etc.
        Blur //evasion is raised;
    }
    public enum Stance
    {
        None,
        Alert, //will attempt to evade damage, goes off gait
        Guarding, //will attempt to block incoming damage, goes off grit
        Vulnerable, //will not defend if attacked, is not aware of communication attempts
        Countering, //will defend with specific attack if attacked directly, is not aware of communication attempts
        Signalling, //attempting to communicate, is on the lookout for communication
        Coordinating, //is actively in communication with another actor, may be alerted if attacked
        Protecting //takes damage as a proxy for whoever they are guarding
    }
    public enum Stat
    {
        None,
        Heart,
        Gait,
        Grit,
        Mind,
        Spirit
    }
    public enum Movement
    {
        //magic used in conjunction with movement will override gait limitations and result in a "warp"
        None,
        Advance,
        Retreat
    }
    public enum Creatable
    {
        None,
        Trap,
        Enemy,
        Obstacle
    }
    public enum AttackVariants
    {
        Deadly, //has the potential to instantly kill
        GuardBreak, //will instantly kill a target who is guarding or protecting
        ChannelBreak, //will instantly kill a target who is channeling
        Vampire, //will heal a wounded attacker upon successful attack
        QuickAttack, //may take no action time (cannot occur successively)
    }
}
