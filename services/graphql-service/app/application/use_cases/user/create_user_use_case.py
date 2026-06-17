from app.domain.entities.user import User
from app.application.interfaces.user_repository_interface import IUserRepository
from app.application.dto.user_response import UserResponse

class CreateUserUseCase:

    def __init__(
        self,
        repository: IUserRepository
    ):
        self.repository = repository

    async def execute(
        self,
        user: User
    ) -> UserResponse:
        
        return await self.repository.create(
            user
        )