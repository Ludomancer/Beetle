using UnityEngine;
using System.Collections.Generic;
 
internal class Player {

    #region Enumerations
 
    #endregion
 
    #region Events and Delegates
 
    #endregion
 
    #region Variables

    private readonly Beetle _beetle = new Beetle();
    private float _score;

    private readonly List<float> _previousScores = new List<float>(); 

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

    public void ArchiveScore()
    {
        _previousScores.Add(_score);
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