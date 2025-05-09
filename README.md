# TimeLog API (.NET 8 + MongoDB + OpenAI)

## 🚀 Description

API RESTful pour la gestion de time logs, utilisateurs, tâches et intégration OpenAI (chatbot), développée en ASP.NET Core 8, avec MongoDB Atlas pour la persistance des données.

---

## 🛠️ Stack technique

- **Backend** : ASP.NET Core 8
- **Base de données** : MongoDB Atlas
- **Hébergement** : Render (ou Azure possible)
- **Secrets** : Variables d'environnement (jamais dans le code)
- **Intégration IA** : OpenAI (GPT-3.5-turbo)

---

## 📦 Installation & Lancement local

1. **Cloner le repo**
2. Créer un fichier `.env` à la racine avec :
   ```env
   OPENAI_API_KEY=sk-xxxxxxx
   MONGODB_URI=ta_chaine_mongodb
   ```
3. Vérifier que `appsettings.json` ne contient aucun secret (laisser les champs vides ou factices)
4. Lancer :
   ```bash
   dotnet run
   ```
5. Accéder à Swagger : [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## 🌐 Déploiement Render

- Déployer le projet sur Render comme web service.
- Définir les variables d'environnement dans le dashboard Render :
  - `OPENAI_API_KEY` : ta clé OpenAI
  - `MongoDbSettings__ConnectionString` : ta chaîne MongoDB Atlas
- L'API écoute automatiquement sur le port fourni par Render.
- Un job GitHub Actions ping l'API toutes les 10 minutes pour la garder active.

---

## 🔑 Gestion des secrets

- **Jamais de clé dans le code ou les fichiers versionnés**
- Utiliser les variables d'environnement (Render, Azure, ou `.env` en local)
- Pour la prod, privilégier Azure Key Vault ou Render Secrets

---

## 📚 Endpoints principaux

### OpenAI

- `POST /api/OpenAI/chat` : Chat avec OpenAI (body : `{ "Prompt": "..." }`)

### Authentification

- `POST /api/Auth/login` : Connexion
- `POST /api/Auth/register` : Inscription
- `POST /api/Auth/refresh-token` : Rafraîchir le token
- `POST /api/Auth/logout` : Déconnexion
- `GET /api/Auth/me` : Infos utilisateur connecté

### Utilisateurs

- `GET /api/Users` : Liste des utilisateurs
- `GET /api/Users/{id}` : Détail utilisateur
- `POST /api/Users` : Créer utilisateur
- `PUT /api/Users/{id}` : Modifier utilisateur
- `DELETE /api/Users/{id}` : Supprimer utilisateur

### Tâches utilisateur

- `GET /api/UserTask` : Liste des tâches
- `GET /api/UserTask/{id}` : Détail tâche
- `POST /api/UserTask` : Créer tâche
- `PUT /api/UserTask/{id}` : Modifier tâche
- `DELETE /api/UserTask/{id}` : Supprimer tâche

### TimeLogs

- `GET /api/TimeLogs` : Liste des time logs
- `GET /api/TimeLogs/{id}` : Détail time log
- `POST /api/TimeLogs` : Créer time log
- `PUT /api/TimeLogs/{id}` : Modifier time log
- `DELETE /api/TimeLogs/{id}` : Supprimer time log

### Test Key Vault

- `GET /api/TestKeyVault/secret` : Test lecture d'un secret Azure Key Vault

---

## 🖥️ Frontend

- Le front doit être développé dans un projet séparé (React, Vue, Angular, etc.)
- Il consommera l'API via les endpoints ci-dessus
- Déploiement possible sur Vercel, Netlify, Render, etc.

---

## 📝 Notes

- L'API est prévue pour être sécurisée (JWT, gestion des secrets)
- Pour toute contribution, merci de ne jamais commiter de secrets ou de clés API
- Pour toute question ou amélioration, ouvrir une issue ou une pull request
