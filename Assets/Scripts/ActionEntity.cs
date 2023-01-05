using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Systems{
    public class ActionEntity : MonoBehaviour
    {
        public ActionData actionData;
        public int ArchetypeLevel;
        // public List<Condition> Conditions;
        public float Likelihood;
        
    }

    public enum Archetype{
        Wheel,
        Storm,
        Oak,
        Urn,
        Coin,
        Beast,
        Sword,
        Torch,
        Spirit
    }

    // public enum Condition{
    //     Survival,
    //     Social,
    //     WoundedAlly,
    //     EscapeAvailable,

    // }
}
