using System.Collections;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat
{
    //FLAGGED FOR DELETION

    public class EnemyEntity : Health
    {
        EnemyData _enemyData;
        public List<AbilityData> Abilities { get; set; }
        public void ReceiveAbility(AbilityData abilityData)
        {
            throw new System.NotImplementedException();
        }
    }
}
