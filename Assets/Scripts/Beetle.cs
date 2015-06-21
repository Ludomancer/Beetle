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
        get { return LegsLeft == 0 && AntennasLeft == 0 && EyesLeft == 0 && IsHeadDrawn && IsBodyDrawn && WingsLeft == 0; }
    }

    public int LegsLeft
    {
        get { return _legsLeft; }
    }

    public bool IsHeadDrawn
    {
        get { return _isHeadDrawn; }
    }

    public bool IsBodyDrawn
    {
        get { return _isBodyDrawn; }
    }

    public int AntennasLeft
    {
        get { return _antennasLeft; }
    }

    public int EyesLeft
    {
        get { return _eyesLeft; }
    }

    public int WingsLeft
    {
        get { return _wingsLeft; }
    }

    #endregion

    #region Methods

    //For debug purposes
    public bool IsValid(Part part)
    {
        switch (part)
        {
            case Part.Leg:
                if (IsBodyDrawn && LegsLeft > 0)
                {
                    return true;
                }
                return false;
            case Part.Head:
                if (IsBodyDrawn && !IsHeadDrawn)
                {
                    return true;
                }
                return false;
            case Part.Body:
                if (!IsBodyDrawn)
                {
                    return true;
                }
                return false;
            case Part.Antenna:
                if (IsBodyDrawn && IsHeadDrawn && AntennasLeft > 0)
                {
                    return true;
                }
                return false;
            case Part.Eye:
                if (IsBodyDrawn && IsHeadDrawn && EyesLeft > 0)
                {
                    return true;
                }
                return false;
            case Part.Wing:
                if (IsBodyDrawn && WingsLeft > 0)
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
        Debug.Log("_legsLeft: " + LegsLeft);
        Debug.Log("_isHeadDrawn: " + IsHeadDrawn);
        Debug.Log("_isBodyDrawn: " + IsBodyDrawn);
        Debug.Log("_antennasLeft: " + AntennasLeft);
        Debug.Log("_eyesLeft: " + EyesLeft);
        Debug.Log("_wingsLeft: " + WingsLeft);
        Debug.Log("----------------------------------");
        switch (part)
        {
            case Part.Leg:
                if (IsBodyDrawn && LegsLeft > 0)
                {
                    _legsLeft = LegsLeft - 1;
                    return true;
                }
                return false;
            case Part.Head:
                if (IsBodyDrawn && !IsHeadDrawn)
                {
                    _isHeadDrawn = true;
                    return true;
                }
                return false;
            case Part.Body:
                if (!IsBodyDrawn)
                {
                    _isBodyDrawn = true;
                    return true;
                }
                return false;
            case Part.Antenna:
                if (IsBodyDrawn && IsHeadDrawn && AntennasLeft > 0)
                {
                    _antennasLeft = AntennasLeft - 1;
                    return true;
                }
                return false;
            case Part.Eye:
                if (IsBodyDrawn && IsHeadDrawn && EyesLeft > 0)
                {
                    _eyesLeft = EyesLeft - 1;
                    return true;
                }
                return false;
            case Part.Wing:
                if (IsBodyDrawn && WingsLeft > 0)
                {
                    _wingsLeft = WingsLeft - 1;
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