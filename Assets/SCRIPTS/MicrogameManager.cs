using UnityEngine;

public class MicrogameManager : MonoBehaviour
{
    [SerializeField] private MicrogameTimer timer;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    private bool isFinished;

    public void WinGame()
    {
        if (isFinished)
        {
            return;
        }

        isFinished = true;
        timer.StopTimer();

        victoryPanel.SetActive(true);
    }

    public void LoseGame()
    {
        if (isFinished)
        {
            return;
        }

        isFinished = true;

        defeatPanel.SetActive(true);
    }
}