from uuid import UUID

from fastapi import APIRouter, HTTPException, status

from app.application.dto.create_user_request import CreateUserRequest
from app.application.dto.update_user_dto import UpdateUserDto
from app.application.dto.user_response import UserResponse
from app.application.dto.update_user_request import UpdateUserRequest
from app.domain.entities.user import User

from app.dependencies.types import (
    CreateUserDep,
    DeleteUserDep, 
    GetAllUsersDep, 
    GetUserDep,
    UpdateUserDep
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
    use_case: CreateUserDep
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
    use_case: GetAllUsersDep
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
    use_case: GetUserDep
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
    use_case: DeleteUserDep
):
    deleted = await use_case.execute(user_id)
    
    if not deleted:
        raise HTTPException(
            status_code = status.HTTP_404_NOT_FOUND,
            detail = "User not found"
        )



@router.patch(
    "/{user_id}",
    response_model = UserResponse
)
async def update_user(
    user_id: UUID,
    request: UpdateUserRequest,
    use_case: UpdateUserDep
):
    
    dto = UpdateUserDto(
        id = user_id,
        name = request.name,
        email = request.email,
        age = request.age
    )

    user = await use_case.execute(dto)

    if user is None:
        raise HTTPException(
            status_code = status.HTTP_404_NOT_FOUND,
            detail = "User not found"
        )
    return UserResponse(**user.model_dump())

