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
│       └── user/
│           ├── create_user_use_case.py
│           ├── get_user_use_case.py
│           ├── get_all_users_use_case.py
│           ├── update_users_use_case.py
│           └── delete_user_use_case.py
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
│
└── main.py

Dockerfile
requirements.txt
.env 
```

Testar 

```txt
http://localhost:8001/docs
```




### Fase 2 — GraphQL

Objetivo da graph-service é seguir práticas GraphQL

Clean Architecture
SOLID principles
Dependency Injection
DTOs
Repository Pattern
Use Cases
Pydantic v2
Motor (async MongoDB)
Docker

Estrutura do projeto

```txt
graphql-service/
app/
│
├── presentation/
│   └── graphql/
│       ├── queries/
│       │   └── user_query.py
│       │
│       ├── mutations/
│       │   └── user_mutation.py
│       │
│       ├── types/
│       │   └── user_type.py
│       │
│       ├── inputs/
│       │   ├── create_user_input.py
│       │   └── update_user_input.py
│       │
│       └── schema.py
│   
├── dependencies/
│   └── user_dependencies.py
│
├── application/
│   ├── dto/ 
│   │   └── user_response.py
│   │
│   ├── interfaces/
│   │   └── user_repository_interface.py
│   │
│   └── use_cases/
│       └── user/
│           ├── create_user_use_case.py
│           ├── get_user_use_case.py
│           ├── get_all_users_use_case.py
│           ├── update_user_use_case.py
│           └── delete_user_use_case.py
│
├── domain/
│   └── entities/
│       ├── entity_base.py
│       └── user.py
│
│
├── infrastructure/
│   ├── database/
│   │   └── session.py
│   ├── repositories/
│   │   └── mongo_user_repository.py
│   └── config/
│       └── settings.py
│
└── main.py
```

Testar 

```txt
http://localhost:8002/graphql
```

Mutation Create User
```GraphQL
mutation {
  createUser(
    input: {
      name: "Teste"
      email: "teste@email.com"
      age: 25
    }
  ) {
    id
    name
    email
    age
    createdAt
  }
}
```

Query Get All Users
```GraphQL
query {
  getAllUsers {
    id
    name
    email
    age
  }
}
```

Query Get User
```GraphQL
mutation{
    getUser(
        id: "8d3f3c91-5db9-4b2c-b1d3-1df3e7e56d5d"
    ) {
        id
        name
        email
        age
    }
}
```

Mutation Update User
```GraphQl
mutation{
    updateUser(
        input: {
            id: "8d3f3c91-5db9-4b2c-b1d3-1df3e7e56d5d"
            name: Teste 2
            age: 22
        }
    ) {
        id
        name
        email
        age
        updatedAt
    }
}
```

Mutation Delete User
```GraphQL
mutation{
    deleteUser(
        userId: "8d3f3c91-5db9-4b2c-b1d3-1df3e7e56d5d"
    )
}
```

