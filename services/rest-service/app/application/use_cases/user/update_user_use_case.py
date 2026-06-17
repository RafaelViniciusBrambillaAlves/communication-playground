from uuid import UUID

from app.application.interfaces.user_repository_interface import IUserRepository
from app.domain.entities.user import User

class UpdateUserUseCase:

    def __init__(
        self, 
        repository: IUserRepository
    ):
        self.user_repository = repository

    async def execute(
        self, 
        user_id: UUID,
        name: str
    ) -> User | None:
        
        return await self.user_repository.update_name(
            user_id,
            name
        )