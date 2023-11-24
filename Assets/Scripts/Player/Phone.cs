using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

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

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private MeshRenderer phoneQuad;

    private float textTimer;
    private float colorTimer;

    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlaySoundServerRpc(SoundType _sound)
    {
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
        DisplayTextClientRpc(_text, _duration);
    }

    [ClientRpc]
    private void DisplayTextClientRpc(string _text, float _duration)
    {
        DisplayText(_text, _duration);
    }

    public void DisplayText(string _text, float _duration)
    {
        text.text = _text;
        textTimer = _duration;
        text.enabled = true;
    }


    [ServerRpc(RequireOwnership = false)]
    public void DisplayColorServerRpc(Color _color, float _duration)
    {
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
        colorTimer = _duration;
        phoneQuad.enabled = true;
    }

    private void Update()
    {
        TextUpdate();
        ColorUpdate();
    }

    private void TextUpdate()
    {
        textTimer -= Time.deltaTime;
        if (textTimer <= 0 && text.enabled)
        {
            text.enabled = false;
        }
    }

    private void ColorUpdate()
    {
        colorTimer -= Time.deltaTime;
        if (colorTimer <= 0 && phoneQuad.enabled)
        {
            phoneQuad.enabled = false;
        }
    }
}
