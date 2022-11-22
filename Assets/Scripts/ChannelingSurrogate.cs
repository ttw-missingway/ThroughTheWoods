using System.Collections;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;


// //UNDER REVIEW

// namespace TTW.Combat{
//     public class ChannelingSurrogate : MonoBehaviour
//     {
//         EventBroadcaster _eventBroadcaster;
//         LinkedList<Channel> _channelOrderAlly = new LinkedList<Channel>();
//         LinkedList<Channel> _channelOrderEnemy = new LinkedList<Channel>();

//         //API
//         public LinkedList<Channel> ChannelOrderAlly => _channelOrderAlly;
//         public LinkedList<Channel> ChannelOrderEnemy => _channelOrderEnemy;

//         public void Start(){
//             _eventBroadcaster = CombatManager.Current.EventBroadcaster;
//         }

//         public void AddToChannelOrder(Channel channeler, CombatSide side){
//             if (side == CombatSide.Ally){
//                 SortChannelOrder(channeler, ref _channelOrderAlly);
//             }
//             else{
//                 SortChannelOrder(channeler, ref _channelOrderEnemy);
//             }   
//         }

//         private void SortChannelOrder(Channel channeler, ref LinkedList<Channel> channelList){
//             foreach (Channel c in channelList){
//                 if (channeler.ChannelTime < c.ChannelTime){
//                     var node = channelList.Find(c);
//                     channelList.AddBefore(node, channeler);
//                     return;
//                 }
//             }

//             channelList.AddLast(channeler);
//         }

//         // private void AddToChannelOrderAlly(Combatant channeler){
//         //     foreach (Combatant c in _channelOrderAlly){
//         //         if (channeler.ChannelTime < c.ChannelTime){
//         //             var node = _channelOrderAlly.Find(c);
//         //             _channelOrderAlly.AddBefore(node, channeler);
//         //             return;
//         //         }
//         //     }

//         //     _channelOrderAlly.AddLast(channeler);
//         // }

//         // public void AddToChannelOrderEnemy(Combatant channeler){
//         //     print(channeler + " is channeling an ability!");
//         //     foreach (Combatant c in _channelOrderEnemy){
//         //         if (channeler.ChannelTime < c.ChannelTime){
//         //             var node = _channelOrderEnemy.Find(c);
//         //             _channelOrderEnemy.AddBefore(node, channeler);
//         //             _eventBroadcaster.CallEndOfAction();
//         //             return;
//         //         }
//         //     }

//         //     _eventBroadcaster.CallEndOfAction();
//         //     _channelOrderEnemy.AddLast(channeler);
//         // }

//         public void CallChannel(CombatSide turn){
//             if (turn == CombatSide.Ally){
//                 CallChannelAlly();
//             }
//             else{
//                 CallChannelEnemy();
//             }
//         }

//         private void CallChannelAlly(){
//             if (_channelOrderAlly.Count > 0 && _channelOrderAlly.First.Value.ChannelTime == 0){
//                 _channelOrderAlly.First.Value.PerformChanneledAbility();
//                 _channelOrderAlly.RemoveFirst();
//             }
//         }

//         private void CallChannelEnemy(){
//             if (_channelOrderEnemy.Count > 0 && _channelOrderEnemy.First.Value.ChannelTime == 0){
//                 _channelOrderEnemy.First.Value.PerformChanneledAbility();
//                 _channelOrderEnemy.RemoveFirst();
//             }
//         }

//         public bool EndOfChannel(CombatSide turn){
//             bool value = true;
//             if (turn == CombatSide.Ally){
//                 value = EndOfChannelAlly();
//             }
//             else{
//                 value = EndOfChannelEnemy();
//             }
//             return value;
//         }
//         private bool EndOfChannelAlly(){
//             if (_channelOrderAlly.Count > 0 && _channelOrderAlly.First.Value.ChannelTime == 0){
//                 CallChannelAlly();
//                 return false;
//             }

//             return true;
//         }

//         private bool EndOfChannelEnemy(){
//             if (_channelOrderEnemy.Count > 0 && _channelOrderEnemy.First.Value.ChannelTime == 0){
//                 CallChannelEnemy();
//                 return false;
//             }

//             return true;
//         }

//         public bool CheckForEvents(CombatSide turn){
//             if (turn == CombatSide.Ally){
//                 foreach(Combatant c in ChannelOrderAlly){
//                     if (c.ChannelTime == 0){
//                         return true;
//                     }
//                 }
//             }
//             else{
//                 foreach(Combatant c in ChannelOrderEnemy){
//                     if (c.ChannelTime == 0){
//                         return true;
//                     }
//                 }
//             }

//             return false;
//         }
//     }
// }