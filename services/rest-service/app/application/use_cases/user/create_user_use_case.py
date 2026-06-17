from app.application.interfaces.user_repository_interface import IUserRepository
from app.domain.entities.user import User

class CreateUserUseCase:

    def __init__(
        self,
        user_repository: IUserRepository
    ):
        self.user_repository = user_repository

    async def execute(
        self, 
        user: User
    ) -> User:
        
        return await self.user_repository.create(user)