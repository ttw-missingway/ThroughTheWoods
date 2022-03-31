using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AbilityFX : MonoBehaviour
    {
        [SerializeField] float _animationTime = 3f;
        CombatInstance _instance;

        private void Start()
        {
            _instance = CombatInstance.Current;
            StartCoroutine(Animate());
        }

        IEnumerator Animate()
        {
            float countdown = _animationTime;

            while (countdown > 0)
            {
                countdown--;
                yield return new WaitForSeconds(1f);
            }

            _instance.AbilityAnimationEnd();
            Destroy(gameObject);
        }
    }
}
