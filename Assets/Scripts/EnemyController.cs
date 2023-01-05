using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TTW.Systems
{
    public class EnemyController : MonoBehaviour
    {
        public DecisionData currentDecision;
        public PlayerController playerController;
        public EnemyData enemy;

        private void Start()
        {
            playerController.EndOfPlayerTurn += OnEndOfPlayerTurn;
            DecisionLoop();
        }

        private void DecisionLoop()
        {
            WriteNarration(currentDecision);
            playerController.ToggleCommandPhase();
        }

        private void OnEndOfPlayerTurn(object sender, EventArgs e)
        {
            playerController.ToggleCommandPhase();
            PerformAction(currentDecision);
            currentDecision = currentDecision.DefaultPath;
            DecisionLoop();
        }

        private void WriteNarration(DecisionData currentDecision)
        {
            print(currentDecision.Narration);
        }

        private void PerformAction(DecisionData decision){
            var target = playerController.Actors[decision.ActionToPerform[0].Position];
            var action = decision.ActionToPerform[0].Action;
            print($"{enemy.name} is performing {action.name} on {target}");
            target.Health.ReceiveAction(action);
        }
    }
}
