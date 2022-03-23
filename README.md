# About
Projeto destinado a realizar busca de dados de forma flex�vel no Redis Search.

# �ndices
## Cria��o de �ndice no Redis para permitir a realiza��o das buscas
FT.CREATE fund-index ON HASH PREFIX 1 fund: SCHEMA
	id TAG SORTABLE
	name TEXT SORTABLE
	class TEXT NOINDEX
	start TEXT NOINDEX
	manager TEXT NOINDEX
	fundType TEXT NOINDEX
	updatedDate TEXT NOINDEX
	administrator TEXT NOINDEX

# Refer�ncia
Redis: https://oss.redis.com/redisearch/Commands/
Nuget Redis.OM: https://github.com/redis/redis-om-dotnet