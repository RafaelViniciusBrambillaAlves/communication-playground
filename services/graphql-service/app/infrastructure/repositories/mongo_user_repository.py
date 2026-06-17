from uuid import UUID
from datetime import datetime, timezone

from app.application.interfaces.user_repository_interface import IUserRepository
from app.domain.entities.user import User
from app.application.dto.update_user_dto import UpdateUserDTO
from app.infrastructure.database.session import MongoDatabase

class MongoUserRepository(IUserRepository):
    
    def __init__(self):
        database = MongoDatabase()
        self.collection = database.get_database()["users"]


    async def create(self, user: User) -> User:

        await self.collection.insert_one(
            user.model_dump(mode = "json")
        )
        return user


    async def get_by_id(self, user_id: UUID) -> User | None:
        
        user = await self.collection.find_one(
            {"id": str(user_id)}
        )

        if user is None:
            return None
        
        user.pop("_id", None)

        return User(**user)
    

    async def get_all(self) -> list[User]:

        users = []
        
        async for user in self.collection.find():

            user.pop("_id", None)

            users.append(
                User(**user)
            )
        
        return users
    

    async def delete(self, user_id: UUID) -> None:

        self.collection.delete_one(
            {"id": str(user_id)}
        )

    
    async def update(self, dto: UpdateUserDTO) -> User:
        
        update_data = {}

        if dto.name is not None:
            update_data["name"] = dto.name
        
        if dto.email is not None:
            update_data["email"] = dto.email

        if dto.age is not None:
            update_data["age"] = dto.age

        update_data["updated_at"] = datetime.now(
            timezone.utc
        )

        await self.collection.update_one(
            {
                "id": str(dto.id)
            },
            {
                "$set": update_data
            }
        )

        user = await self.collection.find_one(
            {
                "id": str(dto.id)
            }
        )

        if user is None:
            return None
        
        user.pop("_id", None)

        return User(
            **user
        )
    