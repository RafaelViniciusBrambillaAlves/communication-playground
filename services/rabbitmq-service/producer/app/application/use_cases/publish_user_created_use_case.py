from uuid import UUID, uuid4
from datetime import datetime, timezone

from app.application.interfaces.message_broker_interface import IMessageBroker
from app.application.dto.user_event_request import PublishUserCreatedRequest
from app.domain.events.user_event import UserEvent, UserEventType


class PublishUserCreatedUseCase:

    def __init__(self, broker: IMessageBroker):
        self.broker = broker

    async def execute(self, request: PublishUserCreatedRequest) -> UUID:
        user_id = uuid4()

        event = UserEvent(
            event_id = uuid4(),
            event_type = UserEventType.CREATED,
            timestamp = datetime.now(timezone.utc),
            payload = {
                "user_id": str(user_id),
                "name": request.name,
                "email": request.email,
                "age": request.age   
            }
        )

        await self.broker.publish(event)
        return user_id
