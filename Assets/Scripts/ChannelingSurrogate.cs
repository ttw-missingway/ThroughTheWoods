using System.Collections;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat{
    public class ChannelingSurrogate : MonoBehaviour
    {
        LinkedList<Combatant> _channelOrder = new LinkedList<Combatant>();
        LinkedList<Combatant> _channelOrderAlly = new LinkedList<Combatant>();
        LinkedList<Combatant> _channelOrderEnemy = new LinkedList<Combatant>();

        //API
        public LinkedList<Combatant> ChannelOrderAlly => _channelOrderAlly;
        public LinkedList<Combatant> ChannelOrderEnemy => _channelOrderEnemy;

        public void AddToChannelOrder(Combatant combatant){
            if (combatant.Position.PlayingFieldSide == CombatSide.Ally){
                AddToChannelOrderAlly(combatant);
            }
            else{
                AddToChannelOrderEnemy(combatant);
            }
        }
        private void AddToChannelOrderAlly(Combatant channeler){
            foreach (Combatant c in _channelOrderAlly){
                if (channeler.ChannelTime < c.ChannelTime){
                    var node = _channelOrderAlly.Find(c);
                    _channelOrderAlly.AddBefore(node, channeler);
                    return;
                }
            }

            _channelOrderAlly.AddLast(channeler);
        }
        public void AddToChannelOrderEnemy(Combatant channeler){
            print(channeler + " is channeling an ability!");
            foreach (Combatant c in _channelOrderEnemy){
                if (channeler.ChannelTime < c.ChannelTime){
                    var node = _channelOrderEnemy.Find(c);
                    _channelOrderEnemy.AddBefore(node, channeler);
                    EventBroadcaster.Current.CallEndOfAction();
                    return;
                }
            }

            EventBroadcaster.Current.CallEndOfAction();
            _channelOrderEnemy.AddLast(channeler);
        }

        public void CallChannel(CombatSide turn){
            if (turn == CombatSide.Ally){
                CallChannelAlly();
            }
            else{
                CallChannelEnemy();
            }
        }
        private void CallChannelAlly(){
            if (_channelOrderAlly.Count > 0 && _channelOrderAlly.First.Value.ChannelTime == 0){
                _channelOrderAlly.First.Value.PerformChanneledAbility();
                _channelOrderAlly.RemoveFirst();
            }
        }
        private void CallChannelEnemy(){
            if (_channelOrderEnemy.Count > 0 && _channelOrderEnemy.First.Value.ChannelTime == 0){
                _channelOrderEnemy.First.Value.PerformChanneledAbility();
                _channelOrderEnemy.RemoveFirst();
            }
        }

        public bool EndOfChannel(CombatSide turn){
            bool value = true;
            if (turn == CombatSide.Ally){
                value = EndOfChannelAlly();
            }
            else{
                value = EndOfChannelEnemy();
            }
            return value;
        }
        private bool EndOfChannelAlly(){
            if (_channelOrderAlly.Count > 0 && _channelOrderAlly.First.Value.ChannelTime == 0){
                CallChannelAlly();
                return false;
            }

            return true;
        }

        private bool EndOfChannelEnemy(){
            if (_channelOrderEnemy.Count > 0 && _channelOrderEnemy.First.Value.ChannelTime == 0){
                CallChannelEnemy();
                return false;
            }

            return true;
        }

        public bool CheckForEvents(CombatSide turn){
            if (turn == CombatSide.Ally){
                foreach(Combatant c in ChannelOrderAlly){
                    if (c.ChannelTime == 0){
                        return true;
                    }
                }
            }
            else{
                foreach(Combatant c in ChannelOrderEnemy){
                    if (c.ChannelTime == 0){
                        return true;
                    }
                }
            }

            return false;
        }
    }
}