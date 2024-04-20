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
```
{
	"CLE_UNIQUE_1": "My English translation",
	"CLE_UNIQUE_2": "Another translation"
}
```

Il est possible d'ajouter une traduction � la vol�e en faisant appel � la fonction suivante :

```cs
LocalizationAPI.AddRecord(<langue>, "<CLE_UNIQUE>", "Valeur en jeu");
```