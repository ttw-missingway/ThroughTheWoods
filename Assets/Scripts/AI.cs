using System.Collections;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;
using System.Linq;

namespace TTW.Combat
{
    public class AI : MonoBehaviour
    {
        [SerializeField] List<AbilityData> _abilities = new List<AbilityData>();
        Combatant _combatant;
        Health _health;
        CombatInstance _combatInstance;

        private void Start()
        {
            _health = GetComponent<Health>();
            _combatant = GetComponent<Combatant>();
            _combatInstance = CombatInstance.Current;
            _combatInstance.OnActionEnd += _combatInstance_OnActionEnd;
        }

        private void _combatInstance_OnActionEnd(object sender, System.EventArgs e)
        {
            if (_health.StatusExists(StatusEffect.Down))
            {
                Destroy(gameObject);
                return;
            }

            if (_combatant.Exhausted) return;
            if (_combatant.Channeling) return;

            _combatInstance.QueueUp(_combatant, TakeTurn);
        }

        public void TakeTurn()
        {
            int randomIndex = Random.Range(0, _abilities.Count);
            var allEnemies = FindObjectsOfType<Targetable>().Where(t => t.TargetType == TargetType.Actor).ToList();
            int randomEnemyIndex = Random.Range(0, allEnemies.Count);
            _combatInstance.SetFocus(_combatant);
            _combatInstance.SetAbility(_abilities[randomIndex]);
            _combatInstance.SetTarget(allEnemies[randomEnemyIndex]); //will fail when all allies are dead, just make sure game over is properly implemented
            _combatInstance.ExecuteAbility();
        }
    }
}
