using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Systems;

namespace TTW.World
{
    public enum Direction
    {
        South,
        North,
        East,
        West,
        SouthEast,
        SouthWest,
        NorthEast,
        NorthWest,
        Up,
        Down
    }

    public class WorldExplorer
    {
        ScreenWriter _screenWriter;
        GameController _gameController;

        public WorldExplorer()
        {
            _screenWriter = ScreenWriter.Current;
            _gameController = GameController.Current;
        }

        public void TouchThing(string thing)
        {
            if (thing == "")
            {
                string[] descr = { "What would you like to touch?" };
                ScreenWriter.Current.PrintToScreen(descr);
                return;
            }
            else
            {
                var t = GameController.Current.CurrentRoom.things.Where(t => t.canLookAt == true && t.Keyword == thing).FirstOrDefault();

                if (t == null)
                {
                    string[] descr = { "There's no " + thing + " to touch." };
                    ScreenWriter.Current.PrintToScreen(descr);
                }
                else
                {
                    ScreenWriter.Current.PrintToScreen(t.lookDescription);
                }
            }
        }

        public void LookAtThing(string thing)
        {
            if (thing == "")
            {
                var nav = new Navigator();
                nav.WriteRoomDescription(GameController.Current.CurrentRoom);
                return;
            }
            else
            {
                var t = GameController.Current.CurrentRoom.things.Where(t => t.canTouch == true && t.Keyword == thing).FirstOrDefault();

                if (t == null)
                {
                    string[] descr = { "There's no " + thing + " to look at." };
                    ScreenWriter.Current.PrintToScreen(descr);
                }
                else
                {
                    ScreenWriter.Current.PrintToScreen(t.touchDescription);
                }
            }
        }

        public void TakeThing(Actor actor, string thing)
        {
            var t = GameController.Current.CurrentRoom.things.Where(t => t.canTouch == true && t.Keyword == thing).FirstOrDefault();

            if (t == null)
            {
                ScreenWriter.Current.PrintToScreen("There's no " + thing + " to take.");
                return;
            }
                
            if (!t.canTake)
            {
                ScreenWriter.Current.PrintToScreen(t.Keyword + " can't be taken.");
                return;
            }

            actor.Inventory.AddItem(t);
        }

        public void LookInDirection(string dir)
        {

        }
    }
}
