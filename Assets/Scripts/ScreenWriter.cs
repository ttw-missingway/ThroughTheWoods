using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity;

namespace TTW.Systems
{
    public class ScreenWriter : MonoBehaviour
    {
        [SerializeField] RectTransform _graphicContainer;

        TMP_Text _text;
        public static ScreenWriter Current;

        void Awake()
        {
            _text = GetComponent<TMP_Text>();
            Current = this;
        }

        public void PrintToScreen(string[] textToWrite)
        {
            _text.text = "";

            foreach (var s in textToWrite)
            {
                _text.text += s + "\n";
            }
        }

        public void PrintToScreen(string textToWrite)
        {
            _text.text = "";

            _text.text = textToWrite;
        }

        public void AddToScreen(string textToWrite)
        {
            _text.text += textToWrite;
        }

        public void DrawGraphic(Graphic newGraphic)
        {
            foreach (var c in _graphicContainer.GetComponentsInChildren<Graphic>())
                Destroy(c);

            Instantiate(newGraphic, _graphicContainer);
        }
    }
}
