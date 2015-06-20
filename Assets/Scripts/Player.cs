using UnityEngine;
using System.Collections.Generic;
 
internal class Player {

    #region Enumerations
 
    #endregion
 
    #region Events and Delegates
 
    #endregion
 
    #region Variables

    private readonly Beetle _beetle = new Beetle();
    private int _score;

    #endregion

    #region Properties
    public Beetle PlayerBeetle
    {
        get { return _beetle; }
    }

    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }
    #endregion

    #region Methods

    #endregion

    #region Structs

    #endregion

    #region Classes

    #endregion
}