# TrainingRoomsApi

Projekt wykonany w ramach ćwiczenia z ASP.NET Core Web API.

## Opis projektu

Aplikacja przedstawia prosty backend dla centrum szkoleniowego. System pozwala zarządzać salami dydaktycznymi oraz ich rezerwacjami.

Projekt został przygotowany jako ASP.NET Core Web API oparte na kontrolerach. Dane są przechowywane w pamięci aplikacji w statycznych listach. Aplikacja nie korzysta z Entity Framework Core ani z bazy danych SQL.

## Technologie

- C#
- ASP.NET Core Web API
- Kontrolery API
- Routing atrybutowy
- Data Annotations
- Swagger
- Postman

## Struktura projektu

- `Controllers/RoomsController.cs` — obsługa endpointów dotyczących sal.
- `Controllers/ReservationsController.cs` — obsługa endpointów dotyczących rezerwacji.
- `Models/Room.cs` — model sali.
- `Models/Reservation.cs` — model rezerwacji.
- `Data/AppData.cs` — statyczne dane przechowywane w pamięci aplikacji.

## Uruchomienie projektu

```bash
dotnet run