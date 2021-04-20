using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class MessageHighligh : MonoBehaviour
{
    TMP_Text actionText;
    public bool isShowing;
    public float timeToDisapper = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        actionText = GetComponent<TMP_Text>();
    }

    public IEnumerator ShowPickUpText() {
        actionText.enabled = true;
        float currentTime = 0f;
        while (currentTime < timeToDisapper) {
            currentTime += Time.deltaTime; 
            yield return null;
        }
        actionText.enabled = false;
    }

    public void UpdateText(string newText) {
        actionText.text = newText;
    }
}
