using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Systems{
    public class PlayerUnloader : MonoBehaviour
    {
        GameStats GameStats;
        public ActorEntity EntityShell;

        private void Awake(){
            GameStats = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStats>();
            GameObject playerController = GameObject.FindGameObjectWithTag("PlayerController");
            var activeActors = GameStats.AllActorStats.Where(a => a.IsActive).ToList();

            foreach (ActorStats a in activeActors){
                BuildPlayer(a, playerController.transform);
            }

            Destroy(gameObject);
        }

        private void BuildPlayer(ActorStats stats, Transform parent){
            var newPlayer = Instantiate(EntityShell, parent);
            newPlayer.BuildPlayer(stats);
            newPlayer.name = stats.ActorData.Name;
            parent.GetComponent<PlayerController>().AddActorEntity(newPlayer);
        }
    }
}
