using System;
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

    public Button diceButton;
    public Button drawingDoneButton;

    public Text[] playerScoreTexts;
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
                    break;
                case State.WaitingDiceRoll:
                    Painter.Instance.Lock();
                    diceButton.gameObject.SetActive(true);
                    drawingDoneButton.gameObject.SetActive(false);
                    break;
                case State.Drawing:
                    Painter.Instance.PaintCanvas = CurrentBeetleImage.sprite.texture;
                    Painter.Instance.Unlock(true);
                    diceButton.gameObject.SetActive(false);
                    drawingDoneButton.gameObject.SetActive(true);
                    break;
            }
        }
    }

    #endregion

    #region Methods

    private void Start()
    {
        CurrentState = State.None;
        for (int i = 0; i < _playerBeetleImages.Length; i++)
        {
            Texture2D tempTexture = Instantiate(emptyCanvasTexture);
            _playerBeetleImages[i].sprite = Sprite.Create(tempTexture, new Rect(0, 0, tempTexture.width, tempTexture.height), new Vector2(0.5f, 0.5f));
        }
        
    }

    private void Update()
    {

    }

    private int RollDice()
    {
        return 3;
        return Random.Range(1, 6);
    }

    private void NextPlayer()
    {
        _playerIndex++;
        if (_playerIndex >= _players.Length) _playerIndex = 0;
    }

    public void OnDiceRollClicked()
    {
        if (CurrentPlayer.PlayerBeetle.TryAction((Beetle.Action)RollDice()))
        {
            CurrentPlayer.Score += 50;
            CurrentScoreText.text = CurrentPlayer.Score.ToString("0");
            CurrentState = State.Drawing;
        }
    }

    public void OnDrawCompletedClicked()
    {
        Texture2D currentTexture2D = Painter.Instance.PaintCanvas;
        CurrentBeetleImage.sprite = Sprite.Create(currentTexture2D, new Rect(0, 0, currentTexture2D.width, currentTexture2D.height), new Vector2(0.5f, 0.5f));
        NextPlayer();
        CurrentState = State.WaitingDiceRoll;
    }

    #endregion

    #region Structs

    #endregion

    #region Classes

    #endregion
}