# LycansAPI.Localization

## � propos

Module qui permet de rajouter de nouveaux textes et traductions � Lycans.

## Utilisation

Le module charge tous les fichier .json qui se trouvent dans le dossier `\plugins\*\lang\*.json`.
Leur contenu sert � remplir un dictionnaire de traductions.

Il est possible d'ajouter une traduction � la vol�e en faisant appel � la fonction suivante :

```cs
LocalizationAPI.AddRecord(<langue>, "<CLE_UNIQUE>", "Valeur en jeu");
```