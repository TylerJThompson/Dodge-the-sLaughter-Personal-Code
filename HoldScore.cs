using UnityEngine;

public class HoldScore : MonoBehaviour
{
    private int dodgerScore, slaughtererScore;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void setDodgerScore(int score)
    {
        dodgerScore = score;
    }

    public int getDodgerScore()
    {
        return dodgerScore;
    }

    public void setSlaughtererScore(int score)
    {
        slaughtererScore = score;
    }

    public int getSlaughtererScore()
    {
        return slaughtererScore;
    }
}
