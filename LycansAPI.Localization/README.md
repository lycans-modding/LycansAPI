# LycansAPI.Localization

## À propos

Module qui permet de rajouter de nouveaux textes et traductions à Lycans.

## Utilisation

Le module charge tous les fichier .json qui se trouvent dans le dossier `\plugins\*\lang\*.json`.
Leur contenu sert à remplir un dictionnaire de traductions.

Exemple:

Pour ajouter une traduction en français et en anglais il faut :

- Créer un dossier "lang" dans le dossier de votre mod (/plugins/[Nom de votre mode]/lang)
- Créer 2 fichiers json dans ce dossier "lang". Il faut les nommer fr.json et en.json, sinon les traductions ne seront pas chargés
- Pour chaque clé unique, créer les traductions dans le fichier JSON

fr.json :
```json
{
    "CLE_UNIQUE_1": "Ma traduction en français",
    "CLE_UNIQUE_2": "Une autre traduction"
}
```

en.json :
```json
{
    "CLE_UNIQUE_1": "My English translation",
    "CLE_UNIQUE_2": "Another translation"
}
```

Il est possible d'ajouter une traduction à la volée en faisant appel à la fonction suivante :

```cs
LocalizationAPI.AddRecord("fr", "CLE_UNIQUE_1", "Ma traduction en français");
// Le "true" à la fin permet de recharger les traductions en jeu
LocalizationAPI.AddRecord("fr", "CLE_UNIQUE_2", "Une autre traduction", true);
```

Il est également possible d'ajouter un texte générique qui est tout le temps chargé, peu importe la langue soit en créant
le fichier json "generic.json" soit en utilisant la fonction suivante :

```cs
LocalizationAPI.AddRecord("CLE_UNIQUE_3", "Chaîne générique qui ne sera pas traduite");
```