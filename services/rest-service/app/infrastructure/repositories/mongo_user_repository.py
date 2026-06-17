from app.application.interfaces.user_repository_interface import IUserRepository
from app.domain.entities.user import User
from app.infrastructure.database.session import MongoDatabase
from datetime import datetime, timezone
from uuid import UUID


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
        
        document = await self.collection.find_one(
            {"id": str(user_id)}
        )

        if document is None:
            return None
        
        document.pop("_id", None)

        return User(**document)


    async def get_all(self) -> list[User]:
        
        users = []

        async for user in self.collection.find():
            
            user.pop("_id", None)

            users.append(
                User(**user)
            )
        
        return users


    async def delete(self, user_id: UUID) -> None:
        
        await self.collection.delete_one(
            {"id": str(user_id)}
        )

    async def update_name(self, user_id: UUID, name: str):
        
        await self.collection.update_one(
            {"id": str(user_id)},
            {
                "$set": {
                    "name": name,
                    "updated_at": datetime.now(timezone.utc)
                }
            }
        )

        user = await self.collection.find_one(
            {"id": str(user_id)}
        )

        if user is None:
            return None

        user.pop("_id", None)

        return User(**user)
        
        