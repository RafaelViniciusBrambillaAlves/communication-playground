from contextlib import asynccontextmanager
from fastapi import FastAPI

from app.api.v1 import events, health
from app.infrastructure.messaging.rabbitmq_connection import RabbitMQConnection


@asynccontextmanager
async def lifespan(app: FastAPI):
    await RabbitMQConnection.connect()
    yield
    await RabbitMQConnection.disconnect()


app = FastAPI(
    title = "RabbitMQ Producer Service",
    version = "1.0.0",
    lifespan = lifespan
)

prefix = "/api/v1"
app.include_router(events.router, prefix = prefix)
app.include_router(health.router, prefix = prefix)