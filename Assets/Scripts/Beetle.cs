using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
public class Beetle
{

    #region Enumerations
    public enum Part
    {
        Leg = 1,
        Head = 2,
        Body = 3,
        Antenna = 4,
        Eye = 5,
        Wing = 6
    }

    #endregion

    #region Events and Delegates

    #endregion

    #region Variables

    private int _legsLeft = 4;
    private bool _isHeadDrawn = false;
    private bool _isBodyDrawn = false;
    private int _antennasLeft = 2;
    private int _eyesLeft = 2;
    private int _wingsLeft = 2;
    #endregion

    #region Properties

    public bool IsCompleted
    {
        get { return _legsLeft == 0 && _antennasLeft == 0 && _eyesLeft == 0 && _isHeadDrawn && _isBodyDrawn && _wingsLeft == 0; }
    }

    #endregion

    #region Methods

    //For debug purposes
    public bool IsValid(Part part)
    {
        switch (part)
        {
            case Part.Leg:
                if (_isBodyDrawn && _legsLeft > 0)
                {
                    return true;
                }
                return false;
            case Part.Head:
                if (_isBodyDrawn && !_isHeadDrawn)
                {
                    return true;
                }
                return false;
            case Part.Body:
                if (!_isBodyDrawn)
                {
                    return true;
                }
                return false;
            case Part.Antenna:
                if (_isBodyDrawn && _isHeadDrawn && _antennasLeft > 0)
                {
                    return true;
                }
                return false;
            case Part.Eye:
                if (_isBodyDrawn && _isHeadDrawn && _eyesLeft > 0)
                {
                    return true;
                }
                return false;
            case Part.Wing:
                if (_isBodyDrawn && _wingsLeft > 0)
                {
                    return true;
                }
                return false;
            default: return false;
        }
    }

    public void Reset()
    {
        _legsLeft = 4;
        _isHeadDrawn = false;
        _isBodyDrawn = false;
        _antennasLeft = 2;
        _eyesLeft = 2;
        _wingsLeft = 2;
    }

    public bool TryAction(Part part)
    {
        Debug.Log("Try: " + part);
        Debug.Log("_legsLeft: " + _legsLeft);
        Debug.Log("_isHeadDrawn: " + _isHeadDrawn);
        Debug.Log("_isBodyDrawn: " + _isBodyDrawn);
        Debug.Log("_antennasLeft: " + _antennasLeft);
        Debug.Log("_eyesLeft: " + _eyesLeft);
        Debug.Log("_wingsLeft: " + _wingsLeft);
        Debug.Log("----------------------------------");
        switch (part)
        {
            case Part.Leg:
                if (_isBodyDrawn && _legsLeft > 0)
                {
                    _legsLeft--;
                    return true;
                }
                return false;
            case Part.Head:
                if (_isBodyDrawn && !_isHeadDrawn)
                {
                    _isHeadDrawn = true;
                    return true;
                }
                return false;
            case Part.Body:
                if (!_isBodyDrawn)
                {
                    _isBodyDrawn = true;
                    return true;
                }
                return false;
            case Part.Antenna:
                if (_isBodyDrawn && _isHeadDrawn && _antennasLeft > 0)
                {
                    _antennasLeft--;
                    return true;
                }
                return false;
            case Part.Eye:
                if (_isBodyDrawn && _isHeadDrawn && _eyesLeft > 0)
                {
                    _eyesLeft--;
                    return true;
                }
                return false;
            case Part.Wing:
                if (_isBodyDrawn && _wingsLeft > 0)
                {
                    _wingsLeft--;
                    return true;
                }
                return false;
            default: return false;
        }
    }

    #endregion

    #region Structs

    #endregion

    #region Classes

    #endregion
}