from uuid import UUID

from fastapi import APIRouter, HTTPException, status, Depends

from app.application.dto.create_user_request import CreateUserRequest
from app.application.dto.user_response import UserResponse
from app.application.dto.update_user_request import UpdateUserRequest
from app.application.use_cases.user.create_user_use_case import CreateUserUseCase
from app.application.use_cases.user.delete_user_use_case import DeleteUserUseCase
from app.application.use_cases.user.get_all_users_use_case import GetAllUsersUseCase
from app.application.use_cases.user.get_user_use_case import GetUserUseCase
from app.application.use_cases.user.update_user_use_case import UpdateUserUseCase
from app.domain.entities.user import User
from app.infrastructure.repositories.mongo_user_repository import (
    MongoUserRepository
)
from app.dependencies.user_dependencies import (
    get_create_user_use_case,
    get_get_all_users_use_case,
    get_delete_user_use_case,
    get_get_user_use_case,
    get_update_user_use_case
)


router = APIRouter(
    prefix = "/users",
    tags = ["Users"]
)

@router.post(
    "",
    response_model = UserResponse,
    status_code = status.HTTP_201_CREATED 
)
async def create_user(
    request: CreateUserRequest,
    use_case: CreateUserUseCase = Depends(get_create_user_use_case)
):

    user = User(
        name = request.name,
        email = request.email,
        age = request.age
    )

    created_user = await use_case.execute(user)

    return UserResponse(**created_user.model_dump())


@router.get(
    "",
    response_model = list[UserResponse]
)
async def get_all_users(
    use_case: GetAllUsersUseCase = Depends(get_get_all_users_use_case)
):
    users = await use_case.execute()

    return [
        UserResponse(**user.model_dump())
        for user in users
    ]    


@router.get(
    "/{user_id}",
    response_model = UserResponse
)
async def get_user(
    user_id: UUID,
    use_case: GetUserUseCase = Depends(get_get_user_use_case)
):

    user = await use_case.execute(user_id)

    if user is None:
        raise HTTPException(
            status_code = status.HTTP_404_NOT_FOUND,
            detail = "User not found"
        ) 
    
    return UserResponse(
        **user.model_dump()
    )


@router.delete(
    "/{user_id}",
    status_code = status.HTTP_204_NO_CONTENT
)
async def delete_user(
    user_id: UUID,
    use_case: DeleteUserUseCase = Depends(get_delete_user_use_case)
):
    await use_case.execute(user_id)


@router.patch(
    "/{user_id}",
    response_model = UserResponse
)
async def update_user(
    user_id: UUID,
    request: UpdateUserRequest,
    use_case: UpdateUserUseCase = Depends(get_update_user_use_case)
):
    user = await use_case.execute(user_id = user_id, name = request.name)

    if user is None:
        raise HTTPException(
            status_code = status.HTTP_404_NOT_FOUND,
            detail = "User not found"
        )
    
    return UserResponse(
        **user.model_dump()
    )