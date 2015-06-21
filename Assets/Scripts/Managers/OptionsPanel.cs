using UnityEngine;
using System.Collections.Generic;
 
internal class OptionsPanel : MonoBehaviour , IPanel {
    #region Singleton
 
    private static OptionsPanel instance;
 
    internal static OptionsPanel Instance {
        get {
            if(instance != null) return instance;
 
            instance = FindObjectOfType(typeof(OptionsPanel)) as OptionsPanel;
            if(instance != null) return instance;
 
            var container = new GameObject("OptionsPanel");
            instance = container.AddComponent<OptionsPanel>();
            return instance;
        }
    }
 
    #endregion
 
    #region Enumerations
 
    #endregion
 
    #region Events and Delegates
 
    #endregion
 
    #region Variables
    #endregion
 
    #region Properties
 
    #endregion
 
    #region Methods
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnDevelopmentModeToggled(bool state)
    {
        FindObjectOfType<GameManager>().debugMode = state;
    }

    public void OnAudioModeToggled(bool state)
    {
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.mute = !state;
        }
    }

    public void OnHelpClicked()
    {
        HelpPanel.Instance.Show();
    }

    #endregion
 
    #region Structs
 
    #endregion
 
    #region Classes
 
    #endregion
}