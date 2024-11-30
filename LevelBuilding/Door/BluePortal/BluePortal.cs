using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DarkPortal))]
public class BluePortal : MonoBehaviour
{
    public GameManager gameManager;

    private bool _closed;
    private DarkPortal _portal;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_closed && gameManager.player.key)
        {
            OpenBluePortal();
        }
    }

    /// <summary>
    /// Open blue portal.
    /// </summary>
    private void OpenBluePortal()
    {
        _closed = false;
        _portal.Appear();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _closed = true;
        _portal = GetComponent<DarkPortal>();
    }
}
