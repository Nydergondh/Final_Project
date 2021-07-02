using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAudios : MonoBehaviour
{
    public static SoundAudios soundAudios;

    public  SoundAudioClip[] soundAudioClipArray;

    void Awake() {
        if (soundAudios != null) {
            Destroy(this);
            return;
        }
        soundAudios = this;
    }

    [System.Serializable]
    public class SoundAudioClip {
        public Sound sound;
        public AudioClip audioClip;
    }

    public enum Sound {
        WeapomSwing_1 = 1,
        WeapomSwing_2 = 2,
        WeapomSwing_3 = 3,
        BloodSplash = 4,
        MainMusic = 5,
        BossMusic = 6
    }
}
