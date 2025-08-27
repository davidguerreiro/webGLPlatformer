using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    private string _path;

    /// <summary>
    /// Write list data into json.
    /// </summary>
    public void WriteDataInJson(LocalVars variables)
    {
        string variablesJson = JsonUtility.ToJson(variables);
        File.WriteAllText(_path, variablesJson);

    }

    /// <summary>
    /// Load saved data.
    /// </summary>
    /// <returns><LocalVars.LVars</returns>
    public LocalVars GetDataFromJson(LocalVars localVars)
    {
        if (SaveDataExists())
        {
            string jsonData = File.ReadAllText(_path);
            VarStructureArray varStructure = JsonUtility.FromJson<VarStructureArray>(jsonData);
            
            foreach(VarStructure data in varStructure.variables)
            {
                localVars.SetVar(data.name, bool.Parse(data.value));
            }

            return localVars;

        }

        return localVars;
    }

    /// <summary>
    /// Checks if save game data exists.
    /// </summary>
    /// <returns>bool</returns>
    public bool SaveDataExists()
    {
        return File.Exists(_path);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _path = Application.persistentDataPath + "/saveData.json";
    }
}

[System.Serializable]
public class VarStructure
{
    public string name;
    public string value;
}

[System.Serializable]
public class VarStructureArray
{
    public VarStructure[] variables;
}
