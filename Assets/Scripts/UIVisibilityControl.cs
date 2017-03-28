using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVisibilityControl : MonoBehaviour
{
    private Text[] texts;
    private Image[] images;
    private RawImage[] rawImages;
    private Button[] buttons;
    private Toggle[] toggles;
    private Slider[] sliders;
    private Scrollbar[] scrollbars;
    private Dropdown[] dropdowns;
    private InputField[] inputFields;
    private ScrollRect[] scrollRects;

    private bool currentlyVisible = true;

    public bool CurrentlyVisible
    {
        get { return currentlyVisible; }
        set { currentlyVisible = value; }
    }

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Shows the UI.
    /// </summary>
    public void ShowUI()
    {
        #region Text
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].enabled = true;
        }
        #endregion

        #region Image
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = true;
        }
        #endregion

        #region RawImage
        for (int i = 0; i < rawImages.Length; i++)
        {
            rawImages[i].enabled = true;
        }
        #endregion

        #region Button
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }
        #endregion

        #region Toggle
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].enabled = true;
        }
        #endregion

        #region Sliders
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].enabled = true;
        }
        #endregion

        #region Scrollbar
        for (int i = 0; i < scrollbars.Length; i++)
        {
            scrollbars[i].enabled = true;
        }
        #endregion

        #region Dropdown
        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].enabled = true;
        }
        #endregion

        #region InputField
        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].enabled = true;
        }
        #endregion

        #region ScrollRect
        for (int i = 0; i < scrollRects.Length; i++)
        {
            scrollRects[i].enabled = true;
        }
        #endregion

        currentlyVisible = true;
    }

    /// <summary>
    /// Hides the UI.
    /// </summary>
    public void HideUI()
    {
        #region Text
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].enabled = false;
        }
        #endregion

        #region Image
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = false;
        }
        #endregion

        #region RawImage
        for (int i = 0; i < rawImages.Length; i++)
        {
            rawImages[i].enabled = false;
        }
        #endregion

        #region Button
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = false;
        }
        #endregion

        #region Toggle
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].enabled = false;
        }
        #endregion

        #region Sliders
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].enabled = false;
        }
        #endregion

        #region Scrollbar
        for (int i = 0; i < scrollbars.Length; i++)
        {
            scrollbars[i].enabled = false;
        }
        #endregion

        #region Dropdown
        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].enabled = false;
        }
        #endregion

        #region InputField
        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].enabled = false;
        }
        #endregion

        #region ScrollRect
        for (int i = 0; i < scrollRects.Length; i++)
        {
            scrollRects[i].enabled = false;
        }
        #endregion

        currentlyVisible = false;
    }

    /// <summary>
    /// Toggles the UI.
    /// </summary>
    public void ToggleUI()
    {
        if (currentlyVisible)
        {
            HideUI();
        }
        else
        {
            ShowUI();
        }
    }

    /// <summary>
    /// Initializes all the arrays with UI elements.
    /// </summary>
    private void Initialize()
    {
        #region Text
        // Finds all the Text components of the Button object.
        try
        {
            texts = GetComponentsInChildren<Text>();
        }
        catch
        {
            // If no Text components are found the array is set to the length of 0.
            texts = new Text[0];
        }
        #endregion

        #region Image
        try
        {
            images = GetComponentsInChildren<Image>();
        }
        catch
        {
            images = new Image[0];
        }
        #endregion

        #region RawImage
        try
        {
            rawImages = GetComponentsInChildren<RawImage>();
        }
        catch
        {
            rawImages = new RawImage[0];
        }
        #endregion

        #region Button
        try
        {
            buttons = GetComponentsInChildren<Button>();
        }
        catch
        {
            buttons = new Button[0];
        }
        #endregion

        #region Toggle
        try
        {
            toggles = GetComponentsInChildren<Toggle>();
        }
        catch
        {
            toggles = new Toggle[0];
        }
        #endregion

        #region Slider
        try
        {
            sliders = GetComponentsInChildren<Slider>();
        }
        catch
        {
            sliders = new Slider[0];
        }
        #endregion

        #region Scrollbar
        try
        {
            scrollbars = GetComponentsInChildren<Scrollbar>();
        }
        catch
        {
            scrollbars = new Scrollbar[0];
        }
        #endregion

        #region Dropdown
        try
        {
            dropdowns = GetComponentsInChildren<Dropdown>();
        }
        catch
        {
            dropdowns = new Dropdown[0];
        }
        #endregion

        #region InputField
        try
        {
            inputFields = GetComponentsInChildren<InputField>();
        }
        catch
        {
            inputFields = new InputField[0];
        }
        #endregion

        #region ScrollRect
        try
        {
            scrollRects = GetComponentsInChildren<ScrollRect>();
        }
        catch
        {
            scrollRects = new ScrollRect[0];
        }
        #endregion
    }

    /// <summary>
    /// Reinitializes all arrays.
    /// </summary>
    public void Reinitialize()
    {
        Initialize();
    }
}
