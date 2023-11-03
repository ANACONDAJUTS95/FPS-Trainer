using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] RectTransform uiHandleRectTransform;

    Toggle toggle;

    Vector2 handlePosition;

    //change color
    [SerializeField] Color backgroundActiveColor;
    [SerializeField] Color handleActiveColor;

    //Post Processing instance
    [SerializeField] PostProcessVolume postProcessVolume;


    Image backgroundImage, handleImage;

    Color backgroundDefaultColor, handleDefaultColor;


    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        handlePosition = uiHandleRectTransform.anchoredPosition;

        //change color
        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        // Load the switch state from PlayerPrefs
        bool isSwitchOn = PlayerPrefs.GetInt("SwitchState", 1) == 1;
        toggle.isOn = isSwitchOn;

        // Update the Post Processing effect based on the loaded state
        UpdatePostProcessingEffect(isSwitchOn);

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
        {
            OnSwitch(true);
        }
    }

    void OnSwitch(bool on)
    {
        if (on)
        {
            uiHandleRectTransform.anchoredPosition = handlePosition * -1;
            backgroundImage.color = backgroundActiveColor;
            handleImage.color = handleActiveColor;

            // Enable the Post Processing GameObject
            postProcessVolume.enabled = true;
        }

        else
        {
            uiHandleRectTransform.anchoredPosition = handlePosition;
            backgroundImage.color = backgroundDefaultColor;
            handleImage.color = handleDefaultColor;

            // Disable the Post Processing GameObject
            postProcessVolume.enabled = false;
        }

        // Update the Post Processing effect based on the switch state
        UpdatePostProcessingEffect(on);

        // Save the switch state to PlayerPrefs
        PlayerPrefs.SetInt("SwitchState", on ? 1 : 0);
        PlayerPrefs.Save();

        // Call the GameManager's function to update the Post Processing effect state
        GameManager.instance.UpdatePostProcessingEffectState();
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }

    void UpdatePostProcessingEffect(bool isSwitchOn)
    {
        // Enable or disable the Post Processing effect based on the switch state
        postProcessVolume.enabled = isSwitchOn;
    }
}
