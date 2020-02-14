using UnityEngine;

public class IncreaseBGMVolume : MonoBehaviour
{
    private void Start()
    {
        AudioSource bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
        bgm.volume = 0.5f;
    }
}
