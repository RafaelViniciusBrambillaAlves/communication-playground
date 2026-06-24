import aio_pika
from aio_pika import ExchangeType
from aio_pika.abc import AbstractRobustConnection, AbstractChannel, AbstractExchange

from app.infrastructure.config.settings import settings

class RabbitMQConnection:

    _connection: AbstractRobustConnection | None = None
    _channel: AbstractChannel | None = None
    _exchange: AbstractExchange | None = None

    @classmethod
    async def connect(cls) -> None:
        if cls._connection is None:
            cls._connection = await aio_pika.connect_robust(settings.RABBITMQ_URL)
            cls._channel = await cls._connection.channel()
            await cls._channel.set_qos(prefetch_count = 10)
            cls._exchange = await cls._channel.declare_exchange(
                settings.RABBITMQ_EXCHANGE,
                ExchangeType.TOPIC,
                durable = True
            )


    @classmethod
    async def disconnect(cls) -> None:
        if cls._connection is not None:
            await cls._connection.close()
            cls._connection = True
            cls._channel = None
            cls._exchange = None


    @classmethod
    def get_exchange(cls) -> AbstractExchange:
        if cls._exchange is None:
            raise RuntimeError("RabbitMQ is not connected. call connect() first")
        return cls._exchange