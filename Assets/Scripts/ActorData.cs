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
