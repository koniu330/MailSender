# MailSender

MailSender to aplikacja backendowa napisana w **ASP.NET Core (.NET 9)** umożliwiająca rejestrację aplikacji klienckich oraz wysyłanie wiadomości e-mail z wykorzystaniem tokenów JWT oraz usługi **Brevo**.

Projekt został wykonany w ramach przedmiotu **Programowanie Aplikacji Backendowych** na WSEI.

---

# Główne funkcjonalności

- ✅ Rejestracja aplikacji klienckiej
- ✅ Autoryzacja z wykorzystaniem JWT
- ✅ Dokumentacja API w Swagger (OpenAPI)
- ✅ Wysyłanie wiadomości e-mail przez Brevo
- ✅ Logika biznesowa zgodna z wymaganiami projektu
- ✅ WebClient umożliwiający testowanie aplikacji z poziomu przeglądarki
- ✅ Przechowywanie poufnych danych z wykorzystaniem User Secrets

---

# Technologie

Projekt został wykonany z wykorzystaniem następujących technologii:

- ASP.NET Core (.NET 9)
- C#
- JWT Authentication
- Swagger / OpenAPI
- Brevo Email API
- Dependency Injection
- HttpClient
- User Secrets
- HTML
- JavaScript

---

# Architektura projektu

Projekt składa się z trzech głównych projektów backendowych oraz klienta demonstracyjnego WebClient.

```
MailSender
│
├── MailSender.Api
│   ├── Controllers
│   ├── Program.cs
│   ├── JWT Authentication
│   ├── Swagger
│   └── Dependency Injection
│
├── MailSender.Core
│   ├── Models
│   ├── Interfaces
│   └── Business Logic
│
├── MailSender.Infrastructure
│   ├── BrevoMailSenderProvider
│   └── Integracja z Brevo API
│
└── WebClient
    ├── HTML
    ├── JavaScript
    └── TypeScript Client
```

Taki podział pozwala oddzielić logikę biznesową od warstwy API oraz komunikacji z zewnętrznym dostawcą wiadomości.

---

# Funkcjonalność API

## Rejestracja aplikacji

Endpoint

```
POST /client-app/register
```

umożliwia rejestrację aplikacji klienckiej.

Przykładowe żądanie

```json
{
  "appId": "demo-app",
  "appName": "Demo App",
  "pass": "q##waQ53"
}
```

Po poprawnej weryfikacji hasła zwracany jest token JWT ważny przez **90 dni**.

Przykładowa odpowiedź

```json
{
  "appId": "demo-app",
  "appName": "Demo App",
  "key": "eyJhbGciOi..."
}
```

---

## Wysyłanie wiadomości

Endpoint

```
POST /mail/send
```

jest zabezpieczony tokenem JWT.

Do wykonania żądania wymagane jest wcześniejsze zarejestrowanie aplikacji oraz uzyskanie tokena.

Przykładowe żądanie

```json
{
  "to": "example@gmail.com",
  "subject": "Czy działa?",
  "body": "Test Koń"
}
```

---

# Logika biznesowa

Przed wysłaniem wiadomości wykonywane są operacje wymagane w specyfikacji projektu.

### Dodawanie prefiksu `[Q]`

Jeżeli temat wiadomości kończy się znakiem zapytania (`?`), automatycznie dodawany jest prefiks:

```
[Q]
```

Przykład

```
Czy działa?
```

↓

```
[Q] Czy działa?
```

---

### Oznaczanie nazwiska

Jeżeli treść wiadomości zawiera nazwisko jednego z autorów projektu, zostaje ono automatycznie oznaczone:

```
[student.surname]Nazwisko[/student.surname]
```

Przykład

```
Test Koń
```

↓

```
Test [student.surname]Koń[/student.surname]
```

---

# Integracja z Brevo

Projekt wykorzystuje REST API usługi **Brevo** do rzeczywistego wysyłania wiadomości e-mail.

Komunikacja realizowana jest przez klasę

```
BrevoMailSenderProvider
```

wykorzystującą `HttpClient`.

Poufne dane, takie jak:

- API Key
- adres nadawcy
- nazwa nadawcy

przechowywane są poza repozytorium z wykorzystaniem **User Secrets**.

---

# Swagger

Projekt posiada dokumentację API wygenerowaną przez Swagger.

Swagger umożliwia:

- rejestrację aplikacji,
- wygenerowanie tokena JWT,
- autoryzację poprzez przycisk **Authorize**,
- testowanie wszystkich endpointów bez korzystania z dodatkowych narzędzi.

---

# WebClient

Projekt zawiera prostego klienta demonstracyjnego znajdującego się w katalogu

```
WebClient/
```

Aplikacja umożliwia:

- wpisanie tokena JWT,
- podanie odbiorcy wiadomości,
- wpisanie tematu,
- wpisanie treści,
- wysłanie wiadomości do backendu,
- wyświetlenie odpowiedzi API.

Dzięki temu możliwe jest przetestowanie aplikacji również bez używania Swagger UI.

---

# Instalacja

Sklonowanie repozytorium

```bash
git clone -b koniu https://github.com/gmarczak/MailSender.git

cd MailSender
```

Przywrócenie zależności

```bash
dotnet restore
```

---

# Konfiguracja User Secrets

Przejdź do katalogu projektu API

```bash
cd MailSender.Api
```

Zainicjuj User Secrets

```bash
dotnet user-secrets init
```

Dodaj wymagane ustawienia

```bash
dotnet user-secrets set "JwtSettings:SecretKey" "YOUR_SECRET_KEY"

dotnet user-secrets set "ExpectedClientPassword" "q##waQ53"

dotnet user-secrets set "Brevo:ApiKey" "YOUR_BREVO_API_KEY"

dotnet user-secrets set "Brevo:SenderEmail" "YOUR_EMAIL"

dotnet user-secrets set "Brevo:SenderName" "MailSender"
```

Dzięki temu poufne dane nie są przechowywane w repozytorium Git.

---

# Uruchomienie projektu

Uruchom aplikację

```bash
dotnet run --project MailSender.Api
```

Swagger dostępny jest pod adresem

```
http://localhost:5134/swagger
```

---

# Test działania

Projekt został przetestowany w dwóch scenariuszach.

### Swagger

- rejestracja aplikacji,
- wygenerowanie tokena JWT,
- autoryzacja,
- wysłanie wiadomości e-mail,
- odebranie wiadomości.

### WebClient

- wpisanie tokena JWT,
- wpisanie danych wiadomości,
- wysłanie wiadomości,
- odebranie odpowiedzi API.

Po poprawnym wykonaniu operacji wiadomość zostaje wysłana przez usługę Brevo na wskazany adres e-mail.

---

# Zrealizowane wymagania

| Wymaganie | Status |
|-----------|:------:|
| Backend ASP.NET Core | ✅ |
| JWT Authentication | ✅ |
| Swagger / OpenAPI | ✅ |
| Rejestracja aplikacji | ✅ |
| Token JWT (90 dni) | ✅ |
| Endpoint chroniony JWT | ✅ |
| Prefiks `[Q]` | ✅ |
| Oznaczanie nazwiska | ✅ |
| Integracja z Brevo | ✅ |
| User Secrets | ✅ |
| WebClient HTML / JavaScript | ✅ |
| Test działania | ✅ |

---

# Autorzy

Projekt został wykonany przez:

- Grzegorz Marczak
- Szymon Koń
- Konrad Francuz
- Jakub Cybak

w ramach przedmiotu **Programowanie Aplikacji Backendowych** na WSEI.