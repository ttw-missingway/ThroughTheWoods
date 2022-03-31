using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace TTW.World
{
    public class Navigator
    {
        public void ChangeRoom(string roomKeyword)
        {
            Room currentRoom = GameController.Current.CurrentRoom;
            Room newRoom = currentRoom.exits.Where(p => p.Keyword == roomKeyword).FirstOrDefault().Room;

            if (newRoom == null)
            {
                Debug.LogError("Room " + roomKeyword + " not found!");
                return;
            }

            GameController.Current.CurrentRoom = newRoom;

            LoadEnemies();
            WriteRoomDescription(newRoom);
            DrawRoom(newRoom);
        }

        private void LoadEnemies()
        {
            if (GameController.Current.CurrentRoom.enemies.Count > 0)
            {
                var images = new List<Image>();

                foreach (var e in GameController.Current.CurrentRoom.enemies)
                    images.Add(e.Image);

                UIEnemyLayer.Current.LoadEnemyImages(images);
            }
        }

        public void WriteRoomDescription(Room newRoom)
        {
            string descr = newRoom.Description;
            string exitsAre = "Exits are ";

            List<string> exits = new List<string>
            {
                descr,
                exitsAre
            };

            foreach (var e in newRoom.exits)
                exits.Add("<b>" + e.Keyword + "</b>");

            var array = exits.ToArray();

            ScreenWriter.Current.PrintToScreen(array);
        }

        private static void DrawRoom(Room newRoom)
        {
            if (newRoom.graphic != null)
            {
                ScreenWriter.Current.DrawGraphic(newRoom.graphic);
            }
        }
    }
}
