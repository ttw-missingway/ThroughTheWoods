using System;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat
{
    public class CombatManager : MonoBehaviour
    {
        //SINGLETON
        public static CombatManager Current;

        //FIELDS
        [SerializeField] int _turnsCount = 0;
        [SerializeField] CombatSide _turn = CombatSide.Ally;
        [SerializeField] CombatState _combatState = CombatState.Event;
        [SerializeField] CombatWriter _combatWriter;
        [SerializeField] public AbilityData AttackOfOpportunity;
        [SerializeField] public AbilityData CounterAttack;

        //Components
        LinkLibrary _linkLibrary;
        AbilityQueue _abilityQueue;
        EventBroadcaster _eventBroadcaster;
        BoardManager _boardManager;
        
        //API
        public CombatState State => _combatState;
        public CombatSide Turn => _turn;
        public EventBroadcaster EventBroadcaster => _eventBroadcaster;
        public AbilityQueue AbilityQueue => _abilityQueue;
        public LinkLibrary LinkLibrary => _linkLibrary;
        public BoardManager Board => _boardManager;

        private void Awake()
        {
            Current = this;
            _linkLibrary = GetComponent<LinkLibrary>();
            _abilityQueue = GetComponent<AbilityQueue>();
            _eventBroadcaster = GetComponent<EventBroadcaster>();
            _boardManager = GetComponent<BoardManager>();
            _eventBroadcaster.StartOfAction += OnStartOfAction;
            _eventBroadcaster.EndOfAction += OnEndOfAction;
            _eventBroadcaster.EndOfEventPhase += OnEndOfEventPhase;
        }

        private void Start(){
            _boardManager.EstablishPositions();
            BeginCombat();
        }

        private void OnStartOfAction(object sender, EventArgs e)
        {
            _combatState = CombatState.Animation;
        }

        private void OnEndOfEventPhase(object sender, EventArgs e)
        {
            if (Turn == CombatSide.Ally) 
                _eventBroadcaster.CallStartOfAlliesTurn();
            else
                _eventBroadcaster.CallStartOfEnemiesTurn();
        }

        private void OnEndOfAction(object sender, EventArgs e)
        {
            CheckForTurnOver();
        }

        public void CheckForTurnOver()
        {
            _combatState = CombatState.Control;

            if (_boardManager.CheckNoCombatantsRemaining(_turn)){
                PromptEndOfTurn();
            }
            else{
                PromptAction();
            }
        }

        private void PromptEndOfTurn()
        {
            _eventBroadcaster.CallEndTurnPrompt();
        }

        private void PromptAction(){
            _eventBroadcaster.CallActionPrompt();
        }

        public void BeginCombat(){
            _eventBroadcaster.StartEventPhase();
        }

        public void EndTurn(){
            if (_turn == CombatSide.Ally){
                _turn = CombatSide.Enemy;
                _eventBroadcaster.CallEndOfAlliesTurn();
            }
            else if (_turn == CombatSide.Enemy){
                _turn = CombatSide.Ally;
                _eventBroadcaster.CallEndOfEnemiesTurn();
            }
            ChangeState(CombatState.Event);
            
            _eventBroadcaster.StartEventPhase();
        }

        public void ChangeState(CombatState state){
            _combatState = state;
        }
    }
}