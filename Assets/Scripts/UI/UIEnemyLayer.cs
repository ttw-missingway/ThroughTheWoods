using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTW.Systems
{
    public class UIEnemyLayer : MonoBehaviour
    {
        public static UIEnemyLayer Current;

        private void Awake()
        {
            Current = this;
        }

        public void LoadEnemyImages(List<Image> enemyImages)
        {
            foreach (var e in enemyImages)
            {
                var enem = Instantiate(e, GetComponent<RectTransform>());
            }
        }

        public void ClearEnemy(Image image)
        {
            foreach (var e in GetComponentsInChildren<Image>())
            {
                if (e == image)
                {
                    Destroy(e);
                }
            }
        }
        public void ClearAllEnemies()
        {
            foreach (var e in GetComponentsInChildren<Image>())
            {
                Destroy(e);
            }
        }
    }
}