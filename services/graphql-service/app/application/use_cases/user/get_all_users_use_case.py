from app.domain.entities.user import User
from app.application.interfaces.user_repository_interface import IUserRepository

class GetAllUsersUseCase:

    def __init__(
        self,
        repository: IUserRepository
    ):
        self.repository = repository

    async def execute(
        self
    ) -> list[User]:
        
        return await self.repository.get_all()