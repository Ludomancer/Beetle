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

    public void Reset()
    {
        _beetle.Reset();
        _score = 0;
    }

    public void ArchiveScore()
    {
        previousScores.Add(_score);
        _score = 0;
    }

    #endregion

    #region Methods

    #endregion

    #region Structs

    #endregion

    #region Classes

    #endregion
}