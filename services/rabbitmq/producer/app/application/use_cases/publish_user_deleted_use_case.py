from uuid import UUID, uuid4
from datetime import datetime, timezone

from app.application.interfaces.message_broker_interface import IMessageBroker
from app.application.dto.user_event_request import PublishUserDeletedRequest
from app.domain.events.user_event import UserEvent, UserEventType


class PublishUserDeletedUseCase:

    def __init__(self, broker: IMessageBroker):
        self.broker = broker

    async def execute(self, request: PublishUserDeletedRequest) -> None:
        event = UserEvent(
            event_id = uuid4(),
            event_type = UserEventType.DELETED,
            timestamp = datetime.now(timezone.utc),
            payload = {
                "user_id": str(request.user_id)
            }
        )

        await self.broker.publish(event)