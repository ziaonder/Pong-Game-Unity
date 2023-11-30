using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int leftPlayerScore, rightPlayerScore;

    private void OnEnable()
    {
        BallControl.OnScore += UpdateScore;
    }

    private void OnDisable()
    {
        BallControl.OnScore -= UpdateScore;
    }

    private void Awake()
    {
        leftPlayerScore = 0;
        rightPlayerScore = 0;
    }

    public static int GetScore(string player)
    {
        if (player == "left")
            return leftPlayerScore;
        else
            return rightPlayerScore;
    }

    public void UpdateScore(string scoredPlayer)
    {
        if (scoredPlayer == "left")
            leftPlayerScore += 1;
        else if (scoredPlayer == "right")
            rightPlayerScore += 1;
    }

    public static void ResetScore()
    {
        leftPlayerScore  = 0;
        rightPlayerScore = 0;
    }
}
