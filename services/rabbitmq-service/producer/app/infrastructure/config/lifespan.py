from contextlib import asynccontextmanager

from fastapi import FastAPI

from app.infrastructure.messaging.rabbitmq_connection import RabbitMQConnection


@asynccontextmanager
async def lifespan(app: FastAPI):
    await RabbitMQConnection.connect()

    yield

    await RabbitMQConnection.disconnect()