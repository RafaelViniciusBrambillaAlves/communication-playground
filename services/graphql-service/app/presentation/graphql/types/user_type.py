from datetime import datetime
from uuid import UUID

import strawberry

@strawberry.type
class UserType:
    id: UUID
    name: str
    email: str
    age: int
    created_at: datetime
    updated_at: datetime | None