using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Combat;
using TTW.World;
using UnityEngine;

namespace TTW.Systems
{

    //FLAGGED FOR DELETION

    public class CommandManager
    {
        //GLOBAL COMMANDS
        //WORLD COMMANDS
        //DIALOGUE COMMANDS
        //COMBAT COMMANDS
        List<string> combatCommands = new List<string>();
        //ROOM COMMANDS
        List<string> roomCommands = new List<string>();
        List<string> worldCommands = new List<string>();
        //CAST COMMANDS
        List<string> castCommands = new List<string>();
        
        public CommandManager()
        {
            if (GameController.Current.CurrentState == GameState.World)
            {
                RoomCommands();
                WorldCommands();
                CastCommands();
            }
            else if (GameController.Current.CurrentState == GameState.Combat)
            {
                CastCommands();
            }
        }

        private void CastCommands()
        {
            foreach (var a in GameController.Current.ActorsInParty)
            {
                castCommands.Add(a.ActorData.Name.ToLower());
            }

            castCommands.Add("party");
        }

        private void WorldCommands()
        {
            worldCommands.Add("look");
            worldCommands.Add("touch");
            worldCommands.Add("exits");
            worldCommands.Add("engage");
        }

        private void RoomCommands()
        {
            foreach (var r in GameController.Current.CurrentRoom.exits)
                roomCommands.Add(r.Keyword.ToLower());
        }

        public void NewCommand(string[] command)
        {
            //if (roomCommands.Contains(command[0]))
            //    ChangeRoom(command[0]);

            //if (worldCommands.Contains(command[0]))
            //    WorldAction(command);

            //if (castCommands.Contains(command[0]))
            //    CastAction(command);

            var actorCommands = LoadActorCommands();
            //var roomCommands = LoadRoomCommands();

            //switch (command[0])
            //{
            //    case ""
            //}
        }

        private List<string> LoadActorCommands()
        {
            var actorStrings = new List<string>();

            //foreach (var s in GameController.Current.ActorsInParty)
            //    actorStrings.Add(s.Name.ToLower());

            return actorStrings;
        }

        private List<string> LoadAbilityCommands(string actorString)
        {
            var abilityStrings = new List<string>();

            //Actor actor = GameController.Current.ActorsInParty.Where(a => a.Name.ToLower() == actorString.ToLower()).FirstOrDefault();

            //foreach (var a in actor.Abilities)
                //abilityStrings.Add(a.Name.ToLower());

            return abilityStrings;
        }

        public void ActorCommand(string actor, string command)
        {
            var abilityCommands = LoadAbilityCommands(actor);

        }

        private void ChangeRoom(string roomKeyword)
        {
            var nav = new Navigator();
            nav.ChangeRoom(roomKeyword);
        }

        private void CastAction(string[] keywords)
        {
            if (keywords[0] == "party")
            {
                ScreenWriter.Current.PrintToScreen("The party contains the following actors: ");

                foreach (var a in GameController.Current.ActorsInParty)
                {
                    ScreenWriter.Current.AddToScreen(a.ActorData.Name);
                }
            }

            if (keywords.Length == 1) return;

            var actor = GameController.Current.ActorsInParty.Where(a => a.ActorData.Name.ToLower() == keywords[0]).FirstOrDefault();

            switch (keywords[1])
            {
                case "items":
                case "bag":
                case "inventory":
                    actor.Inventory.ReadInventory();
                    break;
                case "drop":
                    if (keywords.Length == 2) return;
                    actor.Inventory.DropItem(keywords[2]);
                    break;
                case "use":
                    if (keywords.Length == 2) return;
                    actor.Inventory.UseItem(keywords[2]);
                    break;
                case "take":
                    if (keywords.Length == 2) return;
                    var worldexp = new WorldExplorer();
                    worldexp.TakeThing(actor, keywords[2]);
                    break;
                default:
                    break;
            }
        }

        private void WorldAction(string[] keywords)
        {
            //if (keywords[0] == "look")
            //{
            //    var worldExp = new WorldExplorer();

            //    if (keywords.Length > 1)
            //        worldExp.LookAtThing(keywords[1]);
            //    else
            //        worldExp.LookAtThing("");
            //}
            //if (keywords[0] == "touch")
            //{
            //    var worldExp = new WorldExplorer();

            //    if (keywords.Length > 1)
            //        worldExp.TouchThing(keywords[1]);
            //    else
            //        worldExp.TouchThing("");
            //}
            //if (keywords[0] == "exits")
            //{
            //    var nav = new Navigator();
            //    nav.WriteRoomDescription(GameController.Current.CurrentRoom);
            //}
            //if (keywords[0] == "engage")
            //{
            //    if (GameController.Current.CurrentRoom.enemies.Count > 0)
            //    {
            //        var com = new CombatInstance(GameController.Current.ActorsInParty, GameController.Current.CurrentRoom.enemies);
            //        ScreenWriter.Current.PrintToScreen("The party has engaged in combat! ");
            //        GameController.Current.CurrentState = GameState.Combat;
            //    } 
            //}
        }
    }
}
