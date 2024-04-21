using BepInEx;
using LycansAPI.Core;
using LycansAPI.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace LycansAPI.Localization;

public static class LocalizationAPI
{
    public const string PLUGIN_GUID = LMAPI.PLUGIN_GUID + ".localization";
    public const string PLUGIN_NAME = LMAPI.PLUGIN_NAME + ".Localization";
    public const string PLUGIN_VERSION = LMAPI.PLUGIN_VERSION;

    private readonly static Dictionary<string, Dictionary<string, string>> _translationDicts = new();
    private const string STRING_TABLE = "UI Text";
    private const string GENERIC_LANG = "generic";

    /// <summary>
    /// Ajoute une nouvelle entrée dans la table de langues pour une langue spécifique.
    /// </summary>
    /// <param name="lang">Nom de la langue pour laquelle ajouter une traduction.</param>
    /// <param name="uniqueKey">Clé unique à l'application qui identifie la traduction.</param>
    /// <param name="translation">Texte traduit, tel qu'il sera affiché en jeu.</param>
    /// <param name="reloadTranslation">Si "True", recharge l'ensemble des traductions dynamiquement.</param>
    public static void AddEntry(string lang,  string uniqueKey, string translation, bool reloadTranslation = false)
    {
        if (_translationDicts.TryGetValue(lang, out var translationDict))
        {
            try
            {
                translationDict.Add(uniqueKey, translation);
            }
            catch (Exception e)
            {
                Log.Error($"Unable to add '{uniqueKey}' to '{lang}' - exception:");
                Log.Error(e);
            }
        }
        else
        {
            _translationDicts.Add(lang, new()
            {
                { uniqueKey, translation }
            });
        }

        if (reloadTranslation)
            ReloadLocalization(LocalizationSettings.SelectedLocale);
    }

    /// <summary>
    /// Ajoute une nouvelle entrée dans la table de langues pour toutes les langues
    /// </summary>
    /// <param name="uniqueKey">Clé unique à l'application qui identifie la traduction.</param>
    /// <param name="translation">Texte traduit, tel qu'il sera affiché en jeu.</param>
    public static void AddEntry(string uniqueKey, string translation)
        => AddEntry(GENERIC_LANG, uniqueKey, translation);

    internal static void Hook()
    {
        LoadTranslationFiles();
        ReloadLocalization(LocalizationSettings.SelectedLocale);
        ReloadLocalization(GENERIC_LANG);
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    internal static void Unhook()
    {
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
        UnloadLocalization(GENERIC_LANG);
        UnloadLocalization(LocalizationSettings.SelectedLocale);
        _translationDicts.Clear();
    }

    private static void LocalizationSettings_SelectedLocaleChanged(Locale locale)
    {
        ReloadLocalization(locale);
        ReloadLocalization(GENERIC_LANG);
    }

    private static void ReloadLocalization(Locale locale)
        => ReloadLocalization(locale.Identifier.Code.ToLower());

    private static void ReloadLocalization(string lang)
    {
        var table = LocalizationSettings.StringDatabase.GetTable(STRING_TABLE);

        if (_translationDicts.TryGetValue(lang, out var translationDict))
        {
            translationDict.ForEach(kvp =>
            {
                table.AddEntry(kvp.Key, kvp.Value);
            });
        }
        else
        {
            Log.Warning($"No translation dictionnary found for lang '{lang}'");
        }
    }

    private static void UnloadLocalization(Locale locale)
        => UnloadLocalization(locale.Identifier.Code.ToLower());

    private static void UnloadLocalization(string lang)
    {
        // Dans le cas où on décharge l'api quand on quitte le jeu
        if (LocalizationSettings.SelectedLocale == null) return;

        var table = LocalizationSettings.StringDatabase.GetTable(STRING_TABLE);

        if (_translationDicts.TryGetValue(lang, out var translationDict))
        {
            translationDict.ForEach((kvp) =>
            {
                table.RemoveEntry(kvp.Key);
            });
        }
    }

    private static void LoadTranslationFile(string filePath)
    {
        var lang = Path.GetFileNameWithoutExtension(filePath);
        var json = File.ReadAllText(filePath);
        var newTranslations = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        var translationDict = _translationDicts.ContainsKey(lang) ? _translationDicts[lang] : new Dictionary<string, string>();

        _translationDicts[lang] = translationDict
            .Concat(newTranslations.Where(kvp => !translationDict.ContainsKey(kvp.Key)))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private static void LoadTranslationFiles()
    {
        var translationFiles = GetTranslationFiles();

        try
        {
            translationFiles.ForEach(LoadTranslationFile);
        }
        catch (Exception e)
        {
            Log.Error("An error occured when parsing translation files - exception:");
            Log.Error(e);
        }
    }

    private static List<string> GetTranslationFiles()
    {
        try
        {
            var directories =  Directory.GetDirectories(Paths.PluginPath, "lang", SearchOption.AllDirectories);
            var translationFiles = new List<string>();

            directories.ForEach(dir =>
            {
                translationFiles.AddRange(Directory.GetFiles(dir));
            });
            
            return translationFiles;
        }
        catch(Exception e)
        {
            Log.Error("An error occured when retrieving translation files - exception:");
            Log.Error(e);
        }

        return new();
    }
}