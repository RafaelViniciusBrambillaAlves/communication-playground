import json

from aio_pika import Message, DeliveryMode

from app.application.interfaces.message_broker_interface import IMessageBroker
from app.domain.events.user_event import UserEvent
from app.infrastructure.messaging.rabbitmq_connection import RabbitMQConnection


class RabbitMQProducer(IMessageBroker):

    async def publish(self, event: UserEvent) -> None:
        exchange = RabbitMQConnection.get_exchange()

        message = Message(
            body = json.dumps(
                event.model_dump(mode = "json"),
                default = str
            ).encode(),
            content_type = "application/json",
            delivery_mode = DeliveryMode.PERSISTENT,
            message_id = str(event.event_id)
        )

        await exchange.publish(
            message, 
            routing_key = event.event_type.value
        )