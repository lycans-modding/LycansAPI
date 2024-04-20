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
    private readonly static Dictionary<string, Dictionary<string, string>> _translationDicts = new();
    private const string STRING_TABLE = "UI Text";

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
            ReloadLocalizationForSelectedLocale(LocalizationSettings.SelectedLocale);
    }

    /// <summary>
    /// Ajoute une nouvelle entrée dans la table de langues pour toutes les langues
    /// </summary>
    /// <param name="uniqueKey">Clé unique à l'application qui identifie la traduction.</param>
    /// <param name="translation">Texte traduit, tel qu'il sera affiché en jeu.</param>
    public static void AddEntry(string uniqueKey, string translation)
        => AddEntry("generic", uniqueKey, translation);

    internal static void Hook()
    {
        LoadTranslationFiles();
        ReloadLocalizationForSelectedLocale(LocalizationSettings.SelectedLocale);
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    internal static void Unhook()
    {
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
        UnloadCurrentLocalization();
        _translationDicts.Clear();
    }

    private static void LocalizationSettings_SelectedLocaleChanged(Locale locale)
    {
        ReloadLocalizationForSelectedLocale(locale);
    }

    private static void ReloadLocalizationForSelectedLocale(Locale locale)
    {
        var lang = locale.Identifier.Code.ToLower();
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

    private static void UnloadCurrentLocalization()
    {
        // Dans le cas où on décharge l'api quand on quitte le jeu
        if (LocalizationSettings.SelectedLocale == null) return;

        var lang = LocalizationSettings.SelectedLocale.Identifier.Code.ToLower();
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