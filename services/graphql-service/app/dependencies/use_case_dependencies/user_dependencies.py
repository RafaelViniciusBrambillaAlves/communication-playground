from app.application.use_cases.user.create_user_use_case import CreateUserUseCase
from app.application.use_cases.user.get_user_use_case import GetUserUseCase
from app.application.use_cases.user.get_all_users_use_case import GetAllUsersUseCase
from app.application.use_cases.user.delete_user_use_case import DeleteUserUseCase
from app.application.use_cases.user.update_user_use_case import UpdateUserUseCase

from app.application.interfaces.user_repository_interface import IUserRepository
from app.infrastructure.repositories.mongo_user_repository import MongoUserRepository

from app.dependencies.repository_dependencies import get_user_repository


def get_create_user_use_case() -> CreateUserUseCase:
    repository = get_user_repository()

    return CreateUserUseCase(repository)


def get_get_user_use_case() -> GetUserUseCase:
    repository = get_user_repository()

    return GetUserUseCase(repository)


def get_get_all_users_use_case() -> GetAllUsersUseCase:
    repository = get_user_repository()

    return GetAllUsersUseCase(repository)


def get_delete_user_use_case() -> DeleteUserUseCase:
    repository = get_user_repository()

    return DeleteUserUseCase(repository)


def get_update_user_use_case() -> UpdateUserUseCase:
    repository = get_user_repository()

    return UpdateUserUseCase(repository)
