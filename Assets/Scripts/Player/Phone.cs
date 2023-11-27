using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Color = UnityEngine.Color;

public enum SoundType
{
    Beep,
    Boop
}

public class Phone : NetworkBehaviour
{
    public static Phone Instance
    {
        get;
        private set;
    }
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clips;


    [SerializeField] private PhoneMessage messagePrefab;
    [SerializeField] private RectTransform messageParent;
    [SerializeField] private int allowedMessageCount;
    [SerializeField] private MeshRenderer phoneQuad;
    [SerializeField] private Light lightObj;
    [SerializeField] private Vector3 showPos, hidePos;
    [SerializeField] private float shownAnimationDuration;
    [SerializeField] private Ease showAnimationEase = Ease.OutExpo;
    private bool isShown;

    private float colorTimer;
    private List<PhoneMessage> activeMessages;

    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            activeMessages = new List<PhoneMessage>();
            Hide(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlaySoundServerRpc(SoundType _sound)
    {
        if (!isShown)
        {
            return;
        }
        PlaySoundClientRpc(_sound);
    }

    [ClientRpc]
    private void PlaySoundClientRpc(SoundType _sound)
    {
        PlaySound(_sound);
    }

    public void PlaySound(SoundType _clip)
    {
        source.PlayOneShot(clips[(int)_clip]);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DisplayTextServerRpc(string _text, float _duration)
    {
        if (!isShown)
        {
            return;
        }
        DisplayTextClientRpc(_text, _duration);
    }

    [ClientRpc]
    private void DisplayTextClientRpc(string _text, float _duration)
    {
        DisplayText(_text, _duration);
    }

    public void DisplayText(string _text, float _duration)
    {
        if (activeMessages.Count >= allowedMessageCount)
        {
            PhoneMessage _firstShownMessage = activeMessages.First((_msg) => _msg.IsShown);
            _firstShownMessage?.ForceHide();
        }
        PhoneMessage _phoneMessage = Instantiate(messagePrefab, messageParent);
        _phoneMessage.ShowMessage(_text, _duration, this);
        activeMessages.Add(_phoneMessage);

    }


    [ServerRpc(RequireOwnership = false)]
    public void DisplayColorServerRpc(Color _color, float _duration)
    {
        if (!isShown)
        {
            return;
        }
        DisplayColorClientRpc(_color, _duration);
    }

    [ClientRpc]
    private void DisplayColorClientRpc(Color _color, float _duration)
    {
        DisplayColor(_color, _duration);
    }
    public void DisplayColor(Color _color, float _duration)
    {
        phoneQuad.material.color = _color;
        phoneQuad.material.SetColor("_EmissionColor", _color);
        colorTimer = _duration;
        phoneQuad.enabled = true;
    }

    private void Update()
    {
        ColorUpdate();
    }

    private void ColorUpdate()
    {
        colorTimer -= Time.deltaTime;
        if (colorTimer <= 0 && phoneQuad.enabled)
        {
            phoneQuad.enabled = false;
        }
    }

    internal void Show()
    {
        if (isShown)
        {
            return;
        }
        gameObject.SetActive(true);
        transform.position = hidePos;
        transform.DOLocalMove(showPos, shownAnimationDuration).SetEase(showAnimationEase).OnComplete(() =>
        {
            isShown = true;
        });
    }

    private void Hide(bool _force = false)
    {
        if (!isShown && !_force)
        {
            return;
        }
        transform.position = showPos;
        transform.DOLocalMove(hidePos, shownAnimationDuration).SetEase(showAnimationEase).OnComplete(() =>
        {
            isShown = false;
            gameObject.SetActive(false);
        });
    }

    internal void RemoveMessage(PhoneMessage _phoneMessage)
    {
        activeMessages.Remove(_phoneMessage);
    }
}
