from fastapi import APIRouter, status
from datetime import datetime, timezone

router = APIRouter(prefix = "/health", tags = ["health"])

@router.get(
    "/",
    status_code = status.HTTP_200_OK
)
async def health():
    return {
        "status": "healthy",
        "timestamp": datetime.now(timezone.utc)
    }