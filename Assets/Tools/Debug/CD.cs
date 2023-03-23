using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CD
{
    #region HelperVariables
    public enum Programmers
    {
        DANIEL,
        JUSTIN,
        CORY,
        BEN,
        NULL
    }

    public static Dictionary<Programmers, Color> ProgrammerColors = new Dictionary<Programmers, Color>()
    {
        {Programmers.DANIEL, UnityEngine.Color.red},
        {Programmers.JUSTIN, UnityEngine.Color.blue},
        {Programmers.CORY, UnityEngine.Color.green},
        {Programmers.BEN, UnityEngine.Color.yellow}
    };
    #endregion

    #region General Log Statements

    public static void Log(string outputText)
    {
        Debug.Log(outputText);
    }

    public static void Log(string outputText, Color color)
    {
        Debug.Log(outputText.Color(color));
    }
    
    public static void Log(Programmers person, string outputText)
    {
        string outputString = $"{PersonColor(person)} : {outputText}";
        Debug.Log(outputString);
    }
    
    public static void Log(Programmers person, string outputText, Color color)
    {
        string outputString = $"{PersonColor(person)} : {outputText.Color(color)}";
        Debug.Log(outputString);
    }

    public static void Log(Programmers person, DebugCategories category, string outputText, Color color)
    {
        string outputString = $"{PersonColor(person)} : {category.name.Color(category.color)} : {outputText.Color(color)}";
        Debug.Log(outputString);
    }

    #endregion

    #region TypedOutputs

    public static void LogBool(bool outputVar, string varName, Programmers person = Programmers.NULL)
    {
        string outputString = "";
        if (person != Programmers.NULL)
        {
            outputString += $"{PersonColor(person)} : ";
        }
        outputString += $"Bool Variable Name: {varName}, Value: {outputVar}";
        Debug.Log(outputString);
    }
    
    public static void LogInt(int outputVar, string varName, Programmers person = Programmers.NULL)
    {
        string outputString = "";
        if (person != Programmers.NULL)
        {
            outputString += $"{PersonColor(person)} : ";
        }
        outputString += $"Int Variable Name: {varName}, Value: {outputVar}";
        Debug.Log(outputString);
    }
    public static void LogFloat(float outputVar, string varName, Programmers person = Programmers.NULL)
    {
        string outputString = "";
        if (person != Programmers.NULL)
        {
            outputString += $"{PersonColor(person)} : ";
        }
        outputString += $"Float Variable Name: {varName}, Value: {outputVar}";
        Debug.Log(outputString);
    }
    public static void LogString(string outputVar, string varName, Programmers person = Programmers.NULL)
    {
        string outputString = "";
        if (person != Programmers.NULL)
        {
            outputString += $"{PersonColor(person)} : ";
        }
        outputString += $"String Variable Name: {varName}, Value: {outputVar}";
        Debug.Log(outputString);
    }

    #endregion
    
    #region Helper Functions

    private static string ToHex(this Color c) => string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));

    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return(byte) (f * 255);
    }

    public static string Color(this string text, Color color)
    {
        string output = string.Format("<color={0}>{1}</color>", color.ToHex(), text);
        return output;
    }

    private static string PersonColor(Programmers p)
    {
        return p.ToString().Color(ProgrammerColors[p]);
    }

    #endregion
}
