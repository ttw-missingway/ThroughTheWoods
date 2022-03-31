using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class Position : MonoBehaviour
    {
        Dictionary<Position, float> _distances = new Dictionary<Position, float>();

        public void CombatStart(List<Position> movers)
        {
            _distances.Clear();

            foreach (var m in movers)
            {
                if (m == this) continue;

                if (m.GetComponent<Targetable>().TargetType == GetComponent<Targetable>().TargetType)
                {
                    _distances.Add(m, 1f);
                    print("distance between " + m.gameObject.name + " and " + gameObject.name + " is now " + _distances[m]);
                }
                else
                {
                    _distances.Add(m, 9f);
                    print("distance between " + m.gameObject.name + " and " + gameObject.name + " is now " + _distances[m]);
                }
            }
        }

        public float DistanceTo(Position position)
        {
            return _distances[position];
        }

        public void Advance(Position target, float degree)
        {
            float grossMovement = Mathf.Min(_distances[target] - 1f, degree);

            SingularAdvance(target, degree);

            AdjustAllPositioning(target, grossMovement);

            print("distance between " + target.gameObject.name + " and " + gameObject.name + " is now " + _distances[target]);
        }

        public void Retreat(Position target, float degree)
        {
            float grossMovement = Mathf.Min(-1f * (_distances[target] - 10f), degree);

            SingularRetreat(target, degree);

            AdjustAllPositioning(target, grossMovement);

            print("distance between " + target.gameObject.name + " and " + gameObject.name + " is now " + _distances[target]);
        }

        private void SingularAdvance(Position target, float degree)
        {
            _distances[target] -= degree;

            if (_distances[target] < 1f)
                _distances[target] = 1f;

            target.PositionParity(this, _distances[target]);
        }

        private void SingularRetreat(Position target, float degree)
        {
            _distances[target] += degree;

            if (_distances[target] > 9f)
                _distances[target] = 9f;

            target.PositionParity(this, _distances[target]);
        }

        public void PositionParity(Position mover, float distance)
        {
            _distances[mover] = distance;
        }

        private void AdjustAllPositioning(Position target, float degree)
        {
            var allPositions = FindObjectsOfType<Position>();

            foreach (var p in allPositions)
            {
                if (p.gameObject == gameObject) continue;
                if (p.gameObject == target.gameObject) continue;

                if (p.DistanceTo(this) + DistanceTo(target) < p.DistanceTo(target)  ||
                    DistanceTo(target) + p.DistanceTo(target) < p.DistanceTo(this)  ||
                    p.DistanceTo(target) + p.DistanceTo(this) < DistanceTo(target))
                        _distances[p] = Mathf.Clamp(Mathf.Abs(p.DistanceTo(target) - DistanceTo(target)), 1f, 9f);

                p.PositionParity(this, _distances[p]);

                print("distance between " + p.gameObject.name + " and " + gameObject.name + " is now " + _distances[p]);
            }
        }

        private void AdjustAllPositioningRetreat(Position target, float degree)
        {
            var allPositions = FindObjectsOfType<Position>();

            foreach (var p in allPositions)
            {
                if (p.gameObject == gameObject) continue;
                if (p.gameObject == target.gameObject) continue;

                if (p.DistanceTo(this) + DistanceTo(target) < p.DistanceTo(target))
                    _distances[p] = p.DistanceTo(target) - DistanceTo(target);

                p.PositionParity(this, _distances[p]);

                print("distance between " + p.gameObject.name + " and " + gameObject.name + " is now " + _distances[p]);
            }
        }
    }
}