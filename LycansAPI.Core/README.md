# LycansAPI.Core

## � propos

Module principal de l'API, r�unis tout ce qui peut �tre commun aux autres modules de l'API.

## Utilisation

Le module met � disposition un logger global qui peut �tre utilis� dans l'ensemble de l'API comme suit :

```cs
Log.Debug("Hello")
Log.Info("Hello")
Log.Warning("Hello")
Log.Error("Hello")
```

L'API met �galement � disposition quelques fonctions utilitaires dans le namespace LycansAPI.Core.Extensions.