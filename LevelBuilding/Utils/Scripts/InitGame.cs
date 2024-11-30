using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    public LocalVars gameVars;

    private SaveGame _saveGame;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        InitGameProcedures();
        CreateSaveData();
    }

    /// <summary>
    /// Procedures to be executed during game initialization
    /// at splash screen, which is the very first scene loaded
    /// when booting the game.
    /// </summary>
    public void InitGameProcedures()
    {
        DisableCursor();
    }

    /// <summary>
    /// Disable PC mouse cursor during game execution.
    /// </summary>
    public void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Create save game data if needed.
    /// </summary>
    public void CreateSaveData()
    {
        if (! _saveGame.SaveDataExists())
        {
            _saveGame.WriteDataInJson(gameVars);
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _saveGame = GetComponent<SaveGame>();
        _saveGame.Init();
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
    }
}
