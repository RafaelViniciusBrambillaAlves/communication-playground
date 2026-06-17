# Communication Playground


Um laboratório para estudar e comparar diferentes formas de comunicação entre serviços.

Objetivos do projeto

Aprender, de forma prática:

- Comunicação síncrona
- REST API
- GraphQL
- gRPC
- SOAP
- Comunicação assíncrona
- RabbitMQ
- Kafka
- Pub/Sub
- Webhooks
- Comunicação em tempo real
- WebSocket
- SSE (Server-Sent Events)


Outros conceitos
- Docker
- Arquitetura de microsserviços
- Event-driven architecture
- Message brokers
- Observabilidade
- Idempotência
- Retry
- Dead Letter Queue
- Correlation ID
- Health checks
- API Gateway

```txt
communication-playground/

services/
│
├── api-gateway
│
├── rest-service
├── graphql-service
├── grpc-service
├── soap-service
│
├── websocket-service
├── sse-service
│
├── webhook-producer
├── webhook-consumer
│
├── rabbitmq-producer
├── rabbitmq-consumer
│
├── kafka-producer
├── kafka-consumer
│
├── pubsub-service
│
├── notification-service
├── event-service
│
shared/
│
├── contracts/
├── protobufs/
├── schemas/
│
docker/
│
docker-compose.yml
```


## Stack
Python (FastAPI)

Usaremos para:

- REST
- GraphQL
- WebSocket
- SSE
- RabbitMQ
- Kafka


.NET (ASP.NET Core)

Usaremos para:

- gRPC
- SOAP
- Alguns consumers RabbitMQ/Kafka


## Estrutura do Projeto 

```txt
communication-playground/
│
├── services/
│
│   ├── rest-service/
│   │    ├── app/
│   │    ├── requirements.txt
│   │    ├── Dockerfile
│   │    └── .env
│   │
│   ├── graphql-service/
│   │    ├── app/
│   │    ├── requirements.txt
│   │    ├── Dockerfile
│   │    └── .env
│   │
│   ├── grpc-service/
│   │    ├── Program.cs
│   │    ├── grpc-service.csproj
│   │    ├── Dockerfile
│   │    └── .env
│   │
│   ├── websocket-service/
│   ├── sse-service/
│   ├── webhook-service/
│   ├── rabbitmq-producer/
│   ├── rabbitmq-consumer/
│   ├── kafka-producer/
│   └── kafka-consumer/
│
├── rabbitmq/
├── kafka/
├── shared/
├── docker-compose.yml
└── README.md
```


### Fase 1 — REST

Objetivo da rest-service é seguir práticas REST API

Clean Architecture
SOLID
Dependency Injection
DTOs
Repository Pattern
Services/Use Cases
Pydantic v2
Motor (async)
Configurações por ambiente
Versionamento da API
Docker
OpenAPI/Swagger

user-service (FastAPI)
```txt
POST /api/v1/users

GET /api/v1/users

GET /api/v1/users/{id}

DELETE /api/v1/users/{id}

PATCH /api/v1/users/{id}

GET /api/v1/health
```

Estrutura do Projeto 

```txt
rest-service/

app/
│
├── api/
│   └── v1/
│       ├── users.py
│       └── health.py
│
├── dependencies/
│   └── user_dependencies.py
│
├── application/
│   │
│   ├──dto/
│   │   ├── create_user_request.py
│   │   ├── update_user_request.py
│   │   └── user_response.py
│   │
│   ├── interfaces/
│   │   └── user_repository_interface.py
│   │
│   └── use_cases/
│       ├── create_user_use_case.py
│       ├── get_user_use_case.py
│       ├── get_all_users_use_case.py
│       ├── update_users_use_case.py
│       └── delete_user_use_case.py
│
├── domain/
│   └── entities/
│       ├── entity_base.py
│       └── user.py
│
├── infrastructure/
│   ├── database/
│   │   └── session.py
│   │
│   ├── repositories/
│   |   └── mongo_user_repository.py
│   │
│   └── config/
│       └── settings.py
│
├── dependencies/
│   └──user_dependencies.py
│
└── main.py

Dockerfile
requirements.txt
.env 
```