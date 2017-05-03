using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {

    private string[] bgm;
    private AudioSource audioSource;

    public void Start() {
        bgm = new string[5];
        for (int i = 0; i < 5; ++i)
            bgm[i] = "BGM_" + i;
        int bgmID = Random.Range(0, 1);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip(bgm[bgmID]);
        audioSource.Play();
    }

    public void Update() {
        if (!audioSource.isPlaying) {
            int bgmID = Random.Range(0, 5);
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip(bgm[bgmID]);
            audioSource.Play();
        }
    }
}
