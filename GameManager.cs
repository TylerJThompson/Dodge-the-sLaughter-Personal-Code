using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    [SerializeField] PhidgetReader phidgets;
    [SerializeField] Animator character, face;
    [SerializeField] AudioSource laughter;
    [SerializeField] Image laughterBarFill;
    [SerializeField] float tickleEffectiveness;
    [SerializeField] Camera mainCamera, countdownCamera, roundCamera;
    [SerializeField] VideoClip round1Clip, round2Clip, round3Clip;
    [SerializeField] VideoPlayer countdownVideoPlayer, roundVideoPlayer;
    [SerializeField] Sprite tickleAim, tickleBlock, baseExpression;
    [SerializeField] Image leftArmpitAim, rightArmpitAim, stomachAim;
    [SerializeField] Canvas foregroundCanvas, winLoseCanvas;
    [SerializeField] Image dodgerWin, slaughtererWin, dodgerScore, slaughtererScore;
    [SerializeField] Sprite[] numbers;
    //[SerializeField] HoldScore scoreHolder;
    [SerializeField] AudioSource characterVoice;
    [SerializeField] AudioClip[] characterLines;

    private RectTransform barTransform;
    private bool gameStarted;
    private int attackerWins, defenderWins;
    private int currRound;
    private bool allowTickle;

    private void Start()
    {
        if (laughterBarFill != null) barTransform = laughterBarFill.gameObject.GetComponent<RectTransform>();
        gameStarted = false;
        if (SceneManager.GetActiveScene().name.Equals("GameScene")) StartCoroutine(StartGame());
        attackerWins = 0;
        defenderWins = 0;
        currRound = 1;
        allowTickle = false;
    }

    private void Update()
    {
        if (phidgets != null && gameStarted)
        {
            if (phidgets.IsLeftArmpitSafe())
            {
                character.SetBool("LowerLeft", true);
                leftArmpitAim.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
                leftArmpitAim.sprite = tickleBlock;
                if (!characterVoice.isPlaying)
                {
                    characterVoice.clip = characterLines[Random.Range(0, characterLines.Length)];
                    characterVoice.Play();
                }
            }
            else
            {
                character.SetBool("LowerLeft", false);
                if (!phidgets.IsLeftArmpitTickled())
                {
                    float aimScale = leftArmpitAim.transform.localScale.x;
                    float newScale = Mathf.Min(aimScale + 0.005f, 0.06f);
                    leftArmpitAim.transform.localScale = new Vector3(newScale, newScale, 0.06f);
                }
                leftArmpitAim.sprite = tickleAim;
            }

            if (phidgets.IsRightArmpitSafe())
            {
                character.SetBool("LowerRight", true);
                rightArmpitAim.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
                rightArmpitAim.sprite = tickleBlock;
                if (!characterVoice.isPlaying)
                {
                    characterVoice.clip = characterLines[Random.Range(0, characterLines.Length)];
                    characterVoice.Play();
                }
            }
            else
            {
                character.SetBool("LowerRight", false);
                if (!phidgets.IsRightArmpitTickled())
                {
                    float aimScale = rightArmpitAim.transform.localScale.x;
                    float newScale = Mathf.Min(aimScale + 0.005f, 0.06f);
                    rightArmpitAim.transform.localScale = new Vector3(newScale, newScale, 0.06f);
                }
                rightArmpitAim.sprite = tickleAim;
            }

            if (phidgets.IsStomachSafe())
            {
                character.SetBool("CoverStomach", true);
                stomachAim.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
                stomachAim.sprite = tickleBlock;
                if (!characterVoice.isPlaying)
                {
                    characterVoice.clip = characterLines[Random.Range(0, characterLines.Length)];
                    characterVoice.Play();
                }
            }
            else
            {
                character.SetBool("CoverStomach", false);
                if (!phidgets.IsStomachTickled())
                {
                    float aimScale = stomachAim.transform.localScale.x;
                    float newScale = Mathf.Min(aimScale + 0.005f, 0.06f);
                    stomachAim.transform.localScale = new Vector3(newScale, newScale, 0.06f);
                }
                stomachAim.sprite = tickleAim;
            }

            if (phidgets.IsBeingTickled())
            {
                if (allowTickle)
                {
                    face.SetBool("BeingTickled", true);
                    if (!laughter.isPlaying) laughter.Play();
                    barTransform.localScale = new Vector3(0.8f, Mathf.Min(barTransform.localScale.y + tickleEffectiveness, 0.8f), 0.8f);
                }
                else
                {
                    if (phidgets.IsLeftArmpitTickled())
                    {
                        float aimScale = leftArmpitAim.transform.localScale.x;
                        float newScale = Mathf.Max(aimScale - 0.005f, 0.025f);
                        leftArmpitAim.transform.localScale = new Vector3(newScale, newScale, 0.06f);
                        if (newScale == 0.025f) allowTickle = true;
                    }
                    else if (phidgets.IsRightArmpitTickled())
                    {
                        float aimScale = rightArmpitAim.transform.localScale.x;
                        float newScale = Mathf.Max(aimScale - 0.005f, 0.025f);
                        rightArmpitAim.transform.localScale = new Vector3(newScale, newScale, 0.06f);
                        if (newScale == 0.025f) allowTickle = true;
                    }
                    else if (phidgets.IsStomachTickled())
                    {
                        float aimScale = stomachAim.transform.localScale.x;
                        float newScale = Mathf.Max(aimScale - 0.005f, 0.025f);
                        stomachAim.transform.localScale = new Vector3(newScale, newScale, 0.06f);
                        if (newScale == 0.025f) allowTickle = true;
                    }
                }
            }
            else
            {
                face.SetBool("BeingTickled", false);
                laughter.Pause();
                allowTickle = false;
            }
        }

        if (gameStarted)
        {
            if (barTransform.localScale.y == 0.8f)
            {
                attackerWins++;
                slaughtererWin.gameObject.SetActive(true);
                currRound++;
                StartCoroutine(AdvanceRound(currRound));
            }
            else if (countdownVideoPlayer.isPlaying == false)
            {
                defenderWins++;
                dodgerWin.gameObject.SetActive(true);
                currRound++;
                StartCoroutine(AdvanceRound(currRound));
            }
        }
    }

    IEnumerator StartGame()
    {
        roundVideoPlayer.Play();
        yield return new WaitForSeconds(19f + (23f/30f));
        //yield return new WaitWhile(() => roundVideoPlayer.isPlaying);
        character.gameObject.SetActive(true);
        roundCamera.gameObject.SetActive(false);
        foregroundCanvas.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(true);
        countdownVideoPlayer.Play();
        yield return new WaitForSeconds(0.15f);
        countdownCamera.gameObject.SetActive(true);
        gameStarted = true;
    }

    IEnumerator AdvanceRound(int round)
    {
        if (round >= 1 && round <= 3)
        {
            if (defenderWins >= 2)
            {
                //scoreHolder.setDodgerScore(defenderWins);
                //scoreHolder.setSlaughtererScore(attackerWins);
                ChangeScene("SurvivedScene");
            }
            else if (attackerWins >= 2)
            {
                //scoreHolder.setDodgerScore(defenderWins);
                //scoreHolder.setSlaughtererScore(attackerWins);
                ChangeScene("WonScene");
            }
            countdownCamera.gameObject.SetActive(false);
            foregroundCanvas.gameObject.SetActive(false);
            leftArmpitAim.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            rightArmpitAim.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            stomachAim.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            dodgerScore.sprite = numbers[defenderWins];
            slaughtererScore.sprite = numbers[attackerWins];
            winLoseCanvas.gameObject.SetActive(true);
            face.SetBool("BeingTickled", false);
            laughter.Pause();
            character.gameObject.SetActive(false);
            gameStarted = false;
            barTransform.localScale = new Vector3(0.8f, 0f, 0.8f);
            yield return new WaitForSeconds(2f);
            winLoseCanvas.gameObject.SetActive(false);
            dodgerWin.gameObject.SetActive(false);
            slaughtererWin.gameObject.SetActive(false);
            if (round == 1) roundVideoPlayer.clip = round1Clip;
            else if (round == 2) roundVideoPlayer.clip = round2Clip;
            else if (round == 3) roundVideoPlayer.clip = round3Clip;
            roundVideoPlayer.Play();
            roundCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            yield return new WaitForSeconds(9f + (18f / 30f));
            //yield return new WaitWhile(() => roundVideoPlayer.isPlaying);
            face.gameObject.GetComponent<SpriteRenderer>().sprite = baseExpression;
            character.gameObject.SetActive(true);
            roundCamera.gameObject.SetActive(false);
            foregroundCanvas.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(true);
            countdownVideoPlayer.Stop();
            countdownVideoPlayer.Play();
            yield return new WaitForSeconds(0.15f);
            countdownCamera.gameObject.SetActive(true);
            gameStarted = true;
        }
        else
        {
            if (defenderWins >= 2)
            {
                //scoreHolder.setDodgerScore(defenderWins);
                //scoreHolder.setSlaughtererScore(attackerWins);
                ChangeScene("SurvivedScene");
            }
            else if (attackerWins >= 2)
            {
                //scoreHolder.setDodgerScore(defenderWins);
                //scoreHolder.setSlaughtererScore(attackerWins);
                ChangeScene("WonScene");
            }
            gameStarted = false;
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
