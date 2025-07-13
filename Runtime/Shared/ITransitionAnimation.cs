using  com.ez.engine.foundation.animation;
using UnityEngine;

namespace com.ez.engine.manager.ui
{
    public interface ITransitionAnimation : IAnimation
    {
        void SetPartner(RectTransform partnerRectTransform);
        
        void Setup(RectTransform rectTransform);
    }
}
