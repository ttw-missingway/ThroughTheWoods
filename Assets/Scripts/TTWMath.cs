using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TTW.Systems
{
    public class TTWMath
    {
        public IEnumerable<T> Shuffle<T>(IEnumerable<T> list, int size)
        {
            var r = new System.Random();
            var shuffledList = 
                list.
                    Select(x => new { Number = r.Next(), Item = x }).
                    OrderBy(x => x.Number).
                    Select(x => x.Item).
                    Take(size);

            return shuffledList.ToList();
        }
    }
}
