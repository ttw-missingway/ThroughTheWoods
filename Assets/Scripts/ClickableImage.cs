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

        public void Start(){
            library = GameObject.FindGameObjectWithTag("Master").GetComponent<LinkLibrary>();
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