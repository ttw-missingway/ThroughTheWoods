using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Systems{
    public class ActorEntity : MonoBehaviour
    {
        [SerializeField]
        private ActorData _actor;

        [SerializeField]
        private ActorStats _stats;

        private Health _health;

        //API
        public ActorData Actor => _actor;
        public ActorStats Stats => _stats;
        public Health Health => _health;

        public void BuildPlayer(ActorStats stats){
            _actor = stats.ActorData;
            _stats = stats;
            _health = GetComponent<Health>();
        }
    }
}
