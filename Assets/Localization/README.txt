Comment utiliser le package Localization ?

Pré-requis :
- un fichier CSV
- une scèhne contenant des textes
- mettre le prefab "LanguageManager" du projet dans la scène

1. Importez votre fichier CSV contenant les traductions dans différentes pour vos textes.
2. Glissez-déposez le fichier CSV dans le champ "CSV File" du LanguageManager.
3. Choissisez la language dans laquelle vous souhaitez que votre texte soit traduit de base.
4. Récupérez les différents textes de la scène avec le bouton "Fetch texts from scene".
5. Lancez le jeu.

Rajouter des boutons pour changer de langue en jeu :
- Utilisez le prefab "LanguageSwitcher" pour vos boutons.
- Attribuez-leur une language dans leur champ "Language" sur leur composant "Language switcher button".