## Running Example

1. Start postgres

```
docker-compose up -d
```

2. Start producer

```
dotnet run --project producer/producer.csproj
```

3. Start consumer

```
dotnet run --project consumer/consumer.csproj
```

