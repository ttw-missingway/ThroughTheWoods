using System.Collections;
using System.Collections.Generic;
using TTW.Combat;
using TTW.Systems;
using UnityEngine;

namespace TTW.World
{   
    public class Actor : Health
    {
        //FLAGGED FOR DELETION

        public ActorType ActorType { get; }
        public ActorData ActorData { get; }
        public Inventory Inventory { get; }
        public string Pronoun { get; set; }

        //public Actor(ActorData actorData):base(actorData.name, actorData.Stats)
        //{
        //    ActorData = actorData;
        //    var stats = new Stats(actorData.Stats.Heart, actorData.Stats.Mind, actorData.Stats.Gait, actorData.Stats.Grit, actorData.Stats.Spirit);
        //    ActorType = actorData.ActorType;
        //    Inventory = new Inventory(this, actorData.BagSize, stats.Grit * 10);
        //    Pronoun = actorData.Pronoun;
        //}
    }
}