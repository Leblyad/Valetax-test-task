# Valetax — partner commission microservices

.NET 10 APIs: **Users**, **Events**, **Wallets**. PostgreSQL per service. Cross-service writes go through transactional outbox + HTTP.

## Ports

| Service | URL | Swagger |
|---------|-----|---------|
| Users | http://localhost:5111 | http://localhost:5111/swagger |
| Events | http://localhost:5164 | http://localhost:5164/swagger |
| Wallets | http://localhost:5174 | http://localhost:5174/swagger |
| Postgres | localhost:5432 | `postgres` / `postgres` |

## Run

```bash
docker compose up --build
```

Dev with `dotnet watch`:

```bash
docker compose -f docker-compose.dev.yml up --build
```

Local build:

```bash
dotnet build Valetax-test-task.slnx
```

## Seed data

Tables are created by EF migrations on API startup. Then:

```bash
docker compose exec -T postgres psql -U postgres < docker/postgres/seed.sql
```

Demo hierarchy: **Alice ← Bob ← Carol** (fixed GUIDs in `docker/postgres/seed.sql` and Postman variables).

## Postman

Import `postman/Valetax.postman_collection.json`. Follow folders 1→4; after creating an event wait a few seconds for outbox.

## Health

- `GET /health` on each API
