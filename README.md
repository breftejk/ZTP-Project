# Konfiguracja

## API

### 1. Generowanie bazy danych

Aplikacja korzysta z bazy danych SQLite.

Jeśli nie masz jeszcze narzędzia `dotnet-ef`, zainstaluj je za pomocą poniższego polecenia:

```bash
dotnet tool install --global dotnet-ef
```

Jeśli nie masz jeszcze bazy danych, musisz ją utworzyć. W tym celu wykonaj poniższe polecenia w katalogu `API`:

```bash
cd API
dotnet ef migrations add InitialCreate
dotnet ef database update
```