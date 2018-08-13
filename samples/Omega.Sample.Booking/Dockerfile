FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Omega.Sample.Booking/Omega.Sample.Booking.csproj Omega.Sample.Booking/
RUN dotnet restore Omega.Sample.Booking/Omega.Sample.Booking.csproj
COPY . .
WORKDIR /src/Omega.Sample.Booking
RUN dotnet build Omega.Sample.Booking.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Omega.Sample.Booking.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Omega.Sample.Booking.dll"]
