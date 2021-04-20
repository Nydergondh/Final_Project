using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour
{
    //WARNING Doing matrix physic changes with cHest Layer for performance
    //Maybe put control of the chest on player insted of chest scripts
    BoxCollider col;
    PickUpItem pickUpItem;
    Animator anim;

    bool transitioning = false;
    // Start is called before the first frame update
    void Start() {
        pickUpItem = GetComponentInParent<PickUpItem>();
        col = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (pickUpItem) {
            anim.SetBool("Pop", true);
            anim.SetBool("Shrink", false);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (!transitioning) {
            if (Input.GetKeyDown(KeyCode.F)) {
                pickUpItem.PickUpWeapom();
                anim.SetBool("Shrink", true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (pickUpItem) {
            anim.SetBool("Shrink", true);
            anim.SetBool("Pop", false);
        }
    }

    public void OnEndShrinkAnim() {
        if (pickUpItem) {
            anim.SetBool("Shrink", false);
            transitioning = false;
        }
    }

    public void OnEndPopAnim() {
        if (pickUpItem) {
            anim.SetBool("Pop", false);
            transitioning = false;
        }
    }
}
