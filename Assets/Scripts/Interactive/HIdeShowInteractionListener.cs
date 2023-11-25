using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIdeShowInteractionListener : BaseToggleInteractionListener
{
    [SerializeField] private GameObject objToHideShow;

    public override void Animate(bool _state)
    {
        objToHideShow.SetActive(_state);
    }
}
