# Используем базовый образ ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Этот этап используется для сборки проекта
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем проект EComputer/Api.csproj в текущую директорию
COPY ["EComputer/Api.csproj", "EComputer/"]

# Восстанавливаем зависимости
RUN dotnet restore "EComputer/Api.csproj"

# Копируем остальные файлы проекта
COPY . . 

# Собираем проект
WORKDIR "/src/EComputer"
RUN dotnet build "Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикуем проект
FROM build AS publish
RUN dotnet publish "Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ для запуска
FROM base AS final
WORKDIR /app

# Копируем опубликованный проект
COPY --from=publish /app/publish .

# Устанавливаем точку входа для приложения
ENTRYPOINT ["dotnet", "Api.dll"]
