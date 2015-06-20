using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;

internal class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance;

    internal static GameManager Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            if (instance != null) return instance;

            var container = new GameObject("GameManager");
            instance = container.AddComponent<GameManager>();
            return instance;
        }
    }

    #endregion

    #region Enumerations

    internal enum State
    {
        None,
        WaitingDiceRoll,
        Drawing
    }

    #endregion

    #region Events and Delegates

    #endregion

    #region Variables

    private Player[] _players = new Player[2] { new Player(), new Player() };
    [SerializeField]
    private State _currentState = State.None;
    private int _playerIndex = 0;
    private int _currentRound = 1;
    [SerializeField]
    private int _numberOfRounds = 5;

    public Button diceButton;
    public Button drawingDoneButton;

    public Text[] playerScoreTexts;
    public Text informationText;
    public Text drawBeetlePartText;
    public Image[] _playerBeetleImages;

    public Texture2D emptyCanvasTexture;
    #endregion

    #region Properties

    public Player CurrentPlayer
    {
        get { return _players[_playerIndex]; }
    }

    public Text CurrentScoreText
    {
        get { return playerScoreTexts[_playerIndex]; }
    }

    public Image CurrentBeetleImage
    {
        get { return _playerBeetleImages[_playerIndex]; }
    }

    public State CurrentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            switch (_currentState)
            {
                case State.None:
                    Painter.Instance.Lock();
                    diceButton.gameObject.SetActive(true);
                    drawingDoneButton.gameObject.SetActive(false);
                    drawBeetlePartText.gameObject.SetActive(false);
                    break;
                case State.WaitingDiceRoll:
                    diceButton.interactable = true;
                    Painter.Instance.Lock();
                    diceButton.gameObject.SetActive(true);
                    drawingDoneButton.gameObject.SetActive(false);
                    drawBeetlePartText.gameObject.SetActive(false);
                    diceButton.GetComponentInChildren<Text>().text = Constants.ROLL_DICE_TEXT;
                    diceButton.GetComponent<Image>().color = Constants.DICE_DEFAULT_COLOR;
                    informationText.text = string.Format(Constants.PLAYER_TURN_TEXT,(_playerIndex + 1));
                    break;
                case State.Drawing:
                    Painter.Instance.PaintCanvas = CurrentBeetleImage.sprite.texture;
                    Painter.Instance.Unlock(true);
                    diceButton.gameObject.SetActive(false);
                    drawBeetlePartText.gameObject.SetActive(true);
                    drawingDoneButton.gameObject.SetActive(true);
                    informationText.text = "";
                    break;
            }
        }
    }

    #endregion

    #region Methods

    private void Start()
    {
        drawingDoneButton.GetComponentInChildren<Text>().text = Constants.DONE_DRAWING_TEXT;
        diceButton.GetComponentInChildren<Text>().text = Constants.ROLL_DICE_TEXT;
        for (int i = 0; i < _playerBeetleImages.Length; i++)
        {
            Texture2D tempTexture = Instantiate(emptyCanvasTexture);
            _playerBeetleImages[i].sprite = Sprite.Create(tempTexture, new Rect(0, 0, tempTexture.width, tempTexture.height), new Vector2(0.5f, 0.5f));
        }

        CurrentState = State.WaitingDiceRoll;
    }

    private void Update()
    {

    }

    private int RollDice()
    {
        return Random.Range(1, 6);
    }

    private void NextPlayer()
    {
        _playerIndex++;
        if (_playerIndex >= _players.Length) _playerIndex = 0;
    }

    public void OnDiceRollClicked()
    {
        diceButton.interactable = false;
        StartCoroutine(RollDiceRotuine());
    }

    public void OnDrawCompletedClicked()
    {
        Texture2D currentTexture2D = Painter.Instance.PaintCanvas;
        CurrentBeetleImage.sprite = Sprite.Create(currentTexture2D, new Rect(0, 0, currentTexture2D.width, currentTexture2D.height), new Vector2(0.5f, 0.5f));
        if (CurrentPlayer.PlayerBeetle.IsCompleted)
        {

        }
        else
        {
            NextPlayer();
            CurrentState = State.WaitingDiceRoll;
        }
    }

    IEnumerator AddScore(Text playerScoreText, float scoreToAdd)
    {
        while (scoreToAdd > 0)
        {
            float change = Time.deltaTime * 250;
            scoreToAdd -= change;
            CurrentPlayer.Score += change;
            if (scoreToAdd < 0) CurrentPlayer.Score += scoreToAdd;
            playerScoreText.text = CurrentPlayer.Score.ToString("0");
            yield return null;
        }
    }

    private IEnumerator RollDiceRotuine()
    {
        Beetle.Part dice = (Beetle.Part)Random.Range(1, 6);
        int loopCount = Random.Range(5, 7);
        const float minWaitInterval = 0.05f;
        const float maxWaitInterval = 0.5f;
        diceButton.GetComponent<Image>().color = Constants.DICE_ROLLING_COLOR;
        int previousRoll = 0;
        for (int i = 0; i < loopCount; i++)
        {
            int roll = Random.Range(1, 6);
            while (previousRoll == roll) roll = Random.Range(1, 6);
            previousRoll = roll;
            diceButton.GetComponentInChildren<Text>().text = Constants.GetBeetlePartText((Beetle.Part)roll);
            yield return new WaitForSeconds(Mathf.Lerp(minWaitInterval, maxWaitInterval, i / (float)loopCount));

        }

        string partText = Constants.GetBeetlePartText(dice);
        diceButton.GetComponentInChildren<Text>().text = partText;

        if (CurrentPlayer.PlayerBeetle.TryAction(dice))
        {
            diceButton.GetComponent<Image>().color = Constants.DICE_ROLL_VALID_COLOR;
            StartCoroutine(AddScore(CurrentScoreText, 50));
            informationText.text = "50";
            informationText.GetComponent<Animator>().SetTrigger("Spawn");
            drawBeetlePartText.text = String.Format(Constants.DRAW_BEETLE_PART, (_playerIndex + 1), partText);
            yield return new WaitForSeconds(1);
            CurrentState = State.Drawing;
        }
        else
        {
            diceButton.GetComponent<Image>().color = Constants.DICE_ROLL_INVALID_COLOR;
            yield return new WaitForSeconds(1);
            NextPlayer();
            CurrentState = State.WaitingDiceRoll;
        }
    }

    #endregion

    #region Structs

    #endregion

    #region Classes

    #endregion
}