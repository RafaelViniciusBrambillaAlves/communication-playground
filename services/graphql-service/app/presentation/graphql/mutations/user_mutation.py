from uuid import UUID

import strawberry

from app.dependencies.use_case_dependencies.user_dependencies import (
    get_create_user_use_case,
    get_delete_user_use_case,
    get_update_user_use_case
)

from app.domain.entities.user import User

from app.presentation.graphql.inputs.create_user_input import CreateUserInput
from app.presentation.graphql.inputs.update_user_input import UpdateUserInput
from app.presentation.graphql.types.user_type import UserType
from app.application.dto.update_user_dto import UpdateUserDTO


@strawberry.type
class UserMutation:

    @strawberry.mutation
    async def create_user(
        self, 
        input: CreateUserInput
    ) -> UserType:
    
        use_case = get_create_user_use_case()

        user = User(
            name = input.name,
            email = input.email,
            age = input.age
        )

        created_user = await use_case.execute(user)

        return UserType(
            id = created_user.id,
            name = created_user.name,
            email = created_user.email,
            age = created_user.age,
            created_at = created_user.created_at,
            updated_at = created_user.updated_at
        ) 

    
    @strawberry.mutation
    async def update_user(
        self, 
        input: UpdateUserInput
    ) -> UserType:
        
        use_case = get_update_user_use_case()

        dto = UpdateUserDTO(
            id = input.id,
            name = input.name,
            email = input.email,
            age = input.age
        )

        user = await use_case.execute(dto)
        
        if user is None:
            raise ValueError("User not found")

        return UserType(
            id = user.id,
            name = user.name,
            email = user.email,
            age = user.age,
            created_at = user.created_at,
            updated_at = user.updated_at,
        )
    
    @strawberry.mutation
    async def delete_user(
        self,
        user_id: UUID
    ) -> bool:
        
        use_case = get_delete_user_use_case()

        deleted = await use_case.execute(user_id)

        if not deleted:
            raise ValueError("User not found")

        return True



