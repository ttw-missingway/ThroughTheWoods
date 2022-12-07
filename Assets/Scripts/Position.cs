using System.Collections.Generic;
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
        public Health Health => _health;

        public void Awake(){
            _health = GetComponent<Health>();
        }

        public void SetCombatSide(CombatSide side){
            _combatSide = side;
        }

        public void SetPositionOrder(List<Position> list, int index){
            _orderNo = index + 1;
            if (index > 0){
                _leftNeighbor = list[index-1];
                Neighbors[0] = _leftNeighbor;
            }
                
            if (index < list.Count - 1){
                _rightNeighbor = list[index+1];
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
            
        }

        public void Reposition(Position targetPosition){
            
        }


        // Dictionary<Position, float> _distances = new Dictionary<Position, float>();

        // public void CombatStart(List<Position> movers)
        // {
        //     _distances.Clear();

        //     foreach (var m in movers)
        //     {
        //         if (m == this) continue;

        //         if (m.GetComponent<Targetable>().TargetType == GetComponent<Targetable>().TargetType)
        //         {
        //             _distances.Add(m, 1f);
        //         }
        //         else
        //         {
        //             _distances.Add(m, 9f);
        //         }
        //     }
        // }

        // public float DistanceTo(Position position)
        // {
        //     return _distances[position];
        // }

        // public void Advance(Position target, float degree)
        // {
        //     float grossMovement = Mathf.Min(_distances[target] - 1f, degree);

        //     SingularAdvance(target, degree);

        //     AdjustAllPositioning(target, grossMovement);

        //     CombatWriter.Singleton.Write("distance between " + target.gameObject.name + " and " + gameObject.name + " is now " + _distances[target]);
        // }

        // public void Retreat(Position target, float degree)
        // {
        //     float grossMovement = Mathf.Min(-1f * (_distances[target] - 10f), degree);

        //     SingularRetreat(target, degree);

        //     AdjustAllPositioning(target, grossMovement);

        //     CombatWriter.Singleton.Write("distance between " + target.gameObject.name + " and " + gameObject.name + " is now " + _distances[target]);
        // }

        // private void SingularAdvance(Position target, float degree)
        // {
        //     _distances[target] -= degree;

        //     if (_distances[target] < 1f)
        //         _distances[target] = 1f;

        //     target.PositionParity(this, _distances[target]);
        // }

        // private void SingularRetreat(Position target, float degree)
        // {
        //     _distances[target] += degree;

        //     if (_distances[target] > 9f)
        //         _distances[target] = 9f;

        //     target.PositionParity(this, _distances[target]);
        // }

        // public void PositionParity(Position mover, float distance)
        // {
        //     _distances[mover] = distance;
        // }

        // private void AdjustAllPositioning(Position target, float degree)
        // {
        //     var allPositions = FindObjectsOfType<Position>();

        //     foreach (var p in allPositions)
        //     {
        //         if (p.gameObject == gameObject) continue;
        //         if (p.gameObject == target.gameObject) continue;

        //         if (p.DistanceTo(this) + DistanceTo(target) < p.DistanceTo(target)  ||
        //             DistanceTo(target) + p.DistanceTo(target) < p.DistanceTo(this)  ||
        //             p.DistanceTo(target) + p.DistanceTo(this) < DistanceTo(target))
        //                 _distances[p] = Mathf.Clamp(Mathf.Abs(p.DistanceTo(target) - DistanceTo(target)), 1f, 9f);

        //         p.PositionParity(this, _distances[p]);
        //     }
        // }
    }
}