from pydantic import BaseModel, Field

class UpdateUserRequest(BaseModel):
    name: str = Field(
        min_length = 2,
        max_length = 100
    )