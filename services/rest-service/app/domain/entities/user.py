from app.domain.entities.entity_base import EntityBase
from uuid import UUID
from datetime import datetime, timezone
from pydantic import Field, EmailStr, field_validator

class User(EntityBase):
    name: str
    email: EmailStr
    age: int 
    created_at: datetime = Field(default_factory = lambda: datetime.now(timezone.utc))
    updated_at: datetime | None = None
    

    @field_validator("email")
    @classmethod
    def normalize_email(cls, value: str) -> str:
        return value.lower()

    @field_validator("age")
    @classmethod
    def validate_age(cls, value: int) -> int:
        
        if value < 18:
            raise ValueError("User must be at least 18 years old")
        
        return value


