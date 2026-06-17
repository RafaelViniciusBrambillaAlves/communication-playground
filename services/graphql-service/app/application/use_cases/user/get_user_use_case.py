from uuid import UUID

from app.domain.entities.user import User
from app.application.interfaces.user_repository_interface import IUserRepository

class GetUserUseCase:

    def __init__(
        self,
        repository: IUserRepository
    ):
        self.repository = repository

    async def execute(
        self,
        user_id: UUID
    ) -> User | None:
        
        return await self.repository.get_by_id(
            user_id
        )