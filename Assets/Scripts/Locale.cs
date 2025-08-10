using UnityEngine.Localization.Settings;

public static class Locale
{
    public static string Get(string key, string table = "Default")
    {
        var stringTable = LocalizationSettings.StringDatabase.GetTable(table);
        if (stringTable == null) return $"null : {table}";
            
        var entry = stringTable.GetEntry(key);
        return entry?.GetLocalizedString() ?? $"null : {key}>";
    }
}