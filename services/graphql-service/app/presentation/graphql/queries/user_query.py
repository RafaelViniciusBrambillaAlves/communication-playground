from uuid import UUID

import strawberry

from app.dependencies.use_case_dependencies.user_dependencies import (
    get_get_user_use_case, 
    get_get_all_users_use_case
)

from app.presentation.graphql.types.user_type import UserType


@strawberry.type
class UserQuery:

    @strawberry.field
    async def get_user(self, id: UUID) -> UserType:

        use_case = get_get_user_use_case()

        user = await use_case.execute(id)

        if user is None:
            raise ValueError("User not found")

        return UserType(
            id = user.id,
            name = user.name,
            email = user.email,
            age = user.age,
            created_at = user.created_at,
            updated_at = user.updated_at
        )
    

    @strawberry.field
    async def get_all_users(self) -> list[UserType]:
        
        use_case = get_get_all_users_use_case()

        users = await use_case.execute()

        return [
            UserType(
                id = user.id,
                name = user.name,
                email = user.email,
                age = user.age,
                created_at = user.created_at,
                updated_at = user.updated_at
            )
            for user in users
        ]


