using System;
using UnityEngine;
using System.Collections.Generic;

internal static class Constants
{
    #region Enumerations

    #endregion

    #region Events and Delegates

    #endregion

    #region Variables

    internal static Color DICE_DEFAULT_COLOR = new Color(1, 1, 1);
    internal static Color DICE_ROLLING_COLOR = new Color(0.8f, 0.8f, 0.8f);
    internal static Color DICE_ROLL_VALID_COLOR = new Color(0,1,0);
    internal static Color DICE_ROLL_INVALID_COLOR = new Color(1,0,0);

    internal static Color ROUND_PANEL_WINNING_COLOR = new Color(0, 1f, 0,0.5f);
    internal static Color ROUND_PANEL_NORMAL_COLOR = new Color(1, 1, 1);

    internal const string NEXT_ROUND = "Next Round";
    internal const string RESTART = "Restart";
    internal const string ROUND_TEXT = "Round {0} of {1}";
    internal const string PLAYER_TURN_TEXT = "Player {0}'s Turn";
    internal const string DRAW_BEETLE_PART = "Player {0}: Draw a Beetle {1}";
    internal const string LEG_TEXT = "Leg";
    internal const string HEAD_TEXT = "Head";
    internal const string BODY_TEXT = "Body";
    internal const string ANTENNA_TEXT = "Antenna";
    internal const string EYE_TEXT = "Eye";
    internal const string WING_TEXT = "Wing";
    internal const string ROLL_DICE_TEXT = "Roll Dice";
    internal const string DONE_DRAWING_TEXT = "Done!";
    internal const string DRAWN_TEXT = " <color=#008000ff>Drawn</color>";
    internal const string NOT_DRAWN_TEXT = "Not Drawn";
    #endregion

    #region Properties

    #endregion

    #region Methods

    public static string GetBeetlePartText(Beetle.Part part)
    {
        switch (part)
        {
            case Beetle.Part.Leg: return LEG_TEXT;
            case Beetle.Part.Head: return HEAD_TEXT;
            case Beetle.Part.Body: return BODY_TEXT;
            case Beetle.Part.Antenna: return ANTENNA_TEXT;
            case Beetle.Part.Eye: return EYE_TEXT;
            case Beetle.Part.Wing: return WING_TEXT;
            default: return "";
        }
    }

    #endregion

    #region Structs

    #endregion

    #region Classes

    #endregion
}