using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyTutorialText : MonoBehaviour
{
    public TextMeshPro tutorialText;
    private Enemy_Test enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy_Test>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.alive) {
            tutorialText.enabled = false;
        }
    }
}
