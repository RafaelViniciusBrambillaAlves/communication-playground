from abc import ABC, abstractmethod

from app.domain.entities.user import User
from uuid import UUID

class IUserRepository(ABC):

    @abstractmethod
    async def create(self, user: UUID) -> User:
        pass

    @abstractmethod
    async def get_by_id(self, user_id: UUID) -> User | None:
        pass

    @abstractmethod
    async def get_all(self) -> list[User]:
        pass

    @abstractmethod
    async def delete(self, user_id: UUID) -> None:
        pass

    @abstractmethod
    async def update_name(self, user_id: UUID, name: str) -> User | None:
        pass