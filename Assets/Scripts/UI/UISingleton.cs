using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UISingleton : MonoBehaviour
{
    [HideInInspector]
    public MessageHighligh messageHighlighter;
    public static UISingleton INSTANCE;

    private Animator anim;

    private void Awake() {
        if (INSTANCE == null) {
            INSTANCE = this;
        }
        else {
            Destroy(gameObject);
        }
        anim = GetComponent<Animator>();
    }

    private void Start() {
        messageHighlighter = GetComponentInChildren<MessageHighligh>();
    }

    public void ShowStageCleared() {
        anim.SetBool("StageCleared", true);
        //messageHighlighter.UpdateText("STAGE CLEARED");
        //messageHighlighter.ShowPickUpText();
    }

    public void FadeIn() {
        anim.SetBool("FadeIn", true);
        anim.SetBool("FadeOut", false);
    }

    public void FadeOut() {
        anim.SetBool("FadeIn", false);
        anim.SetBool("FadeOut", true);
    }

    public void LoadNewMap() {
        anim.SetBool("FadeIn", false);
        anim.SetBool("FadeOut", false);
    }

}
