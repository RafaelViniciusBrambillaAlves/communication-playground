from motor.motor_asyncio import AsyncIOMotorClient, AsyncIOMotorDatabase

from app.infrastructure.config.settings import settings

class MongoDatabase:
    _client: AsyncIOMotorClient | None = None

    @classmethod
    def connect(cls) -> None:
        if cls._client is None:
            cls._client = AsyncIOMotorClient(settings.MONGODB_URL)


    @classmethod
    def disconnect(cls) -> None:
        if cls._client is not None:
            cls._client.close()
            cls._client = None


    @classmethod
    def get_database(cls) -> None:
        if cls._client is None:
            raise RuntimeError("MongoDB is not connected. Call connect() first.")
        return cls._client[settings.MONGODB_DATABASE]
        