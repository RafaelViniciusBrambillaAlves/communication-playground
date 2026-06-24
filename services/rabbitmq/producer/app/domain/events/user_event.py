from enum import Enum
from uuid import UUID
from datetime import datetime
from pydantic import BaseModel


class UserEventType(str, Enum):
    CREATED = "user.created"
    UPDATED = "user.updated"
    DELETED = "user.deleted"


class UserEvent(BaseModel):
    event_id: UUID
    event_type: UserEventType
    timestamp: datetime
    payload: dict