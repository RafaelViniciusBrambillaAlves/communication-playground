
from typing import Annotated
from fastapi import Depends

from app.application.use_cases.user.create_user_use_case import CreateUserUseCase
from app.application.use_cases.user.delete_user_use_case import DeleteUserUseCase
from app.application.use_cases.user.get_all_users_use_case import GetAllUsersUseCase
from app.application.use_cases.user.get_user_use_case import GetUserUseCase
from app.application.use_cases.user.update_user_use_case import UpdateUserUseCase

from app.dependencies.user_dependencies import (
    get_create_user_use_case,
    get_get_all_users_use_case,
    get_delete_user_use_case,
    get_get_user_use_case,
    get_update_user_use_case
)

CreateUserDep = Annotated[CreateUserUseCase, Depends(get_create_user_use_case)]

DeleteUserDep = Annotated[DeleteUserUseCase, Depends(get_delete_user_use_case)]

GetAllUsersDep = Annotated[GetAllUsersUseCase, Depends(get_get_all_users_use_case)]

GetUserDep = Annotated[GetUserUseCase, Depends(get_get_user_use_case)]

UpdateUserDep = Annotated[UpdateUserUseCase, Depends(get_update_user_use_case)]