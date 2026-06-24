from uuid import UUID, uuid4
from datetime import datetime, timezone

from app.application.interfaces.message_broker_interface import IMessageBroker
from app.application.dto.user_event_request import PublishUserUpdatedRequest
from app.domain.events.user_event import UserEvent, UserEventType


class PublishUserUpdatedUseCase:

    def __init__(self, broker: IMessageBroker):
        self.broker = broker

    
    async def execute(self, request: PublishUserUpdatedRequest) -> None:
        payload: dict = {"user_id": str(request.user_id)}

        if request.name is not None:
            payload["name"] = request.name
        
        if request.email is not None:
            payload["email"] = request.email

        if request.age is not None:
            payload["age"] = request.age

        event = UserEvent(
            event_id = uuid4(),
            event_type = UserEventType.UPDATED,
            timestamp = datetime.now(timezone.utc),
            payload = payload
        )

        await self.broker.publish(event)