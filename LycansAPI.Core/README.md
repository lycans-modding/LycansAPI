# LycansAPI.Core

## À propos

Module principal de l'API, réunis tout ce qui peut être commun aux autres modules de l'API.

## Utilisation

Le module met à disposition un logger global qui peut être utilisé dans l'ensemble de l'API comme suit :

```cs
Log.Debug("Hello")
Log.Info("Hello")
Log.Warning("Hello")
Log.Error("Hello")
```

L'API met également à disposition quelques fonctions utilitaires dans le namespace LycansAPI.Core.Extensions.