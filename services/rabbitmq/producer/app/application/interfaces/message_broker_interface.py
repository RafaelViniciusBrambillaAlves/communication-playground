from abc import ABC, abstractmethod
from app.domain.events.user_event import UserEvent


class IMessageBroker(ABC):

    @abstractmethod
    async def publish(self, envet: UserEvent) -> None:
        pass
