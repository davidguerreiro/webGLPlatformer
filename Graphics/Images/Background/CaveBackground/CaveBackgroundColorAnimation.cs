using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveBackgroundColorAnimation : MonoBehaviour
{
    [Header("Colors")]
    public Color[] colors;

    [Header("Settings")]
    public float timeBetweenChanges;

    private SpriteRenderer _renderer;
    private Coroutine _changingColors;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_changingColors == null)
        {
            _changingColors = StartCoroutine(ChangeBackgroundColour());
        }
    }

    /// <summary>
    /// Change background color for rendering.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ChangeBackgroundColour()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            _renderer.color = colors[i];
            yield return new WaitForSeconds(timeBetweenChanges);
        }

        for (int j = colors.Length - 1; j >= 0; j--)
        {
            _renderer.color = colors[j];
            yield return new WaitForSeconds(timeBetweenChanges);
        }

        _changingColors = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
}
