using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour
{
    [SerializeField] float delay;

    private void Start()
    {
        if (delay == 0f) SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
        else StartCoroutine(WaitForVideo());
    }

    IEnumerator WaitForVideo()
    {
        AudioSource bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
        bgm.volume = 0;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
    }
}
