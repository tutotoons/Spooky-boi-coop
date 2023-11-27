using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhoneMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] RectTransform rect;
    [SerializeField] float animTime;

    private float timer;
    private bool isShown;
    private TweenerCore<Vector2, Vector2, VectorOptions> activeTween;
    private Phone phone;

    public bool IsShown => isShown;

    public void ShowMessage(string _message, float _duration, Phone _phone)
    {
        timer = _duration;
        phone = _phone;
        text.text = _message;
        rect.sizeDelta = new Vector2(6f, 0f);

        activeTween = rect.DOSizeDelta(new Vector2(6f, 2.5f), animTime).OnComplete(() => 
        {
            isShown = true;
        });
    }

    private void Update()
    {
        if (!isShown)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            Hide();
        }
    }

    private void Hide()
    {
        isShown = false;
        activeTween = rect.DOSizeDelta(new Vector2(6f, 0f), animTime).SetEase(Ease.OutBack).OnComplete(() =>
        {
            phone.RemoveMessage(this);
            Destroy(gameObject);
        });
    }

    internal void ForceHide()
    {
        activeTween.Kill();
        Hide();
    }
}
