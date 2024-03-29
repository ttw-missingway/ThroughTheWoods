using System;
using TTW.Combat;
using UnityEngine;

[System.Serializable]
public class Exhaust : Countdown
{
    [SerializeField]
    private bool _exhausted = false;
    [SerializeField]
    private CombatSide _combatSide;

    //COMPONENTS
    EventBroadcaster _eventBroadcaster;

    //GET
    public int ExhaustTime => _turns;
    public bool Exhausted => _exhausted;

    bool _turnOfCast = false;

    //INITIALIZE
    public Exhaust (CombatSide combatSide)
    {
        _combatSide = combatSide;
        _eventBroadcaster = CombatManager.Current.EventBroadcaster;
        
        if (_combatSide == CombatSide.Ally)
            _eventBroadcaster.StartOfAlliesTurn += StartTurn;
        if (_combatSide == CombatSide.Enemy)
            _eventBroadcaster.StartOfEnemiesTurn += StartTurn;
    }

    //UPDATE
    private void StartTurn(object sender, EventArgs e)
    {
        if (_turnOfCast == true){
            _turnOfCast = false;
            return;
        }

        SubtractFromCountDown(1);
    }

    public void SetExhaust(int time){
        if (time == 0) return;
        
        SetCountDown(time);
        _exhausted = true;
        _turnOfCast = true;
    }

    internal override void CountDownEnd()
    {
        _exhausted = false;
    }
}
