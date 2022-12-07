// DEPRECATED CLASS

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using System;
// using System.Linq;
// using TTW.Systems;

// namespace TTW.Combat{

//     public class CombatReader : MonoBehaviour
//     {
//         public static CombatReader Singleton;

//         CombatWriter writer;
//         CombatManager _combatManager;
//         [SerializeField] TMP_InputField _inputField;
//         [SerializeField] PlayerController _pc;

//         public PlayerController PC => _pc;

//         public void Awake(){
//             Singleton = this;
//             writer = GetComponent<CombatWriter>();
//             _combatManager = GetComponent<CombatManager>();
//             _inputField.onEndEdit.AddListener(AcceptStringInput);
//         }

//         private void AcceptStringInput(string userInput)
//         {
//             userInput = userInput.ToLower();
//             ParseCommand(userInput);
//             InputComplete();
//         }

//         private void ParseCommand(String userInput)
//         {
//             string[] strArray = userInput.Split(" ");

//             if (GenericCommand(strArray)) return;
//             if (ActorCommand(strArray)) return;

//             print("command not recognized!");
//         }

//         private bool GenericCommand(string[] strArray)
//         {
//             bool stopParsing = true;

//             switch (strArray[0])
//             {
//                 case "end":
//                         break;
//                 case "actors":
//                         writer.WriteAvailableCombatants(_combatManager.Allies);
//                         break;
//                 case "help":
//                         writer.Write("generic commands: ");
//                         writer.Write("end, actors, help, ready-actors, run, enemies, objects");
//                         writer.Write("generic actor command:");
//                         writer.Write(" 'actor's name' <space> abilities");
//                         writer.Write(" 'actor's name' <space> look");
//                         writer.Write(" 'actor's name' <space> status");
//                         writer.Write(" 'actor's name' <space> items");
//                         writer.Write("actor ability command syntax: ");
//                         writer.Write(" 'actor's name' <space> 'actor's ability' <space> 'ability's target'");
//                         break;
//                 case "run":
//                         print("run");
//                         break;
//                 default: 
//                         stopParsing = false;
//                         break;
//             }

//             return stopParsing;
//         }

//         private bool ActorCommand(string[] strArray)
//         {
//             bool stopParsing = true;
//             CheckingTool cTool = new CheckingTool();
//             var selectableActors = cTool.GetAvailableAllies(_combatManager.Allies);
//             var allActors = _combatManager.Allies;
//             Combatant actor = allActors.Where(a => a.Actor.Name.ToLower() == strArray[0]).FirstOrDefault();

//             if (actor == null){
//                 stopParsing = false;
//                 return stopParsing;
//             }

//             if (ActorStatusCommand(actor, strArray)) return stopParsing;
//             if (GenericActorCommand(actor, strArray)) return stopParsing;
//             if (AbilityCommand(actor, strArray)) return stopParsing;
            
//             return stopParsing;
//         }

//         private bool ActorStatusCommand(Combatant actor, string[] strArray){
//             bool stopParsing = true;

//             if (strArray.Length == 1){
//                 print(actor.Actor.Name + " status");
//             }
//             else{
//                 stopParsing = false;
//             }

//             return stopParsing;
//         }

//         private bool AbilityCommand(Combatant actor, string[] strArray)
//         {
//             bool stopParsing = true;

//             TargetingTool tTool = new TargetingTool();
//             AbilityData selectedAbility = actor.Abilities.Where(a => a.Name.ToLower() == strArray[1]).FirstOrDefault();

//             if (selectedAbility == null){
//                 stopParsing = false;
//                 return stopParsing;
//             }

//             print("combatant: " + actor);
//             print("ability: " + selectedAbility);

//             // _pc.SetActor(actor);
//             // _pc.SetAbility(selectedAbility);

//             if (selectedAbility.TargetingMode == TargetScope.Random || selectedAbility.TargetingMode == TargetScope.Global){
//                 // _pc.Execute();
//                 return false;
//             }

//             if (selectedAbility.TargetTypes.Contains(TargetingClass.Self) && strArray.Length < 3){
//                 _pc.SetTargetSelf();
//                 // _pc.Execute();
//                 return false;
//             }

//             if (strArray.Length < 3){
//                 print("target is required");
//                 return false;
//             }

//             List<Targetable> targetables = tTool.FilterTargetables(actor, selectedAbility, false);
//             Targetable selectedTarget = targetables.Where(t => t.Name.ToLower() == strArray[2]).FirstOrDefault();

//             if (selectedTarget == null){
//                 print("target doesn't exist or is not targetable");
//                 return false;
//             }

//             print("target: " + selectedTarget);

//             // _pc.SetActor(actor);
//             // _pc.SetAbility(selectedAbility);
//             // _pc.SetTarget(selectedTarget);
//             // _pc.Execute();

//             return stopParsing;
//         }

//         private bool GenericActorCommand(Combatant actor, string[] strArray)
//         {
//             bool stopParsing = true;

//             switch (strArray[1]){
//                 case "abilities":
//                     foreach (AbilityData a in actor.Abilities){
//                         writer.Write(a.Name);
//                     }
//                     break;
//                 case "look":
//                     print(actor.Actor.Name + " " + "look");
//                     break;
//                 case "items":
//                     print(actor.Actor.Name + " " + "items");
//                     break;
//                 case "status":
//                     foreach (Status s in actor.GetComponent<Health>().StatusEffects){
//                         writer.Write(s.StatusEffect.ToString());
//                     }
//                     break;
//                 default:
//                     stopParsing = false;
//                     break;
//             }

//             return stopParsing;
//         }

//         private void InputComplete()
//         {
//             _inputField.ActivateInputField();
//             _inputField.text = null;
//         }

//         private KeyCode[] keyCodes = {
//          KeyCode.Alpha1,
//          KeyCode.Alpha2,
//          KeyCode.Alpha3,
//          KeyCode.Alpha4,
//          KeyCode.Alpha5,
//          KeyCode.Alpha6,
//          KeyCode.Alpha7,
//          KeyCode.Alpha8,
//          KeyCode.Alpha9,
//          KeyCode.Q,
//          KeyCode.W,
//          KeyCode.E,
//          KeyCode.R,
//          KeyCode.T,
//          KeyCode.Y
//         };
//     }
// }