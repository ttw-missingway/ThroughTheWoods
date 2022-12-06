using System.Collections.Generic;
using TTW.Combat;

public class AoORequest
{
    Targetable _requestee;
    Targetable _target;
    bool _alert = false;
    public bool Alert => _alert;
    
    List<Targetable> _alliesRequested = new List<Targetable>();

    public AoORequest(Targetable requestee, Targetable target){
        _requestee = requestee;
        _target = target;
        _alliesRequested.Add(requestee);
    }

    public void SetAlert(){
        _alert = true;
    }

    public void AddRequested(Targetable requested){
        _alliesRequested.Add(requested);
    }

    public bool AlreadyRequested(Targetable requested){
        return _alliesRequested.Contains(requested);
    }

    public Targetable Target => _target;
    public Targetable Requestee => _requestee;
}
