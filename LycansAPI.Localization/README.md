# LycansAPI.Localization

## � propos

Module qui permet de rajouter de nouveaux textes et traductions � Lycans.

## Utilisation

Le module charge tous les fichier .json qui se trouvent dans le dossier `\plugins\*\lang\*.json`.
Leur contenu sert � remplir un dictionnaire de traductions.

Exemple:

Pour ajouter une traduction en fran�ais et en anglais il faut :

- Cr�er un dossier "lang" dans le dossier de votre mod (/plugins/[Nom de votre mode]/lang)
- Cr�er 2 fichiers json dans ce dossier "lang". Il faut les nommer fr.json et en.json, sinon les traductions ne seront pas charg�s
- Pour chaque cl� unique, cr�er les traductions dans le fichier JSON

fr.json :
```json
{
    "CLE_UNIQUE_1": "Ma traduction en fran�ais",
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

Il est possible d'ajouter une traduction � la vol�e en faisant appel � la fonction suivante :

```cs
LocalizationAPI.AddRecord("fr", "CLE_UNIQUE_1", "Ma traduction en fran�ais");
// Le "true" � la fin permet de recharger les traductions en jeu
LocalizationAPI.AddRecord("fr", "CLE_UNIQUE_2", "Une autre traduction", true);
```

Il est �galement possible d'ajouter un texte g�n�rique qui est tout le temps charg�, peu importe la langue soit en cr�ant
le fichier json "generic.json" soit en utilisant la fonction suivante :

```cs
LocalizationAPI.AddRecord("CLE_UNIQUE_3", "Cha�ne g�n�rique qui ne sera pas traduite");
```