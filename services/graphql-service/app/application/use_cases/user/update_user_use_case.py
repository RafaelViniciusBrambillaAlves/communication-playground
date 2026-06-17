from uuid import UUID

from app.domain.entities.user import User
from app.application.interfaces.user_repository_interface import IUserRepository
from app.application.dto.update_user_dto import UpdateUserDTO

class UpdateUserUseCase:

    def __init__(
        self,
        repository: IUserRepository
    ):
        self.repository = repository

    async def execute(
        self,
        dto: UpdateUserDTO
    ) -> User | None:
        
        return await self.repository.update(dto)