using System.Collections;
using UnityEngine;
using TMPro;

namespace TTW.Systems
{
    public class ScreenReader : MonoBehaviour
    {
        TMP_InputField _inputField;
        public static ScreenReader Current;

        void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            Current = this;
        }

        public void ReadInput()
        {
            string[] userInput = _inputField.text.Split(" ");

            if (userInput.Length == 0)
                return;

            var cmd = new CommandManager();
            cmd.NewCommand(userInput);
                
            _inputField.text = "";
            _inputField.ActivateInputField();
            _inputField.Select();
        }
    }
}