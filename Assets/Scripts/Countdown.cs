using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Countdown
{
    //FIELD
    [SerializeField]    
    internal int _turns = 0;
    public event EventHandler OnCountdownChange;

    internal void SetCountDown(int turns){
        if (turns == 0) return;

        _turns = turns;
        OnCountdownChange?.Invoke(this, EventArgs.Empty);
    }

    internal abstract void CountDownEnd();

    internal void AddToCountDown(int turns)
    {
        _turns += turns;
        OnCountdownChange?.Invoke(this, EventArgs.Empty);
    }

    internal void SubtractFromCountDown(int turns){
        if (_turns == 0) return;

        _turns -= turns;

        if (_turns <= 0){
            _turns = 0;
            CountDownEnd();
        }

        OnCountdownChange?.Invoke(this, EventArgs.Empty);
    }
}
