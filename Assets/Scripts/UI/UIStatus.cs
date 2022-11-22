using System.Collections;
using System.Collections.Generic;
using TMPro;
using TTW.Combat;
using TTW.Systems;
using UnityEngine;

namespace TTW.UI
{
    public class UIStatus : MonoBehaviour
    {
        TMP_Text tmp;
        Status _status;
        public Status Status => _status;

        [SerializeField] StatusEffect _statusEffect;
        private void Awake()
        {
            tmp = GetComponent<TMP_Text>();
        }
        public void SetStatus(Status status)
        {
            _status = status;
            _statusEffect = _status.StatusEffect;

            switch (status.StatusEffect)
            {
                case StatusEffect.None:
                    Debug.Log("Status Effect of type NONE created!");
                    break;
                // case StatusEffect.Enraged:
                //     tmp.text = "#";
                //     tmp.color = Color.red;
                //     break;
                case StatusEffect.Stunned:
                    tmp.text = "{}";
                    tmp.color = Color.yellow;
                    break;
                case StatusEffect.Burned:
                    tmp.text = "*";
                    tmp.color = Color.red + Color.yellow;
                    break;
                case StatusEffect.Wounded:
                    tmp.text = "!";
                    tmp.color = Color.red;
                    break;
                case StatusEffect.Down:
                    tmp.text = "X";
                    tmp.color = Color.black;
                    break;
                case StatusEffect.Madness:
                    tmp.text = "%";
                    tmp.color = Color.blue + Color.red;
                    break;
                case StatusEffect.Blind:
                    tmp.text = "--";
                    tmp.color = Color.gray;
                    break;
                // case StatusEffect.Trapped:
                //     tmp.text = "@";
                //     tmp.color = Color.green;
                //     break;
                case StatusEffect.Asleep:
                    tmp.text = "Z";
                    tmp.color = Color.blue;
                    break;
                case StatusEffect.Bubble:
                    tmp.text = "O";
                    tmp.color = Color.cyan;
                    break;
                case StatusEffect.Genera:
                    tmp.text = "+";
                    tmp.color = Color.green;
                    break;
                case StatusEffect.Mirror:
                    tmp.text = "()";
                    tmp.color = Color.magenta;
                    break;
                case StatusEffect.Angel:
                    tmp.text = "^";
                    tmp.color = Color.yellow;
                    break;
                case StatusEffect.Trance:
                    tmp.text = "&";
                    tmp.color = Color.red + Color.blue;
                    break;
                case StatusEffect.Mute:
                    tmp.text = "...";
                    tmp.color = Color.black;
                    break;
                case StatusEffect.Phase:
                    tmp.text = "~";
                    tmp.color = Color.cyan;
                    break;
                // case StatusEffect.Invulnerable:
                //     tmp.text = "$";
                //     tmp.color = Color.gray;
                //     break;
                default:
                    break;
            }
        }


    }
}