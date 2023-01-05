using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Systems
{
    public class PlayerController : MonoBehaviour
    {
        public List<ActorEntity> Actors = new(); 
        public ActorEntity SelectedActor;
        public ActionData SelectedAction;
        public Health SelectedTarget;
        public Health Gevaudan;
        bool _commandPhase = false;
        public event EventHandler EndOfPlayerTurn;

        public void ToggleCommandPhase(){
            _commandPhase = !_commandPhase;
        }

        public void AddActorEntity(ActorEntity actor){
            Actors.Add(actor);
        }

        private void Update(){
            if (!_commandPhase) return;

            if (SelectedActor == null){
                if (Input.GetKeyDown(KeyCode.Q)){
                SelectedActor = Actors[0];
                }
                if (Input.GetKeyDown(KeyCode.W)){
                    SelectedActor = Actors[1];
                }
                if (Input.GetKeyDown(KeyCode.E)){
                    SelectedActor = Actors[2];
                }

                return;
            }

            if (SelectedAction == null){
                if (Input.GetKeyDown(KeyCode.Q)){
                    SelectedAction = SelectedActor.Stats.Actions[0];
                }
                if (Input.GetKeyDown(KeyCode.W)){
                    SelectedAction = SelectedActor.Stats.Actions[1];
                }

                return;
            }

            if (SelectedTarget == null){
                if (Input.GetKeyDown(KeyCode.Q)){
                    SelectedTarget = Gevaudan;
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.Space)){
                PerformAction(SelectedActor, SelectedAction, SelectedTarget);
                SelectedAction = null;
                SelectedActor = null;
                SelectedTarget = null;
                EndOfPlayerTurn?.Invoke(this, EventArgs.Empty);
            }
        }



        private void PerformAction(ActorEntity sender, ActionData action, Health target){
            print($"{sender.name} is performing {action.name} on {target.name}");
            target.ReceiveAction(action);
        }
    }
}