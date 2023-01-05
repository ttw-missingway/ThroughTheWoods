using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TTW.Systems
{
    [CreateAssetMenu(fileName = "new Obstacle", menuName = "Obstacle")]
    public class ObstacleData : ScriptableObject
    {
        public string Name;
        public string Keyword;
    }
}
