using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Systems{
    public class Health : MonoBehaviour
    {
        public List<StatusEffect> Statuses;

        public void ReceiveAction(ActionData action)
        {
            if (action.StatusEffect != StatusEffect.None){
                Statuses.Add(action.StatusEffect);
            }
        }
    }
}