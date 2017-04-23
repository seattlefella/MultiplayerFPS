using System;
using UnityEngine;

public class DataTranslator : MonoBehaviour
{
    private static string KILL_TAG = "[KILLS]";
    private static string DEATH_TAG = "[DEATHS]";

    public static int DataToKills(string _data)
    {
        return int.Parse(dataToValue(_data, KILL_TAG));
    }

    public static int DataToDeaths(string _data)
    {
        return int.Parse(dataToValue(_data, DEATH_TAG));
    }

    private static string dataToValue(string _data, string _tag)
    {
        string[] elements = _data.Split('/');

        foreach (string s in elements)
        {
            if (s.StartsWith(_tag))
            {

               return (s.Substring(_tag.Length));
            }

        }

        Debug.Log("Symbol was not found in the data-string: " + _data);
        return "";
    }


    public static string ValueToData(int _kills, int _deaths)
    {
        return (KILL_TAG + _kills.ToString() + "/" + DEATH_TAG + _deaths.ToString());
    }

}
