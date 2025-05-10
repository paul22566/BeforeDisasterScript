using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class SaveSystem : MonoBehaviour
{

    private string NameAndPath;

    public void CreatDirectory(string FilePath)
    {
        if (File.Exists(FilePath))
        {
            return;
        }
        Directory.CreateDirectory(FilePath);
    }

    private string SerializeObject(object PlayerData)
    {
        string SerializePlayerData = "";
        SerializePlayerData = JsonConvert.SerializeObject(PlayerData);
        return SerializePlayerData;
    }

    private static object DeserializeObject(string _PlayerData,Type _PlayerDataType)
    {
        object playerData = null;
        playerData = JsonConvert.DeserializeObject(_PlayerData, _PlayerDataType);
        return playerData;
    }

    public void SaveData(object Content, string SavingFileName)
    {
        string Content_string = SerializeObject(Content);
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";

        CreatDirectory(FilePath);
        NameAndPath = FilePath + "/" + SavingFileName;

        StreamWriter _StreamWriter = File.CreateText(NameAndPath);
        _StreamWriter.Write(Content_string);
        _StreamWriter.Close();
    }

    public void SaveData(object Content, int i)
    {
        string Content_string = SerializeObject(Content);
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";

        CreatDirectory(FilePath);
        switch (i)
        {
            case 1:
                NameAndPath = FilePath + "/" + "KeyCodeSave";
                break;
            case 2:
                NameAndPath = FilePath + "/" + "SoundVolumeSave";
                break;
        }

        StreamWriter _StreamWriter = File.CreateText(NameAndPath);
        _StreamWriter.Write(Content_string);
        _StreamWriter.Close();
    }

    public object LoadData(Type DataType, string SavingFileName)
    {
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        NameAndPath = FilePath + "/" + SavingFileName;

        StreamReader _StreamReader = File.OpenText(NameAndPath);
        string Data = _StreamReader.ReadToEnd();

        _StreamReader.Close();
        return DeserializeObject(Data, DataType);
    }

    public object LoadData(Type DataType, int i)
    {
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        switch (i)
        {
            case 1:
                NameAndPath = FilePath + "/" + "KeyCodeSave";
                break;
            case 2:
                NameAndPath = FilePath + "/" + "SoundVolumeSave";
                break;
        }

        StreamReader _StreamReader = File.OpenText(NameAndPath);
        string Data = _StreamReader.ReadToEnd();

        _StreamReader.Close();
        return DeserializeObject(Data, DataType);
    }
    
}
