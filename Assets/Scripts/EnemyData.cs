using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace TTW.Systems
{
    [CreateAssetMenu(fileName = "newEnemy", menuName = "EnemyData", order = 4)]
    public class EnemyData : ScriptableObject
    {
        public string Name;
        public Stats Stats;
        public Image Image;
    }
}
