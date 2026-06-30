# Expense Tracker

Приложение для учета личных расходов с backend API на ASP.NET Core и frontend на React.

## Краткое описание решений

В проекте использована многоуровневая архитектура с разделением ответственности по слоям:

- **Controller** - принимает HTTP-запросы и возвращает ответы клиенту.
- **Service** - содержит бизнес-логику приложения.
- **Repository** - отвечает за работу с базой данных.

Статистика считается на стороне базы данных через LINQ и `IQueryable`. Для агрегации расходов используются SQL-запросы, которые формируются через методы вроде `GroupBy` и `Sum`.

Frontend реализован на React с использованием готовых компонентов **shadcn/ui** и **Tailwind CSS**. Контент приложения разделен на 3 вкладки, чтобы интерфейс был аккуратнее и удобнее для пользователя.

Кнопки удалить и редактировать сделал внутри контекстного меню, всплавающего через ПКМ по элементу.
<img width="1091" height="359" alt="image" src="https://github.com/user-attachments/assets/7ecdd07b-8409-4796-98c3-52697438c534" />

## Стек

- **Backend:** ASP.NET Core, Entity Framework Core, PostgreSQL
- **Frontend:** React, Vite, shadcn/ui, Tailwind CSS

## Требования

- .NET 10 SDK
- Node.js 18+
- pnpm
- PostgreSQL
- dotnet-ef

Если `dotnet-ef` не установлен, его можно установить командой:

```bash
dotnet tool install --global dotnet-ef
```

## Настройка базы данных

В файле `backend/backend/appsettings.json` укажите строку подключения к PostgreSQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=expense_tracker;Username=postgres;Password=твой_пароль"
  }
}
```
## Запуск из корня проекта

### Backend

Применить миграции:

```bash
dotnet restore backend/backend.sln
dotnet ef database update --project backend/backend/backend.csproj --startup-project backend/backend/backend.csproj
```

Запустить:

```bash
dotnet run --project backend/backend/backend.csproj
```

Backend будет доступен по адресу: `http://localhost:5200`

### Frontend

Установить зависимости:

```bash
pnpm -C frontend install
```

Запустить:

```bash
pnpm -C frontend dev
```

Frontend будет доступен по адресу: `http://localhost:5173`
