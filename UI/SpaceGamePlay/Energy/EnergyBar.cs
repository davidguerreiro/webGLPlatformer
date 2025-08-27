using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [Header("Colors")]
    public Color enabledColor;
    public Color disabledColor;

    private Image image;

    /// <summary>
    /// Enable bar.
    /// </summary>
    public void EnableBar()
    {
        image.color = enabledColor;
    }

    /// <summary>
    /// Disable bar.
    /// </summary>
    public void DisableBar()
    {
        image.color = disabledColor;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        image = GetComponent<Image>();
    }
}
