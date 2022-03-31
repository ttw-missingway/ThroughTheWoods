using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Systems;

namespace TTW.World
{
    public class Inventory
    {
        Actor _actor;
        int _currentSize;
        int _currentWeight;
        int _maxSize;
        int _maxWeight;
        List<Thing> _items = new List<Thing>();
        public Inventory(Actor actor, int size, int weight)
        {
            _actor = actor;
            _maxSize = size;
            _maxWeight = weight;
        }

        public void AddItem(Thing item)
        {
            if (_currentSize + item.Size > _maxSize)
                ScreenWriter.Current.PrintToScreen("There's not enough room in " + _actor.ActorData.Name + "'s bag to add " + item.Keyword + ".");
            else if(_currentWeight + item.Weight > _maxWeight)
                ScreenWriter.Current.PrintToScreen("Adding " + item.Keyword + " would make " + _actor.ActorData.Name + "'s bag too heavy to carry.");
            else
            {
                ScreenWriter.Current.PrintToScreen($"{_actor.ActorData.Name} placed {item.Keyword} in {_actor.Pronoun} bag.");
                _items.Add(item);
                _currentSize += item.Size;
                _currentWeight += item.Weight;
            }
        }

        public void ReadInventory()
        {
            if (_items.Count == 0)
            {
                ScreenWriter.Current.PrintToScreen("Nothing in inventory.");
                return;
            }

            ScreenWriter.Current.PrintToScreen(_actor.ActorData.Name + " found these items in his bag: ");

            foreach (var i in _items)
            {
                ScreenWriter.Current.AddToScreen(i.Keyword);
            }
        }

        private void RemoveItem(Thing item)
        {
            _currentSize -= item.Size;
            _currentWeight -= item.Weight;
            _items.Remove(item);
        }
        
        public Thing? SearchForItem(string item)
        {
            var foundItem = KeywordToItem(item);

            return foundItem;
        }

        public void DropItem(string item)
        {
            var foundItem = KeywordToItem(item); ;

            if(foundItem == null)
                ScreenWriter.Current.PrintToScreen(_actor.ActorData.Name + " couldn't find: " + item);
            else
            {
                RemoveItem(foundItem);
                ScreenWriter.Current.PrintToScreen(_actor.ActorData.Name + " dropped " + item + " from bag");
            }
        }

        public void UseItem(string item)
        {
            var foundItem = KeywordToItem(item);

            if (foundItem == null)
                ScreenWriter.Current.PrintToScreen(_actor.ActorData.Name + " couldn't find: " + item);
            else if (foundItem.canBeUsed)
            {
                //item.Use();
                RemoveItem(foundItem);
                ScreenWriter.Current.PrintToScreen(_actor.ActorData.Name + " uses " + item);
            }
            else
            {
                ScreenWriter.Current.PrintToScreen(item + " cannot be used.");
            }    
        }

        private Thing? KeywordToItem(string item)
        {
            var foundItems = _items.Where(i => i.Keyword == item).ToList();

            if (foundItems.Count > 0)
                return foundItems[0];
            else
                return null;
        }
    }
}
