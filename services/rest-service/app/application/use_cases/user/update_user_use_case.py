from uuid import UUID

from app.application.interfaces.user_repository_interface import IUserRepository
from app.domain.entities.user import User
from app.application.dto.update_user_dto import UpdateUserDto

class UpdateUserUseCase:

    def __init__(
        self, 
        repository: IUserRepository
    ):
        self.user_repository = repository

    async def execute(
        self, 
        dto: UpdateUserDto
    ) -> User | None:
        
        return await self.user_repository.update_name(
            dto
        )