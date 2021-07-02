using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteractable : MonoBehaviour
{
    //WARNING Doing matrix physic changes with cHest Layer for performance
    //Maybe put control of the chest on player insted of chest scripts
    BoxCollider col;
    Chest chest;
    Animator anim;

    bool transitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        chest = GetComponentInParent<Chest>();
        col = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!chest.used) {
            anim.SetBool("Pop", true);
            anim.SetBool("Shrink", false);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (!chest.used && !transitioning) {
            if (Input.GetKeyDown(KeyCode.F)) {
                chest.OpenChest();
                anim.SetBool("Shrink", true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!chest.used) {
            anim.SetBool("Shrink", true);
            anim.SetBool("Pop", false);
        }
    }

    public void OnEndShrinkAnim() {
        if (!chest.used) {
            anim.SetBool("Shrink", false);
            transitioning = false;
        }
    }

    public void OnEndPopAnim() {
        if (!chest.used) {
            anim.SetBool("Pop", false);
            transitioning = false;
        }
    }
}
