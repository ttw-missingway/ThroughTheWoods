using System.Collections;
using System.Collections.Generic;
using TMPro;
using TTW.Combat;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TTW.Systems{
    public class ClickableText : MonoBehaviour, IPointerClickHandler
    {
        LinkLibrary library;
        PlayerController pc;

        private void Start(){
            library = CombatManager.Current.LinkLibrary;
            pc = GameObject.FindObjectOfType<PlayerController>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var text = GetComponent<TMPro.TextMeshProUGUI>();
            if (eventData.button == PointerEventData.InputButton.Left){
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
                if (linkIndex > -1){
                    
                    var linkInfo = text.textInfo.linkInfo[linkIndex];
                    var linkId = linkInfo.GetLinkID();
                    print(linkId);
                    var itemData = library.Get(linkId);

                    pc.ReceiveLink(itemData);
                }
            }
        }
    }
}