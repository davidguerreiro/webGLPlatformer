using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressSpaceBar : MonoBehaviour
{
    [Header("Settings")]
    public float interval;

    private Text _text;
    private Coroutine _animation;

    // Start is called before the first frame update
    void Start()
    {
        Init();    
    }

    private void Update()
    {
        if (_animation == null)
        {
            _animation = StartCoroutine(PressSpaceBarTextAnim());
        }
    }

    /// <summary>
    /// Press space bar animation.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator PressSpaceBarTextAnim()
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1f);
        yield return new WaitForSeconds(interval);

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
        yield return new WaitForSeconds(interval);

        _animation = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _text = GetComponent<Text>();
    }
}
