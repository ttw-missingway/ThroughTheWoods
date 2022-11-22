using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Combat;
using UnityEngine;

namespace TTW.Systems
{

    [CreateAssetMenu(fileName = "newActor", menuName = "ActorData", order = 3)]
    public class ActorData : ScriptableObject
    {
        public string Name;
        public string Keyword;
    }
}
