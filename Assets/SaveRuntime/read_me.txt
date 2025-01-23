ATTENTION Vector 3 et dictionaire non fonctionnel
SO = Scriptable Object

Fonctionnement : 
Le tool permet de modifier sur TOUS LES SCRIPTS possedant le Custom Attribute SAVABLE de sauvegarder leurs instances celon les variables voulus.

Pour lancer le tool 3 méthodes : 
- Dans les onglets séléctionner le tool. Puis votre SO de sauvegarde et ce dernier se lancera.
- Double cliqué sur le SO que vous désirez
- Sur le SO appuyer sur OPEN TOOL

En récupérant SAVEFILE vous pouvez appeler 2 méthodes : 
Save() et Load()
Save s'appelle soit avec un ID, celui du SO de sauvegarde de votre choix soit avec un SaveDataBehaviour (le SO directement)
Il en est de meme pour le LOAD

Pour le runtime il est préférable d'utiliser un Serialize Field dans lequel vous mettez votre SO afin d'utiliser uniquement les parametres choisis.
