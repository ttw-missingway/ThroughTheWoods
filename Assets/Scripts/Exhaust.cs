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

    //INITIALIZE
    public Exhaust (CombatSide combatSide)
    {
        _combatSide = combatSide;
        _eventBroadcaster = CombatManager.Current.EventBroadcaster;
        
        if (_combatSide == CombatSide.Ally)
            _eventBroadcaster.StartTurnAlly += StartTurn;
        if (_combatSide == CombatSide.Enemy)
            _eventBroadcaster.StartTurnEnemy += StartTurn;
    }

    //UPDATE
    private void StartTurn(object sender, EventArgs e)
    {
        SubtractFromCountDown(1);
    }

    public void SetExhaust(int time){
        SetCountDown(time);
        _exhausted = true;
    }

    internal override void CountDownEnd()
    {
        _exhausted = false;
    }
}
