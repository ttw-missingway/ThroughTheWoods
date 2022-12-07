using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat{
    public class CreateCombatants : MonoBehaviour
    {
        [SerializeField] List<Combatant> _actors = new List<Combatant>();
        [SerializeField] List<Combatant> _enemies = new List<Combatant>();
        [SerializeField] List<Targetable> _objects = new List<Targetable>();
        GameObject _actorBin;
        GameObject _enemyBin;
        GameObject _objectBin;
        CombatManager _manager;
        BoardManager _board;

        void Awake()
        {
            _manager = CombatManager.Current;
            _board = _manager.Board;
            FindBins();
            Create();
            DestroySelf();
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }

        private void FindBins()
        {
            _actorBin = GameObject.FindGameObjectWithTag("ActorBin");
            _enemyBin = GameObject.FindGameObjectWithTag("EnemyBin");
            _objectBin = GameObject.FindGameObjectWithTag("ObjectBin");
        }

        private void Create()
        {
            Camera cam = Camera.main;
            float height = 2f * cam.orthographicSize;
            float padding = height * 0.1f;
            float width = height * cam.aspect;
            float actorSpacing = width / _actors.Count;
            float enemySpacing = width / _enemies.Count;
            Vector3 testSpacing = new Vector3(100f, 0f, 0f);
            int count = 0;

            foreach(Combatant a in _actors){
                var realA = Instantiate(a, new Vector3(actorSpacing * count, _actorBin.transform.position.y, 0f), Quaternion.identity, _actorBin.transform);
                _board.AddActor(realA);
                realA.Position.SetCombatSide(CombatSide.Ally);
                count++;
            }
            count=0;
            foreach(Combatant e in _enemies){
                var realE = Instantiate(e, new Vector3(-7.6f + (enemySpacing * count), _enemyBin.transform.position.y, 0f), Quaternion.identity, _enemyBin.transform);
                _board.AddEnemy(realE);
                realE.Position.SetCombatSide(CombatSide.Enemy);
                count++;
            }
            foreach(Targetable o in _objects){
                var realO = Instantiate(o, _objectBin.transform);
                _board.AddObject(realO);
            }
        }
    }

}