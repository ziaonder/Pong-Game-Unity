using TMPro;
using UnityEngine;

public class CanvasSettings : MonoBehaviour
{
    [SerializeField] private GameObject textObject, scoreObject;

    private void OnEnable()
    {
        BallControl.OnScore += UpdateScoreUI;
    }

    private void OnDisable()
    {
        BallControl.OnScore -= UpdateScoreUI;
    }

    private void Update()
    {
        if(BallControl.stateMachine.GetCurrentState() == StateMachine.states.SET && 
            textObject.GetComponent<TextMeshProUGUI>().text != "Press Enter to Start")
        {
            textObject.GetComponent<TextMeshProUGUI>().text = "Press Enter to Start";
            textObject.SetActive(true);
            scoreObject.SetActive(false);
        }

        if(BallControl.stateMachine.GetCurrentState() == StateMachine.states.STARTED && textObject.activeSelf)
        {
            textObject.SetActive(false);
            scoreObject.SetActive(true);
        }

        if(BallControl.stateMachine.GetCurrentState() == StateMachine.states.ENDED && !textObject.activeSelf)
        {
            textObject.GetComponent<TextMeshProUGUI>().text = $"Player 1 : {ScoreManager.GetScore("left")}" +
                $" - {ScoreManager.GetScore("right")} : Player 2 " +
                $"\n {(ScoreManager.GetScore("left") > ScoreManager.GetScore("right") ? "Player 1 Won!" : "Player 2 Won!")}" +
                "\n\n Press Enter to Play Again";
            textObject.SetActive(true);
            scoreObject.SetActive(false);
        }
    }

    private void UpdateScoreUI(string a)
    {
        scoreObject.GetComponent<TextMeshProUGUI>().text = ScoreManager.GetScore("left") + " - " +  ScoreManager.GetScore("right");
    }
}
