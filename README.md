# Demo Microservice with Dotnet

## Structure

- docker-compose.yml: file cấu hình docker compose
- services:
  - AdminHotelApi: api quản trị hotel
  - CustomerHotelApi: api hotel của khách hàng
  - CommandHotelWorker: job thực hiện cập nhật hotel
  - Core: Code dùng chung
  - TestConsole: Console dùng để nghịch ngợm
  - DotnetService.sln: file solution của toàn bộ folder service này
- data:
  - opensearch: folder chứa data của opensearch
  - postgres: folder chứa data của postgreSql
  - AdminHotelApi: folder chứa file (bao gồm logs) của service AdminHotelApi
  - CustomerHotelApi: folder chứa file (bao gồm logs) của service CustomerHotelApi
  - CommandHotelWorker: folder chứa file (bao gồm logs) của job CommandHotelWorker

## Docker

```
docker compose up -d
```
