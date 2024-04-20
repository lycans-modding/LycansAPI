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
```
{
	"CLE_UNIQUE_1": "My English translation",
	"CLE_UNIQUE_2": "Another translation"
}
```

Il est possible d'ajouter une traduction à la volée en faisant appel à la fonction suivante :

```cs
LocalizationAPI.AddRecord(<langue>, "<CLE_UNIQUE>", "Valeur en jeu");
```