using System;
using TTW.Combat;
using UnityEngine;

[System.Serializable]
public class Channel : Countdown
{
    [SerializeField]
    private bool _channeling = false;

    private CombatSide _combatSide;

    EventBroadcaster _eventBroadcaster;
    Combatant _combatant;
    Ability _channeledAbility;

    public int ChannelTime => _turns;
    public bool Channeling => _channeling;

    public Channel (CombatSide combatSide, Combatant combatant)
    {
        _combatSide = combatSide;
        _combatant = combatant;
        _eventBroadcaster = CombatManager.Current.EventBroadcaster;
        
        if (_combatSide == CombatSide.Ally)
            _eventBroadcaster.StartTurnAlly += StartTurn;
        if (_combatSide == CombatSide.Enemy)
            _eventBroadcaster.StartTurnEnemy += StartTurn;
    }

    public void StartChannel(int channelTime, Ability channeledAbility)
    {
        
        if (channelTime == 0) return;

        _channeling = true;
        _channeledAbility = channeledAbility;

        SetCountDown(channelTime);
    }

    private void StartTurn(object sender, EventArgs e)
    {
        SubtractFromCountDown(1);
    }

    internal override void CountDownEnd()
    {
        _combatant.SendAbility(_channeledAbility);
        _channeling = false;
    }

    public void ChannelBreak()
    {
        _channeling = false;
        _turns = 0;
    }
}
