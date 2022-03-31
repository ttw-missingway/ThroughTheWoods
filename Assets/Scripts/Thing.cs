using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.World
{
    [CreateAssetMenu(fileName = "NewThing", menuName = "Thing", order = 2)]
    public class Thing : ScriptableObject
    {
        public string Keyword;

        [Title("Look", bold: false)]
        [HideLabel]
        public bool canLookAt;
        public string[] lookDescription;

        [Title("Touch", bold: false)]
        [HideLabel]
        public bool canTouch;
        public string[] touchDescription;

        public bool canTake;
        public int Size;
        public int Weight;

        public bool canBeUsed;

        //public bool canMove;

        //public bool canClimb;

        //public bool canBreak;
    }
}