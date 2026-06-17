from motor.motor_asyncio import AsyncIOMotorClient

from app.infrastructure.config.settings import settings

class MongoDatabase:

    def __init__(self):
        self.client = AsyncIOMotorClient(
            settings.MONGODB_URL
        )

        self.db = self.client[settings.MONGODB_DATABASE]

    def get_database(self):
        return self.db

    async def close(self):
        self.client.close()