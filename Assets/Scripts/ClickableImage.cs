using System;
using TMPro;
using TTW.Combat;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TTW.Systems{
    public class ClickableImage : MonoBehaviour
    {
        LinkLibrary library;
        PlayerController pc;
        string _linkId = "";

        private void Start(){
            library = CombatManager.Current.LinkLibrary;
            pc = GameObject.FindObjectOfType<PlayerController>();
        }

        public void SetLinkId(string id){
            _linkId = id;
        }

        void OnMouseDown()
        {
            var itemData = library.Get(_linkId);

            pc.ReceiveLink(itemData);
        }
    }
}