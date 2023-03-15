FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Point_of_Sale.API/Point_of_Sale", "."]
COPY ["Point_of_Sale.API/Framework.Validator","../Framework.Validator"]
RUN dotnet restore "Point_of_Sale.csproj"
RUN dotnet build "Point_of_Sale.csproj" -c Release -o /app/build
RUN dotnet publish "Point_of_Sale.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update &&\
    apt-get install tzdata
ENV TZ=Asia/Bangkok
ENTRYPOINT ["dotnet", "Point_of_Sale.dll"]