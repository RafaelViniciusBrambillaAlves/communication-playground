from uuid import UUID

import strawberry

@strawberry.input
class UpdateUserInput:
    id: UUID
    name: str | None = None
    email: str | None = None 
    age: int | None = None
    