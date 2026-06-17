from app.application.interfaces.user_repository_interface import IUserRepository
from app.domain.entities.user import User

class GetAllUsersUseCase:

    def __init__(
        self,
        user_repository: IUserRepository
    ):
        self.user_repository = user_repository

    async def execute(
        self
    ) -> list[User]:
        
        return await self.user_repository.get_all()