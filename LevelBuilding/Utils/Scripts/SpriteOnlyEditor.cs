using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOnlyEditor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RemoveSprite();   
    }

    /// <summary>
    /// Remove sprite when this script is attached to
    /// a gameObject with an sprite renderer component.
    /// </summary>
    private void RemoveSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = null;
        }
    }
}
