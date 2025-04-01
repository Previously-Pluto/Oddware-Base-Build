using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public static class StringEntryProcessor
{
    public static string GetLocalizedString(string tableName, string entry)
    {
        LocalizedString newLocalizedString = new LocalizedString(tableName, entry);
        return newLocalizedString.GetLocalizedString();
    }
}
