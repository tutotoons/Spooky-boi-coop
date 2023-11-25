using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowSequenceInteractionListener : BaseSequenceInteractionListener
{
    [SerializeField] private GameObject gameObjectToHideShow;

    public override void Animate(bool _state)
    {
        if (currentState == _state)
        {
            return;
        }

        gameObjectToHideShow.SetActive(_state);
        currentState = _state;
    }
}
