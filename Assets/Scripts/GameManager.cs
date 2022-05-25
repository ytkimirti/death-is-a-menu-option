using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EZCameraShake;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public bool isFrozen;
    public string level;
    public int stage;
    public Option currOption;
    
    [Header("Colors")]
    public Color fillColor;
    public Color whiteColor;
    public Color darkColor;
    public Color hoverColor;
    [Header("Text")]
    public Color textWhiteColor;
    public Color textDisabledColor;
    public Color textBlackColor;
    public static GameManager Instance;
    
    [Header("References")]
    public Option humanOption;
    public GameObject fadeOutGameObject;
    public GameObject playerGameObject;
    public ParticleSystem flowerParticle;
    public GameObject bombGameObject;
    public GameObject helpObject;
    public GameObject editObject;
    public GameObject endTextGameObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fadeOutGameObject.SetActive(true);
        fadeOutGameObject.GetComponent<Image>().DOColor(Color.clear, 3f).OnComplete(() => fadeOutGameObject.SetActive(false));
        StartCoroutine(StartTalk());
    }
    int clickCount = 0;

    private void Update()
    {
        Time.timeScale = Input.GetKey(KeyCode.C) ? 10f : 1f;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AudioManager.main.Play("click");

            if (bombGameObject.activeInHierarchy && !Talker.Instance.isTalking)
            {
                clickCount++;
                if (clickCount % 10 == 0)
                {
                    Talker.Instance.Talk("Try again");
                }
            }
        }
    }

    public void OnNewLevel(string levelName)
    {
        level = levelName;
        stage = 0;
    }

    public void CloseOptions()
    {
        Option[] options = FindObjectsOfType<Option>();

        foreach (Option option in options)
        {
            option.isMouseOver = false;
        }
    }

    IEnumerator AfterCreation()
    {
        Talker.Instance.Talk("hmm...");
        yield return new WaitForSeconds(1.2f);
    }
    IEnumerator AfterCompliment()
    {
        yield return new WaitForSeconds(2f);
        Talker.Instance.Talk("Stop");
    }

    void AfterCompliment2()
    {
        Talker.Instance.Talk("I\nsaid\nstop");
    }
    public void OnClick(Option option)
    {
        //AudioManager.main.Play("double_click");
        if (!option.childWindow)
        {
            CloseOptions();
            if (level == "intro")
            {
                if (option != humanOption)
                {
                    Talker.Instance.Talk("Not that");
                    return;
                }
                
                playerGameObject.SetActive(true);
                editObject.SetActive(true);
                StartCoroutine(AfterCreation());
                OnNewLevel("compliment");
            }
            else if (level == "compliment")
            {
                if (stage == 5)
                {
                    OnComplimentGiven();
                }
                else
                {
                    
                switch (option.gameObject.name)
                {
                    case "Compliment":
                        Talker.Instance.Talk(new string[]
                        {
                            "Nice\nHat",
                            "Beatiful",
                            "Masterpiece",
                            "uwu",
                        });
                        OnComplimentGiven();
                        break;
                    case "Give Flower":
                        Talker.Instance.Talk("*throws flower*");
                        flowerParticle.Play();
                        OnComplimentGiven();
                        break;
                    case "Stare":
                        Talker.Instance.Talk("...");
                        break;
                }
                
                }
            }
            
        }
    }

    public void KillDirect()
    {
        KillHumans();
        if ()(
                {}
                    )
            helpObject.SetActive(true);
        StartCoroutine(BadEnum());
            isFrozen = true;
    }

    IEnumerator BadEnum()
    {
        yield return new WaitForSeconds(1.2f);
        Talker.Instance.Talk("*gasp*");
        // yield return new WaitForSeconds(1.2f);
        Talker.Instance.Talk("You\nwere\nsupposed\nto\nnot press\nthat");
        yield return new WaitForSeconds(7f);
        // Talker.Instance.Talk("This\nis\nscripted\nlmao");
        // SceneManager.LoadScene(0, LoadSceneMode.Single);
        StartCoroutine(AfterDeathTalkEnum());
    }

    public void OnComplimentGiven()
    {
        stage++;
        if (stage == 4 || Input.GetKey(KeyCode.E))
        {
            helpObject.SetActive(true);
            KillHumans();
            isFrozen = true;
            StartCoroutine(AfterDeathTalkEnum());
        }
        else if (stage == 2)
        {
            StartCoroutine(AfterCompliment());
        }
        else if (stage == 3)
        {
            Invoke("AfterCompliment2", 2f);
        }
        else
        {
            CameraShaker.Instance.ShakeOnce(4, 2, 0, 0.5f);
        }
    }

    IEnumerator StartTalk()
    {
        yield return new WaitForSeconds(4f);
        Talker.Instance.Talk("Create\na\nhuman");
        yield return new WaitForSeconds(4f);
        Talker.Instance.Talk("Please?");
    }

    IEnumerator AfterDeathTalkEnum()
    {
        yield return new WaitForSeconds(2f);
        Talker.Instance.Talk("Death\nWAS\nan\noption");
        yield return new WaitForSeconds(4f);
        Talker.Instance.Talk("But\nit\nwill\nnot\nbe\nfor\nyou");
        yield return new WaitForSeconds(7f);
        bombGameObject.SetActive(true);
        bombGameObject.transform.DOMoveX(0, 1f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(2f);
        Talker.Instance.Talk("hehe");
        yield return new WaitForSeconds(33f);// BOMB WAIT TIME
        AudioManager.main.Play("bigbomb");
        bombGameObject.SetActive(false);
        fadeOutGameObject.SetActive(true);
        fadeOutGameObject.GetComponent<Image>().color = Color.black;
        yield return new WaitForSeconds(4f);
        Talker.Instance.Talk("I\ntold\nyou");
        yield return new WaitForSeconds(4f);
        Talker.Instance.Talk("It\nwas\nnot\nan\noption");
        yield return new WaitForSeconds(6f);
        Talker.Instance.Talk("uwu");
        yield return new WaitForSeconds(3f);
        endTextGameObject.SetActive(true);
    }

    public void KillHumans()
    {
        Human[] humans = FindObjectsOfType<Human>();
        foreach (Human human in humans)
        {
            human.Die();
        }
    }
}