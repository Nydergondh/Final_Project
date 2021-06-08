using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodHandler : MonoBehaviour
{
    private GameObject TestBlood;
    private ParticleSystem pSystem;
    private ParticleCollisionEvent[] CollisionEvents;

    //public void OnParticleCollision(GameObject other) {
    //    int collCount = pSystem.GetSafeCollisionEventSize();

    //    if (collCount > CollisionEvents.Length) {
    //        CollisionEvents = new ParticleCollisionEvent[collCount];
    //    }
    //    int eventCount = pSystem.GetCollisionEvents(other, CollisionEvents);

    //    for (int i = 0; i < eventCount; i++) {
    //        Instantiate(TestBlood, CollisionEvents[i].intersection, );
    //        //TODO: Do your collision stuff here. 
    //        // You can access the CollisionEvent[i] to obtaion point of intersection, normals that kind of thing
    //        // You can simply use "other" GameObject to access it's rigidbody to apply force, or check if it implements a class that takes damage or whatever
    //    }
    //}
}
