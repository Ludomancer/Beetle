using UnityEngine;
using System.Collections.Generic;
 
internal class HelpPanel : MonoBehaviour,IPanel {
    #region Singleton
 
    private static HelpPanel instance;
 
    internal static HelpPanel Instance {
        get {
            if(instance != null) return instance;
 
            instance = FindObjectOfType(typeof(HelpPanel)) as HelpPanel;
            if(instance != null) return instance;
 
            var container = new GameObject("HelpPanel");
            instance = container.AddComponent<HelpPanel>();
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

    private void Awake()
    {
        instance = this;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnResumeClicked()
    {
        Hide();
    }
    #endregion
 
    #region Structs
 
    #endregion
 
    #region Classes
 
    #endregion
}