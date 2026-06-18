from uuid import UUID

from app.application.interfaces.user_repository_interface import IUserRepository

class DeleteUserUseCase:

    def __init__(
        self,
        repository: IUserRepository
    ):
        self.repository = repository

    async def execute(
        self,
        user_id: UUID
    ) -> bool:
        
        return await self.repository.delete(
            user_id
        )


