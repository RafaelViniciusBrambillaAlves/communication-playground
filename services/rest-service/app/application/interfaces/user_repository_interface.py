from abc import ABC, abstractmethod
from uuid import UUID

from app.domain.entities.user import User
from app.application.dto.update_user_dto import UpdateUserDto

class IUserRepository(ABC):

    @abstractmethod
    async def create(self, user: User) -> User:
        pass

    @abstractmethod
    async def get_by_id(self, user_id: UUID) -> User | None:
        pass

    @abstractmethod
    async def get_all(self) -> list[User]:
        pass

    @abstractmethod
    async def delete(self, user_id: UUID) -> bool:
        pass

    @abstractmethod
    async def update_name(self, dto: UpdateUserDto) -> User | None:
        pass