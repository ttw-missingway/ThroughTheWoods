using System.Collections.Generic;
using System.Linq;

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

        public IList<T> SwitchPositions<T>(IList<T> list, int posA, int posB){
            (list[posB], list[posA]) = (list[posA], list[posB]);

            return list;
        }

        public IList<T> ReorderPositions<T>(IList<T> list, int oldIndex, int newIndex){
            T item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);

            // for (var i = 0; i < list.Count(); i++){
            //     list[i].SetPositionOrder(list, i+1);
            // }

            return list;
        }
    }
}
