using TTW.Systems;
using UnityEngine;

namespace TTW.Combat{
    public class TargetableLink : MonoBehaviour
    {
        [SerializeField] ActorData _data;
        LinkLibrary _linkLibrary;
        LinkLibrary.LinkClass _linkClass;
        Targetable targetable;

        private void Start()
        {
            _linkLibrary = LinkLibrary.Current;
            targetable = GetComponent<Targetable>();
            SetLinkClass();
            RegisterLinkInfo();
            SendLinkInfo();
        }

        private void SetLinkClass()
        {
            if (targetable.Position.PlayingFieldSide == CombatSide.Ally)
                _linkClass = LinkLibrary.LinkClass.Ally;
            else if (targetable.Position.PlayingFieldSide == CombatSide.Enemy)
                _linkClass = LinkLibrary.LinkClass.Enemy;
        }

        private void RegisterLinkInfo()
        {
            _linkLibrary.AddLink(_data.Keyword, _linkClass, _data);
        }

        private void SendLinkInfo(){
            if (GetComponent<ClickableImage>() != null){
                GetComponent<ClickableImage>().SetLinkId(_data.Keyword);
            }
            GetComponent<Targetable>().SetKeyword(_data.Keyword);
        }
    }
}