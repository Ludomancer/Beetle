using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

internal class Beetle
{

    #region Enumerations
    internal enum Part
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

    public bool TryAction(Part part)
    {
        Debug.Log(part);
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