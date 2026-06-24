from datetime import datetime, timezone
from fastapi import APIRouter, status

router = APIRouter(prefix = "/health", tags = ["Health"])


@router.get("", status_code = status.HTTP_200_OK)
async def health():
    return {
        "status": "health",
        "timestamp": datetime.now(timezone.utc)
    }