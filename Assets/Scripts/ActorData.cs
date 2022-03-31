using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Combat;
using UnityEngine;

namespace TTW.Systems
{
    public enum ActorType
    {
        Art,
        Aisling,
        Nicolo,
        Rim,
        Kazan,
        Krusk,
        Freyja,
        Giles,
        Rusalka,
        Unit,
        Whisperwill,
        Kanaloa
    }

    [Serializable]
    public struct Stats
    {
        public int Heart { get; set; }
        public int Mind { get; set; }
        public int Gait { get; set; }
        public int Grit { get; set; }
        public int Spirit { get; set; }

        public Stats(int heart, int mind, int gait, int grit, int spirit)
        {
            Heart = heart;
            Mind = mind;
            Gait = gait;
            Grit = grit;
            Spirit = spirit;
        }
    }

    [CreateAssetMenu(fileName = "newActor", menuName = "ActorData", order = 3)]
    public class ActorData : ScriptableObject
    {
        public ActorType ActorType;
        public string Name;
        public Stats Stats;

        public int BagSize;
        public string Pronoun;
    }
}
