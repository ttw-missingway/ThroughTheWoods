using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Systems;

namespace TTW.Combat
{
    public class Ability
    {
        public float RangeMin { get; set; }
        public float RangeMax { get; set; }
        public int ChannelTime { get; set; }
        public int ExhaustTime { get; set; }
        public AbilityData AbilityData { get; set; }
        public AbilityType AbilityType { get; set; }
        public float Accuracy { get; set; }
        public bool NoMiss { get; set; }
        public bool ChangeNeutralState { get; set; }
        public Stance NeutralState { get; set; }
        public bool IsMagical { get; set; }
        public MagicType MagicType { get; set; }
        public StatusEffect StatusEffect { get; set; }
        public int StatusEffectDuration { get; set; }
        public bool ChangeStatsSelf { get; set; }
        public Stat StatSelf { get; set; }
        public int StatSelfDegree { get; set; }
        public bool ChangeStatsTarget { get; set; }
        public Stat StatTarget { get; set; }
        public int StatTargetDegree { get; set; }
        public bool Reposition { get; set; }
        public Movement Movement { get; set; }
        public int MovementDegree { get; set; }
        public bool TrySignal { get; set; }
        public AttackVariants AttackVariant { get; set; }
        public Creatable Creation { get; set; }
        public TrapData Trap { get; set; }
        public EnemyData Enemy { get; set; }
        public ObstacleData Obstacle { get; set; }
        public Targetable? Sender { get; set; }
        public int SendCount { get; set; }
        public AidType AidType { get; set; }

        public Ability (AbilityData data, Combatant sender)
        {
            RangeMin = data.RangeMin;
            RangeMax = data.RangeMax;
            ChannelTime = data.ChannelTime;
            ExhaustTime = data.ExhaustTime;
            AbilityData = data;
            AbilityType = data.AbilityType;
            Accuracy = data.Accuracy;
            NoMiss = data.NoMiss;
            ChangeNeutralState = data.ChangeNeutralState;
            NeutralState = data.NeutralState;
            IsMagical = data.IsMagical;
            MagicType = data.MagicType;
            StatusEffect = data.StatusEffect;
            StatusEffectDuration = data.statusEffectDuration;
            ChangeStatsSelf = data.ChangeStatsSelf;
            StatSelf = data.StatSelf;
            StatSelfDegree = data.StatSelfDegree;
            ChangeStatsTarget = data.ChangeStatsTarget;
            StatTarget = data.StatTarget;
            StatTargetDegree = data.StatTargetDegree;
            Reposition = data.Reposition;
            Movement = data.Movement;
            MovementDegree = data.MovementDegree;
            TrySignal = data.TrySignal;
            AttackVariant = data.AttackVariant;
            Creation = data.Creation;
            Trap = data.Trap;
            Enemy = data.Enemy;
            Obstacle = data.Obstacle;
            AidType = data.AidType;
            SendCount = 1;

            if (sender.GetComponent<Targetable>() != null)
                Sender = sender.GetComponent<Targetable>();
            else
                Sender = null;
        }
    }
}
