using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MagicUI : MonoBehaviour
{
    private Image magicImage;
    public Sprite[] magicIcons;
    public static MagicUI INSTANCE;

    private void Awake() {
        if (INSTANCE != null) {
            Destroy(gameObject);
        }
        else {
            INSTANCE = this;
        }
        magicImage = GetComponent<Image>();
    }

    public void SwitchMagicIcon(int index) {
        if (index < magicIcons.Length && index >= 0) {
            magicImage.sprite = magicIcons[index];
        }
        else {
            Debug.LogError("Sprite Index out of bounds");
        }
    }
}
