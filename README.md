# About
Projeto destinado a realizar busca de dados de forma flexível no Redis Search.

# Índices
## Criação de índice no Redis para permitir a realização das buscas
FT.CREATE fund-index ON HASH PREFIX 1 fund: SCHEMA
	id TAG SORTABLE
	name TEXT SORTABLE
	class TEXT NOINDEX
	start TEXT NOINDEX
	manager TEXT NOINDEX
	fundType TEXT NOINDEX
	updatedDate TEXT NOINDEX
	administrator TEXT NOINDEX

# Referência
Redis: https://oss.redis.com/redisearch/Commands/
Nuget Redis.OM: https://github.com/redis/redis-om-dotnet