using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat
{
    public enum CombatDistance{
        Front,
        Back
    }

    public enum CombatSide{
        Ally,
        Enemy
    }

    public class Position : MonoBehaviour
    {
        [SerializeField] CombatDistance _distance;
        [SerializeField] int _orderNo;
        [SerializeField] CombatSide _combatSide;

        [SerializeField] Position _leftNeighbor;
        [SerializeField] Position _rightNeighbor;

        public Position[] Neighbors = new Position[2];
        public CombatSide CombatSide => _combatSide;

        public CombatDistance Distance => _distance;
        public int OrderNo => _orderNo;

        Health _health;
        CombatManager _manager;
        BoardManager _board;
        public Health Health => _health;

        public void Awake(){
            _health = GetComponent<Health>();
        }

        private void Start(){
            _manager = CombatManager.Current;
            _board = _manager.Board;
        }

        public void SetCombatSide(CombatSide side){
            _combatSide = side;
        }

        public void SetPositionOrder(List<Position> list, int index)
        {
            _orderNo = index + 1;

            SetNeighbors(list, index);
        }

        private void SetNeighbors(List<Position> list, int index)
        {
            if (index > 0)
            {
                _leftNeighbor = list[index - 1];
                Neighbors[0] = _leftNeighbor;
            }

            if (index < list.Count - 1)
            {
                _rightNeighbor = list[index + 1];
                Neighbors[1] = _rightNeighbor;
            }
        }

        public void AreaAttackToNeighbors(Ability ability){
            if (_leftNeighbor != null)
                _leftNeighbor.GetComponent<Targetable>().ReceiveAbility(ability);
            if (_rightNeighbor != null)
                _rightNeighbor.GetComponent<Targetable>().ReceiveAbility(ability);
        }

        public void Advance(){
            _distance = CombatDistance.Front;
            _health.UpdateDisplay();
        }

        public void Retreat(){
            _distance = CombatDistance.Back;
            _health.UpdateDisplay();
        }

        public void Swap(Position targetPosition){
            _board.SwapPositions(_combatSide, _orderNo, targetPosition._orderNo);
        }

        public void Reposition(Position targetPosition){
            
        }
    }
}