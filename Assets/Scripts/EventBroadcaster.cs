using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat {
    public class EventBroadcaster : MonoBehaviour
    {
        public static EventBroadcaster Current;
        [SerializeField] CombatManager _manager;
        [SerializeField] ChannelingSurrogate _channeler;
        public ChannelingSurrogate Channeler => _channeler;
        public event EventHandler EndTurn;
        public event EventHandler StartTurn;
        public event EventHandler EndAction;
        private bool _endOfActionFlag = false;
        public bool EndOfActionFlag => _endOfActionFlag;

        private void Awake(){
            Current = this;
        }

        private void EndEventPhase()
        {
            _manager.ChangeState(CombatState.Control);
        }

        public void ReadyForNewActionChain(bool inProgress){
            _endOfActionFlag = inProgress;
        }

        public void StartEventPhase()
        {
            ReduceExhaustAndChannel();

            if (!CheckForEvents())
            {
                EndEventPhase();
                CallStartTurn();
            }
            else
            {
                _channeler.CallChannel(_manager.Turn);
            }
        }

        private void ReduceExhaustAndChannel()
        {
            if (_manager.Turn == CombatSide.Ally)
            {
                foreach (Combatant c in _manager.Allies)
                {
                    c.ReduceChannelTime();
                    c.ReduceExhaustTime();
                }
            }
            else
            {
                foreach (Combatant c in _manager.Enemies)
                {
                    c.ReduceChannelTime();
                    c.ReduceExhaustTime();
                }
            }
        }

        private bool CheckForEvents()
        {
           if (_channeler.CheckForEvents(_manager.Turn)) return true;
            return false;
        }

        public void EndOfAnimation(){
            if (_manager.State == CombatState.Event){
                if (_channeler.EndOfChannel(_manager.Turn)){
                        EndEventPhase();
                    } 
            }
            else{
                if (_endOfActionFlag){
                    _manager.ChangeState(CombatState.Control);
                    CallEndOfAction();
                }
            }
            if (_manager.Turn == CombatSide.Ally){
                GetComponent<CombatReader>().PC.StartOfTurn();
            }
        }

        public void CallEndOfAction(){
            EndAction?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndTurn(){
            EndTurn?.Invoke(this, EventArgs.Empty);
        }

        public void CallStartTurn(){
            StartTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}