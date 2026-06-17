from app.application.interfaces.user_repository_interface import IUserRepository

from app.infrastructure.repositories.mongo_user_repository import MongoUserRepository


def get_user_repository() -> IUserRepository:

    return MongoUserRepository()