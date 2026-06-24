from fastapi import APIRouter, Depends, status

from app.application.dto.event_response import PublishEventResponse
from app.application.dto.user_event_request import (
    PublishUserCreatedRequest,
    PublishUserUpdatedRequest,
    PublishUserDeletedRequest
)

from app.application.use_cases.publish_user_created_use_case import PublishUserCreatedUseCase
from app.application.use_cases.publish_user_updated_use_case import PublishUserUpdatedUseCase
from app.application.use_cases.publish_user_deleted_use_case import PublishUserDeletedUseCase
from app.dependencies.messaging_dependencies import (
    get_publish_user_created_use_case,
    get_publish_user_updated_use_case, 
    get_publish_user_deleted_use_case
)

router = APIRouter(prefix = "/events", tags = ["Events"])

@router.post(
    "/users/created",
    response_model = PublishEventResponse,
    status_code = status.HTTP_202_ACCEPTED
)
async def publish_user_created(
    request: PublishUserCreatedRequest,
    use_case: PublishUserCreatedUseCase = Depends(get_publish_user_created_use_case)
):
    user_id = await use_case.execute(request)

    return PublishEventResponse(
        message = "UserCreated event published",
        user_id = user_id 
    )


@router.post(
    "/users/updated",
    response_model = PublishEventResponse,
    status_code = status.HTTP_202_ACCEPTED
)
async def publish_user_updated(
    request: PublishUserUpdatedRequest,
    use_case: PublishUserUpdatedUseCase = Depends(get_publish_user_updated_use_case)
):
    await use_case.execute(request)

    return {
        "message": "UserUpdated event published"
    }    


@router.post(
    "/users/deleted",
    response_model = PublishEventResponse,
    status_code = status.HTTP_202_ACCEPTED
)
async def publish_user_deleted(
    request: PublishUserDeletedRequest,
    use_case: PublishUserDeletedUseCase = Depends(get_publish_user_deleted_use_case)
):
    await use_case.execute(request)

    return {
        "message": "UserDeleted event published"
    }