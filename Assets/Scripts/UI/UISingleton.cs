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
        messageHighlighter.UpdateText("STAGE CLEARED");
        messageHighlighter.ShowPickUpText();
    }

}
