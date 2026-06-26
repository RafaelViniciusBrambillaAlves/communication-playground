from uuid import UUID
from pydantic import BaseModel, EmailStr, Field, model_validator


class PublishUserCreatedRequest(BaseModel):
    name: str = Field(min_length = 2, max_length = 100)
    email: EmailStr
    age: int = Field(ge = 18)


class PublishUserUpdatedRequest(BaseModel):
    user_id: UUID
    name: str | None = Field(default = None, min_length = 2, max_length = 100)
    email: EmailStr | None = None
    age: int | None = Field(default = None, ge = 18)

    @model_validator(mode = "after")
    def check_at_least_one_field(self) -> "PublishUserUpdatedRequest":
        if self.name is None and self.email is None and self.age is None:
            raise ValueError("At least one field must be provided for update.")
        return self


class PublishUserDeletedRequest(BaseModel):
    user_id: UUID
