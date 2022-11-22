using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using TTW.Systems;
using UnityEditor;
using UnityEngine;

namespace TTW.Combat{

    //FLAGGED AS TEMP/TEST CLASS
    public class CombatWriter : MonoBehaviour
    {
        public static CombatWriter Singleton;
        [SerializeField] GameObject _textEngine;
        [SerializeField] CombatManager _combatManager;

        public void Awake(){
            Singleton = this;
            _combatManager = GetComponent<CombatManager>();
        }

        public void Write(string text){
            _textEngine.GetComponentInChildren<TMP_Text>().text += "\n" + text;
        }

        public void WriteAvailableCombatants(List<Combatant> availableActors)
        {
            ClearConsole();
            CombatWriter.Singleton.Write("Available Actors: ");

            for (int i = 0; i < availableActors.Count; i++)
            {
                var actor = availableActors[i].Actor;
                var name = actor.Name;
                var keyword = actor.Keyword;
                CombatWriter.Singleton.Write((i + 1) + ": " + "<link=" + keyword + "><b><color=green>" + name + "</color></b></link>");
            }
        }

        public void WriteAvailableAbilities(List<AbilityData> abilities)
        {
            ClearConsole();
            CombatWriter.Singleton.Write("Available Abilities: ");

            for (int i = 0; i < abilities.Count; i++)
            {
                var ability = abilities[i];
                var name = ability.name;
                var keyword = ability.Keyword;

                CombatWriter.Singleton.Write((i + 1) + ": " + "<link=" + keyword + "><b><color=red>" + name + "</color></b></link>");
            }
        }

        public void WriteAvailableTargets(List<Targetable> targetables)
        {
            ClearConsole();
            CombatWriter.Singleton.Write("Available Targets: ");

            for (int i = 0; i < targetables.Count; i++)
            {
                CombatWriter.Singleton.Write((i + 1) + ": " + targetables[i]);
            }
        }

        public void ClearConsole()
        {
            _textEngine.GetComponentInChildren<TMP_Text>().text = "";
        }
    }
}