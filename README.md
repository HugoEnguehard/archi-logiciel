## Installation du projet 
Une fois le projet récupéré, il va falloir créer la base de données pour pouvoir faire lancer les routes jusqu’au bout. Elle devra s’appeler ‘master’ ou bien modifier le nom dans la première ligne du script SQL.
Pour ce faire, il suffira juste de lancer la requête SQL dans n’importe quel logiciel de gestion de base de données (de préférence SSMS).

Une fois la BDD créée, on peut venir récupérer la chaîne de connexion de cette dernière (sur Visual Studio, il est possible de se connecter à la BDD pour avoir les informations utiles dont la chaîne de connexion). 
Il ne reste plus qu’à coller la chaîne dans le fichier appsettings.json, dans l’emplacement du même nom (DBConnection).
