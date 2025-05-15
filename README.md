<h1 align="center" id="title">BérAutó</h1>

<h2>Start with docker:</h2>

<p>1. Install docker engine</p>
<p>2. Build the dockerfile: "docker build -t berauto-app:latest -f BerAuto/Dockerfile ."</p>
<p>3. Start the container: "docker run -d -p 8080:8080 --name berauto-container -e "ASPNETCORE_URLS=http://+:8080" -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal,1433;Database=CarRentalDB;User Id=admin;Password=test123;TrustServerCertificate=True" berauto-app:latest"</p>

<h2>Start with docker compose:</h2>
<p>1. Install docker engine</p>

<p>2. Edit docker-compose.yml in the BerAuto directory(change connection string)</p>

<p>3. docker-compose up</p>

