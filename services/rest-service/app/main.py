from contextlib import asynccontextmanager

from fastapi import FastAPI
from app.api.v1 import health, users
from app.infrastructure.database.session import MongoDatabase


@asynccontextmanager
async def lifespan(app: FastAPI):
    MongoDatabase.connect()
    yield
    MongoDatabase.disconnect()


app = FastAPI(
    title = "Rest Service",
    version = "1.0.0",
    lifespan = lifespan
)


app.include_router(
    health.router,
    prefix = "/api/v1"
)

app.include_router(
    users.router,
    prefix = "/api/v1"
)
