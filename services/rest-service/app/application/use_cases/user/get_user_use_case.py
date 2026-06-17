from uuid import UUID

from app.application.interfaces.user_repository_interface import IUserRepository
from app.domain.entities.user import User

class GetUserUseCase:

    def __init__(
        self,
        user_repository: IUserRepository
    ):
        self.user_repository = user_repository

    async def execute(
        self, 
        user_id: UUID
    ) -> User | None:
        
        return await self.user_repository.get_by_id(
            user_id
        )