using System.Collections.Generic;
using TTW.Systems;

namespace TTW.Combat
{
    public class Ability
    {
        public List<TargetingClass> TargetTypes { get; set; }
        public TargetScope TargetingMode { get; set; }
        public List<Targetable> CurrentTargets { get; set; } //list of targets that will receive this ability
        public List<Targetable> AffectedTargets { get; set; } //list of targets that have already been affected by this ability
        public bool Damaging { get; set; }
        public RangeType Range { get; set; }
        public int ChannelTime { get; set; }
        public int ExhaustTime { get; set; }
        public AbilityData AbilityData { get; set; }
        public AbilityType AbilityType { get; set; }
        public float Accuracy { get; set; }
        public bool NoMiss { get; set; }
        public bool ChangeStance { get; set; }
        public Stance Stance { get; set; }
        public MagicType MagicType { get; set; }
        public StatusEffect StatusEffect { get; set; }
        public int StatusEffectDuration { get; set; }
        public bool Reposition { get; set; }
        public Movement Movement { get; set; }
        public int MovementDegree { get; set; }
        public AttackVariants AttackVariant { get; set; }
        public Creatable Creation { get; set; }
        public ObstacleData Obstacle { get; set; }
        public Combatant Sender { get; set; }
        public AidType AidType { get; set; }
        public bool AoO { get; set; } 

        public Ability (AbilityData data, Combatant sender)
        {
            CurrentTargets = new List<Targetable>();
            AffectedTargets = new List<Targetable>();
            TargetTypes = data.TargetTypes;
            TargetingMode = data.TargetingMode;
            Damaging = data.Damaging;
            Range = data.RangeType;
            ChannelTime = data.ChannelTime;
            ExhaustTime = data.ExhaustTime;
            AbilityData = data;
            AbilityType = data.AbilityType;
            Accuracy = data.Accuracy;
            NoMiss = data.NoMiss;
            ChangeStance = data.ChangeStance;
            Stance = data.Stance;
            MagicType = data.MagicType;
            StatusEffect = data.StatusEffect;
            StatusEffectDuration = data.statusEffectDuration;
            Reposition = data.Reposition;
            Movement = data.Movement;
            AttackVariant = data.AttackVariant;
            Creation = data.Creation;
            Obstacle = data.Obstacle;
            AidType = data.AidType;
            AoO = false;
            Sender = sender;
        }

        public void AddAffectedTarget(Targetable target){
            AffectedTargets.Add(target);
        }

        public void AttackOfOpportunity(){
            AoO = true;
        }

        public void ClearTargets(){
            CurrentTargets.Clear();
        }

        public void AddTarget(Targetable target){
            CurrentTargets.Add(target);
        }
    }
}
