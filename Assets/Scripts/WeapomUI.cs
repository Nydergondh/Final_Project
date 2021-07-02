using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeapomUI : MonoBehaviour
{
    private GameObject weapomObj;
    private Image imageCurrentWeapom = null;
    public static WeapomUI INSTANCE;

    private void Awake() {
        if (INSTANCE != null) {
            Destroy(gameObject);
        }
        else {
            INSTANCE = this;
        }
    }

    private void Start() {
        imageCurrentWeapom = GetComponent<Image>();
    }

    public void SwitchWeapom(Weapom_SO newWeapom) {
        if (!imageCurrentWeapom.enabled) {
            imageCurrentWeapom.enabled = true;
        }
        imageCurrentWeapom.sprite = newWeapom.weapomSprite;
    }

}
