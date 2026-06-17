from uuid import UUID

from pydantic import BaseModel, EmailStr


class UpdateUserDTO(BaseModel):
    id: UUID
    name: str | None = None
    email: EmailStr | None = None
    age: int | None = None