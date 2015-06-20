using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

internal class RoundOverPanel : MonoBehaviour, IPanel
{
    #region Singleton

    private static RoundOverPanel instance;

    internal static RoundOverPanel Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType(typeof(RoundOverPanel)) as RoundOverPanel;
            if (instance != null) return instance;

            var container = new GameObject("RoundOverPanel");
            instance = container.AddComponent<RoundOverPanel>();
            return instance;
        }
    }

    #endregion

    #region Enumerations

    #endregion

    #region Events and Delegates

    #endregion

    #region Variables

    public GameObject[] playerPanels;
    public Text titleText;
    public Button nextRoundButton;
    public Button restartButton;

    #endregion

    #region Properties

    #endregion

    #region Methods

    private void Awake()
    {
        instance = this;
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        if (nextRoundButton.gameObject.activeSelf) nextRoundButton.GetComponentInChildren<Text>().text = Constants.NEXT_ROUND;
        restartButton.GetComponentInChildren<Text>().text = Constants.RESTART;
    }

    public void Init(int currentRound, int maxRound, Player[] players)
    {
        titleText.text = string.Format(Constants.ROUND_TEXT, currentRound, maxRound);
      
        for (int i = 0; i < players.Length; i++)
        {
            for (int j = 0; j < players[i].previousScores.Count; j++)
            {
                playerPanels[i].transform.GetChild(j + 1).GetComponent<Text>().text = players[i].previousScores[j].ToString("0");
            }
        }

        if (currentRound == maxRound)
        {
            nextRoundButton.gameObject.SetActive(false);
            float[] totalScores = new float[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                if (currentRound == maxRound) playerPanels[i].GetComponent<Image>().color = Constants.ROUND_PANEL_NORMAL_COLOR;
                for (int j = 0; j < players[i].previousScores.Count; j++)
                {
                    totalScores[i] += players[i].previousScores[j];
                }
            }

            int highestIndex = 0;
            for (int i = 1; i < totalScores.Length; i++)
            {
                if (totalScores[i] > totalScores[highestIndex])
                {
                    highestIndex = i;
                }
            }
            playerPanels[highestIndex].GetComponent<Image>().color = Constants.ROUND_PANEL_WINNING_COLOR;
        }
    }

    public void OnNextRoundClicked()
    {
        GameManager.Instance.Show();
        GameManager.Instance.Init();
        Hide();
    }

    public void OnRestartClicked()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    #endregion

    #region Structs

    #endregion

    #region Classes

    #endregion
}