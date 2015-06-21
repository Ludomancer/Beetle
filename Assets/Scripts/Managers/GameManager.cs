using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
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

    public Action<Player, int> OnPlayerScoreChanged;
    public Action<Player, int> OnPlayerBeetleChanged;
    public Action<Player, int, State> OnGameStateChanged;
    #endregion

    #region Variables
    [SerializeField]
    private int _numberOfRounds = 5;
    [SerializeField]
    private Texture2D _emptyCanvasTexture;
      [SerializeField]
    private AudioSource _diceAudioSource;
      [SerializeField]
    private AudioSource _successAudioSource;

    private readonly Player[] _players = new Player[2];
    private State _currentState = State.None;
    private int _playerIndex = 0;
    private int _currentRound = 0;

    public Button diceButton;
    public Button drawingDoneButton;

    public bool debugMode;
    #endregion

    #region Properties

    public Player CurrentPlayer
    {
        get { return _players[_playerIndex]; }
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
                    break;
                case State.WaitingDiceRoll:
                    diceButton.interactable = true;
                    Painter.Instance.Lock();
                    diceButton.gameObject.SetActive(true);
                    drawingDoneButton.gameObject.SetActive(false);
                    diceButton.GetComponentInChildren<Text>().text = Constants.ROLL_DICE_TEXT;
                    diceButton.GetComponent<Image>().color = Constants.DICE_DEFAULT_COLOR;
                    break;
                case State.Drawing:
                    Painter.Instance.PaintCanvas = CurrentPlayer.BeetleCanvas;
                    Painter.Instance.Unlock(true);
                    diceButton.gameObject.SetActive(false);
                    drawingDoneButton.gameObject.SetActive(true);
                    break;
            }
            if (OnGameStateChanged != null) OnGameStateChanged(CurrentPlayer, _playerIndex, _currentState);
        }
    }

    #endregion

    #region Methods

    private void Start()
    {
        instance = this;
        GameHUD.Instance.Show();
        Init();
    }

    public void Init()
    {
        _currentRound++;
        drawingDoneButton.gameObject.SetActive(true);
        drawingDoneButton.GetComponentInChildren<Text>().text = Constants.DONE_DRAWING_TEXT;
        diceButton.gameObject.SetActive(true);
        diceButton.GetComponentInChildren<Text>().text = Constants.ROLL_DICE_TEXT;
        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i]) _players[i].ClearBeetleImage();
            else _players[i] = new Player(Instantiate(_emptyCanvasTexture));
            if (OnPlayerScoreChanged != null) OnPlayerScoreChanged(_players[i], i);
            if (OnPlayerBeetleChanged != null) OnPlayerBeetleChanged(_players[i], i);
        }

        _playerIndex = Random.Range(0, _players.Length);
        CurrentState = State.WaitingDiceRoll;
    }

    private void Update()
    {

    }

    private void NextPlayer()
    {
        _playerIndex++;
        if (_playerIndex >= _players.Length) _playerIndex = 0;
    }

    public void OnDiceRollClicked()
    {
        _diceAudioSource.Play();
        diceButton.interactable = false;
        StartCoroutine(RollDiceRotuine());
    }

    public void OnDrawCompletedClicked()
    {
        if (OnPlayerBeetleChanged != null) OnPlayerBeetleChanged(CurrentPlayer, _playerIndex);
        if (CurrentPlayer.PlayerBeetle.IsCompleted)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].ArchiveScore();
                _players[i].Reset();
            }
            RoundOverPanel.Instance.Init(_currentRound, _numberOfRounds, _players);
            RoundOverPanel.Instance.Show();
            Painter.Instance.Lock();
            GameHUD.Instance.Hide();
        }
        else
        {
            NextPlayer();
            CurrentState = State.WaitingDiceRoll;
        }
    }

    IEnumerator AddScore(float scoreToAdd)
    {
        while (scoreToAdd > 0)
        {
            float change = Time.deltaTime * 150;
            scoreToAdd -= change;
            CurrentPlayer.Score += change;
            if (scoreToAdd < 0) CurrentPlayer.Score += scoreToAdd;
            if (OnPlayerScoreChanged != null) OnPlayerScoreChanged(CurrentPlayer, _playerIndex);
            yield return null;
        }
    }

    private IEnumerator RollDiceRotuine()
    {
        int loopCount = Random.Range(5, 7);
        const float minWaitInterval = 0.05f;
        const float maxWaitInterval = 0.5f;
        Beetle.Part dice;
        if (!debugMode)
        {
            dice = (Beetle.Part)Random.Range(1, 7);
            diceButton.GetComponent<Image>().color = Constants.DICE_ROLLING_COLOR;
            int previousRoll = 0;
            for (int i = 0; i < loopCount; i++)
            {
                int roll = Random.Range(1, 6);
                while (previousRoll == roll) roll = Random.Range(1, 7);
                previousRoll = roll;
                diceButton.GetComponentInChildren<Text>().text = Constants.GetBeetlePartText((Beetle.Part)roll);
                yield return new WaitForSeconds(Mathf.Lerp(minWaitInterval, maxWaitInterval, i / (float)loopCount));

            }
        }
        else
        {
            dice = (Beetle.Part)Random.Range(1, 7);
            while (!CurrentPlayer.PlayerBeetle.IsValid(dice))
            {
                dice = (Beetle.Part)Random.Range(1, 7);
            }
        }
        string partText = Constants.GetBeetlePartText(dice);
        diceButton.GetComponentInChildren<Text>().text = partText;

        if (CurrentPlayer.PlayerBeetle.TryAction(dice))
        {
            _successAudioSource.Play();
            diceButton.GetComponent<Image>().color = Constants.DICE_ROLL_VALID_COLOR;
            StartCoroutine(AddScore(50));
            GameHUD.Instance.ShowInfoText(50, true);
            GameHUD.Instance.ShowBeetlePartText(string.Format(Constants.DRAW_BEETLE_PART, (_playerIndex + 1), partText));
            if (debugMode) yield return null; else yield return new WaitForSeconds(1);
            CurrentState = State.Drawing;
        }
        else
        {
            diceButton.GetComponent<Image>().color = Constants.DICE_ROLL_INVALID_COLOR;
            if (debugMode) yield return null; else yield return new WaitForSeconds(1);
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