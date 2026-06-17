from datetime import datetime
from uuid import UUID

from pydantic import BaseModel, EmailStr


class UserResponse(BaseModel):
    id: UUID
    name: str
    email: EmailStr
    age: int
    created_at: datetime
    updated_at: datetime | None = None