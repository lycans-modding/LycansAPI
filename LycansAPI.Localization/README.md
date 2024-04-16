# LycansAPI.Localization

## À propos

Module qui permet de rajouter de nouveaux textes et traductions à Lycans.

## Utilisation

Le module charge tous les fichier .json qui se trouvent dans le dossier `\plugins\*\lang\*.json`.
Leur contenu sert à remplir un dictionnaire de traductions.

Il est possible d'ajouter une traduction à la volée en faisant appel à la fonction suivante :

```cs
LocalizationAPI.AddRecord(<langue>, "<CLE_UNIQUE>", "Valeur en jeu");
```