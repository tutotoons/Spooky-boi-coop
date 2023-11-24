using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public static Phone Instance
    {
        get;
        private set;
    }
    [SerializeField] private AudioSource source;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private MeshRenderer phoneQuad;

    private float textTimer;
    private float colorTimer;

    public void Init()
    {
        if (Instance != null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip _clip)
    {
        source.PlayOneShot(_clip);
    }

    public void DisplayText(string _text, float _duration)
    {
        text.text = _text;
        textTimer = _duration;
        text.enabled = true;
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
