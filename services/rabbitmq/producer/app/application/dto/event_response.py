from uuid import UUID
from pydantic import BaseModel


class PublishEventResponse(BaseModel):
    message: str
    user_id: UUID
