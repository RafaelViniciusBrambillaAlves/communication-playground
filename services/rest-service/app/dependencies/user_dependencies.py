from fastapi import Depends

from app.application.use_cases.user.create_user_use_case import CreateUserUseCase
from app.application.use_cases.user.delete_user_use_case import DeleteUserUseCase
from app.application.use_cases.user.get_all_users_use_case import GetAllUsersUseCase
from app.application.use_cases.user.get_user_use_case import GetUserUseCase
from app.application.use_cases.user.update_user_use_case import UpdateUserUseCase

from app.application.interfaces.user_repository_interface import IUserRepository
from app.infrastructure.repositories.mongo_user_repository import MongoUserRepository

def get_user_repository() -> IUserRepository:
    return MongoUserRepository()


def get_create_user_use_case(
    repository: MongoUserRepository = Depends(get_user_repository)
) -> CreateUserUseCase:
    return CreateUserUseCase(repository)


def get_get_all_users_use_case(
    repository: MongoUserRepository = Depends(get_user_repository)
) -> GetAllUsersUseCase:
    return GetAllUsersUseCase(repository)


def get_get_user_use_case(
    repository: MongoUserRepository = Depends(get_user_repository)
) -> GetUserUseCase:
    return GetUserUseCase(repository)


def get_delete_user_use_case(
    repository: MongoUserRepository = Depends(get_user_repository)
) -> DeleteUserUseCase:
    return DeleteUserUseCase(repository)

def get_update_user_use_case(
    repository: MongoUserRepository = Depends(get_user_repository) 
) -> UpdateUserUseCase:
    return UpdateUserUseCase(repository)