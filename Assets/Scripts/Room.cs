using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;
using TTW.Systems;

namespace TTW.World
{
    [CreateAssetMenu(fileName = "NewRoom", menuName = "Room", order = 1)]
    public class Room : ScriptableObject
    {
        public Graphic graphic;

        public List<Exit> exits = new List<Exit>();

        [Title("Description", bold: false)]
        [HideLabel]
        [MultiLineProperty(5)]
        public string Description;

        public List<Thing> things = new List<Thing>();

        public List<EnemyData> enemies = new List<EnemyData>();

        [System.Serializable]
        public struct Exit
        {
            public string Keyword;
            public Room Room;
        }
    }
}
