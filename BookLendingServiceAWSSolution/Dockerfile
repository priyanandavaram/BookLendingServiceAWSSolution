FROM public.ecr.aws/lambda/dotnet:8 as base
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src
COPY ["BookLendingServiceAWSSolution.csproj", "BookLendingServiceAWSSolution/"]
RUN dotnet restore "BookLendingServiceAWSSolution/BookLendingServiceAWSSolution.csproj"

WORKDIR "/src/BookLendingServiceAWSSolution"
COPY . .
RUN dotnet build "BookLendingServiceAWSSolution.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "BookLendingServiceAWSSolution.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
CMD ["BookLendingServiceAWSSolution::BookLendingServiceAWSSolution.LambdaEntryPoint::FunctionHandlerAsync"]
COPY --from=publish /app/publish .
