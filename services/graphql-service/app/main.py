from contextlib import asynccontextmanager

from fastapi import FastAPI
from strawberry.fastapi import GraphQLRouter

from app.infrastructure.database.session import MongoDatabase
from app.presentation.graphql.schema import schema


@asynccontextmanager
async def lifespan(app: FastAPI):
    MongoDatabase.connect()
    yield
    MongoDatabase.disconnect()


app = FastAPI(
    title = "GraphQL Service",
    version = "1.0.0",
    lifespan = lifespan
)

graphql_app = GraphQLRouter(
    schema
)

app.include_router(
    graphql_app,
    prefix = "/graphql"
)
