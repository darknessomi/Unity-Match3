using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Player
{
    public int Level;
    public string Name;
    public bool Locked;
    public int Stars;
    public int HightScore;
    public int Background;
    public string ToSaveString()
    {
        string s = Locked + "," + Stars + "," + HightScore + "," + Background + ",";
        return s;
    }
}

public class PlayerUtils
{
    private string KEY_DATA = "DATA";
    private string data = "";
    private string[] dataSplit;
    private Player p;
    public void Save(List<Player> Maps)
    {
        //PlayerPrefs.DeleteKey(KEY_DATA);
        foreach (var item in Maps)
        {
            data += item.ToSaveString();
        }
        PlayerPrefs.SetString(KEY_DATA, data);
    }

    /// <summary>
    /// Load data load by PlayerPrefs, set to buttons level on map scene 
    /// </summary>
    /// <returns></returns>
    public List<Player> Load()
    {
        List<Player> list = new List<Player>();

        string data = PlayerPrefs.GetString(KEY_DATA, "");

        dataSplit = data.Split(',');

        for (int i = 0; i < 40; i++)
        {
            p = new Player();
            p.Level = i + 1;
            p.Name = (i + 1).ToString();
            p.Locked = bool.Parse(dataSplit[i * 4]);
            p.Stars = int.Parse(dataSplit[i * 4 + 1]);
            p.HightScore = int.Parse(dataSplit[i * 4 + 2]);
            p.Background = int.Parse(dataSplit[i * 4 + 3]);
            list.Add(p);
        }

        return list;
    }
}

