using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PrefabLoader
{
    public static GameObject LoadPrefab(string path, string fileName)
    {
        StringBuilder pathAndName = new StringBuilder();
        pathAndName.Append(path);
        pathAndName.Append(fileName);

        return Resources.Load<GameObject>(pathAndName.ToString());
    }
}
