using System;
using UnityEngine;
using System.Collections.Generic;

public class Player
{

    #region Enumerations

    #endregion

    #region Events and Delegates

    #endregion

    #region Variables
    [SerializeField]
    private readonly Beetle _beetle = new Beetle();
    private float _score;
    private readonly Texture2D _beetleCanvas;
    public readonly List<float> previousScores = new List<float>();

    #endregion

    #region Properties
    public Beetle PlayerBeetle
    {
        get { return _beetle; }
    }

    public float Score
    {
        get { return _score; }
        set { _score = value; }
    }

    public Texture2D BeetleCanvas
    {
        get { return _beetleCanvas; }
    }

    public void Reset()
    {
        _beetle.Reset();
        _score = 0;
        ClearBeetleImage();
    }

    public void ArchiveScore()
    {
        previousScores.Add(_score);
        _score = 0;
    }

    #endregion

    #region Methods

    public Player(Texture2D beetleCanvas)
    {
        _beetleCanvas = beetleCanvas;
    }

    public void ClearBeetleImage()
    {
        if (_beetleCanvas)
        {
            for (int x = 0; x < _beetleCanvas.width; x++)
            {
                for (int y = 0; y < _beetleCanvas.height; y++)
                {
                    _beetleCanvas.SetPixel(x, y, Color.white);
                }
            }
        }
    }

    public static implicit operator bool(Player instance)
    {
        return instance != null;
    }

    #endregion

    #region Structs

    #endregion

    #region Classes

    #endregion
}