# Swagger gebruiken in Play Catalog

## Wat is Swagger?

Swagger UI is een webpagina waarmee je een API kunt bekijken en testen.
Je ziet er alle endpoints, parameters en response types. Je kunt ook op
**Try it out** klikken om vanuit de browser een request te versturen.

Swagger gebruikt een OpenAPI-document. Dat document beschrijft automatisch
welke routes de API heeft en welke gegevens de routes verwachten.

## Wat is er aangepast?

### 1. Swagger package toegevoegd

In `Play.Catalog.Service.csproj` staat nu:

```xml
<PackageReference Include="Swashbuckle.AspNetCore" Version="10.3.1" />
```

Deze package levert:

- Swagger JSON-generatie
- Swagger UI
- de middleware voor `/swagger`

### 2. Swagger services geregistreerd

In `Program.cs` staat:

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Play Catalog API",
        Version = "v1"
    });
});
```

`AddSwaggerGen` maakt het OpenAPI-document. De titel en versie verschijnen
in Swagger UI.

### 3. Swagger middleware geactiveerd

In `Program.cs` staat in de Development-omgeving:

```csharp
app.UseSwagger();
app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Play Catalog API v1"));
```

`UseSwagger` maakt het JSON-document beschikbaar. `UseSwaggerUI` maakt de
interactieve webpagina beschikbaar.

De configuratie staat alleen binnen:

```csharp
if (app.Environment.IsDevelopment())
```

Daardoor is Swagger standaard alleen beschikbaar tijdens lokale development.

### 4. Controllers geregistreerd

Omdat de API controllers gebruikt, staat ook dit in `Program.cs`:

```csharp
builder.Services.AddControllers();
app.MapControllers();
```

Zonder deze twee regels worden de routes in `ItemsController` niet actief.

## De API starten

Open PowerShell in de projectmap:

```powershell
cd C:\Kdg\Play.Catalog\src\Play.Catalog.Service
dotnet run
```

De applicatie draait normaal op:

```text
http://localhost:5137
```

## Swagger openen

Open deze URL in je browser:

```text
http://localhost:5137/swagger
```

De Swagger UI toont bijvoorbeeld:

- `GET /api/items`
- `GET /api/items/{id}`
- `POST /api/items`
- `PUT /api/items/{id}`
- `DELETE /api/items/{id}`

Het onderliggende OpenAPI-document staat op:

```text
http://localhost:5137/swagger/v1/swagger.json
```

## Een endpoint testen

1. Open `http://localhost:5137/swagger`.
2. Klap bijvoorbeeld `GET /api/items` open.
3. Klik op **Try it out**.
4. Klik op **Execute**.
5. Bekijk de request URL, statuscode en response body.

Je kunt dezelfde API ook testen met het `.http`-bestand in de projectmap.

## Build controleren

Stop eerst een nog draaiende applicatie als de build meldt dat
`Play.Catalog.Service.exe` in gebruik is. Voer daarna uit:

```powershell
dotnet build
```

Een succesvolle build eindigt met:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## Belangrijk verschil tussen Swagger en OpenAPI

- **OpenAPI** is de standaardbeschrijving van de API.
- **Swagger UI** is de webinterface die dat OpenAPI-document toont.
- `/swagger/v1/swagger.json` is het document.
- `/swagger` is de interactieve pagina.
