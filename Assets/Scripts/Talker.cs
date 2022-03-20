using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Talker : MonoBehaviour
{
    public bool isTalking;
    [Space]
    public float dialogueDelay;
    public float fadeOutTime;
    float fadeOutTimer;
    
    [Header("Testing")]
    [TextArea]
    public string testString;

    [Header("References")]
    public Image backgroundImage;
    public TextMeshProUGUI textMesh;
    
    public static Talker Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetActive(bool enabled)
    {
        backgroundImage.gameObject.SetActive(enabled);
        textMesh.gameObject.SetActive(enabled);
    }

    public void Talk(string[] texts)
    {
        Talk(texts[Random.Range(0, texts.Length)]);
    }

    public void Talk(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TalkEnum(text));
    }

    IEnumerator TalkEnum(string dialogueText)
    {
        string[] currDialogue = dialogueText.Split('\n');

        foreach (string text in currDialogue)
        {
            ShowText(text);
            
            yield return new WaitForSeconds(dialogueDelay);
        }
    }

    public void ShowText(string text)
    {
        isTalking = true;
        AudioManager.main.Play("bum");
        SetActive(true);
        textMesh.text = text;
        backgroundImage.DOColor(new Color(0, 0, 0, 0.7f), 0.4f).From(Color.white);
        textMesh.color = Color.white;
        textMesh.transform.DOScale(Vector3.one, Mathf.Min(0.5f,dialogueDelay)).From(Vector3.one * 1.2f);
        fadeOutTimer = fadeOutTime;
    }

    private void Update()
    {
        if (isTalking)
        {
            fadeOutTimer -= Time.deltaTime;
            if (fadeOutTimer <= 0)
            {
                isTalking = false;
                backgroundImage.DOColor(Color.clear, 0.5f).OnComplete(() => SetActive(false));
                textMesh.DOColor(Color.clear, 0.5f);
            }
        }
    }

    [Button()]
    public void TestTalk()
    {
        Talk(testString);
    }
    
    
}
