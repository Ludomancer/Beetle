using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

internal class GameHUD : MonoBehaviour, IPanel
{
    #region Singleton

    private static GameHUD instance;

    internal static GameHUD Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType(typeof(GameHUD)) as GameHUD;
            if (instance != null) return instance;

            var container = new GameObject("GameHUD");
            instance = container.AddComponent<GameHUD>();
            return instance;
        }
    }

    #endregion

    #region Enumerations

    #endregion

    #region Events and Delegates

    #endregion

    #region Variables
    private const string INFO_TEXT_LEFT_ALIGN_FORMAT = "{0}: {1}";
    private const string INFO_TEXT_RIGHT_ALIGN_FORMAT = "{1} :{0}";
    private const string NUMBER_FORMAT = "0";
    [SerializeField]
    private PlayerUI[] _playerUiObjects;
    [SerializeField]
    private Text infoText;
    [SerializeField]
    private Text drawBeetlePartText;
    #endregion

    #region Properties

    #endregion

    #region Methods

    private void Awake()
    {
        GameManager.Instance.OnPlayerBeetleChanged += OnPlayerBeetleChanged;
        GameManager.Instance.OnPlayerScoreChanged += OnPlayerScoreChanged;
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(Player currentPlayer,int playerIndex, GameManager.State state)
    {
        switch (state)
        {
            case GameManager.State.None:
                HideBeetlePartText();
                HideInfoText();
                break;
            case GameManager.State.WaitingDiceRoll:
                HideBeetlePartText();
                ShowInfoText(string.Format(Constants.PLAYER_TURN_TEXT, (playerIndex + 1)));
                break;
            case GameManager.State.Drawing:
                HideInfoText();
                break;
        }
    }

    private void OnPlayerScoreChanged(Player player, int playerIndex)
    {
        _playerUiObjects[playerIndex].scoreText.text = player.Score.ToString("0");
    }

    private void OnPlayerBeetleChanged(Player player, int playerIndex)
    {
        string infoFormatting = _playerUiObjects[playerIndex].infoText.alignment == TextAnchor.UpperLeft
            ? INFO_TEXT_LEFT_ALIGN_FORMAT
            : INFO_TEXT_RIGHT_ALIGN_FORMAT;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Format(infoFormatting, Constants.BODY_TEXT, player.PlayerBeetle.IsBodyDrawn ? Constants.DRAWN_TEXT : Constants.NOT_DRAWN_TEXT))
            .AppendLine(string.Format(infoFormatting, Constants.HEAD_TEXT, player.PlayerBeetle.IsHeadDrawn ? Constants.DRAWN_TEXT : Constants.NOT_DRAWN_TEXT))
            .AppendLine(string.Format(infoFormatting, Constants.EYE_TEXT, player.PlayerBeetle.EyesLeft == 0 ? Constants.DRAWN_TEXT : player.PlayerBeetle.EyesLeft.ToString(NUMBER_FORMAT)))
            .AppendLine(string.Format(infoFormatting, Constants.ANTENNA_TEXT, player.PlayerBeetle.AntennasLeft == 0 ? Constants.DRAWN_TEXT : player.PlayerBeetle.AntennasLeft.ToString(NUMBER_FORMAT)))
            .AppendLine(string.Format(infoFormatting, Constants.LEG_TEXT, player.PlayerBeetle.LegsLeft == 0 ? Constants.DRAWN_TEXT : player.PlayerBeetle.LegsLeft.ToString(NUMBER_FORMAT)))
            .AppendLine(string.Format(infoFormatting, Constants.WING_TEXT, player.PlayerBeetle.WingsLeft == 0 ? Constants.DRAWN_TEXT : player.PlayerBeetle.WingsLeft.ToString(NUMBER_FORMAT)));
        PlayerUI uiRef = _playerUiObjects[playerIndex];
        uiRef.infoText.text = sb.ToString();
        uiRef.beetleImage.sprite = Sprite.Create(player.BeetleCanvas, new Rect(0, 0, player.BeetleCanvas.width, player.BeetleCanvas.height), new Vector2(0.5f, 0.5f));
    }

    public void ShowInfoText(string text, bool animate = false)
    {
        if (!infoText.gameObject.activeSelf) infoText.gameObject.SetActive(true);
        infoText.text = text;
        if (animate) infoText.GetComponent<Animator>().SetTrigger("Spawn");
    }

    public void ShowInfoText(float text, bool animate = false)
    {
        ShowInfoText(text.ToString(NUMBER_FORMAT), animate);
    }

    public void HideInfoText()
    {
        infoText.text = string.Empty;
        infoText.gameObject.SetActive(false);
    }

    public void ShowBeetlePartText(string text)
    {
        if (!drawBeetlePartText.gameObject.activeSelf) drawBeetlePartText.gameObject.SetActive(true);
        drawBeetlePartText.text = text;
    }

    public void HideBeetlePartText()
    {
        drawBeetlePartText.text = string.Empty;
        drawBeetlePartText.gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    #endregion

    #region Structs

    #endregion

    #region Classes

    [Serializable]
    internal class PlayerUI
    {
        public Image beetleImage;
        public Text scoreText;
        public Text infoText;
    }

    #endregion
}