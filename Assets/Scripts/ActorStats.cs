using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Systems
{
    [Serializable]
    public struct ActorStats 
    {
        public ActorData ActorData;

        public bool IsActive;
        public bool IsUnlocked;

        public Archetype PrimaryArchetype;
        public Archetype SecondaryArchetype;
        public List<ArchetypeEntity> AllArchetypes;
        public List<ActionData> Actions;
    }

    [Serializable]
    public struct ArchetypeEntity{
        public Archetype Archetype;
        public int Level;
        public float Experience;
        public float Integrity;

        public void LevelUp(){
            Level++;
        }
    }
}