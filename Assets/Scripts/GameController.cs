using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.World;
using UnityEngine;

namespace TTW.Systems
{
    public enum GameState
    {
        Title,
        World,
        Scene,
        Dialogue,
        Menu,
        Combat
    }
    public class GameController : MonoBehaviour
    {
        public GameState CurrentState = GameState.World;
        public Room CurrentRoom;
        public ActorData Art;
        public List<Actor> ActorsInParty = new List<Actor>();
        public int LookingGlasses = 0;
        public  int Gold = 0;

        public static GameController Current;

        void Awake()
        {
            Current = this;
        }

        void Start()
        {
            var nav = new Navigator();
            nav.WriteRoomDescription(CurrentRoom);

            //var art = new Actor(Art);
            //ActorsInParty.Add(art);
        }
    }
}
