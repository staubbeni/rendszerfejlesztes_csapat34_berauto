# 🚗 Bérautó

## 🛠️ Telepítés

### Frontend indítása

A frontend futtatásához kövesd az alábbi lépéseket:

1. Navigálj a frontend mappába:

   ```bash
   cd /Berauto/berauto-frontend
   ```

2. Telepítsd a függőségeket és indítsd el az alkalmazást:

   ```bash
   npm install && npm start
   ```

### Docker Compose használata (deprecated)

A projekt Docker Compose segítségével is futtatható. Kövesd az alábbi lépéseket:

1. Telepítsd a **Docker Engine**-t a rendszeredre: Docker telepítési útmutató.

2. Szerkeszd a `docker-compose.yml` fájlt a `Berauto` mappában:

   - Módosítsd a kapcsolati stringet (connection string) a környezetednek megfelelően.

3. Indítsd el a konténereket:

   ```bash
   docker-compose up
   ```
