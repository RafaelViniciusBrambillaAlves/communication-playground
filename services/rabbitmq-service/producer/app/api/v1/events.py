from fastapi import APIRouter, Depends, status

from app.application.dto.event_response import PublishEventResponse
from app.application.dto.user_event_request import (
    PublishUserCreatedRequest,
    PublishUserUpdatedRequest,
    PublishUserDeletedRequest
)

from app.dependencies.types import (
    CreateUserDep,
    UpdateUserDep,
    DeletedUserDep
)

router = APIRouter(prefix = "/events", tags = ["Events"])

@router.post(
    "/users/created",
    response_model = PublishEventResponse,
    status_code = status.HTTP_202_ACCEPTED
)
async def publish_user_created(
    request: PublishUserCreatedRequest,
    use_case: CreateUserDep
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
    use_case: UpdateUserDep
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
    use_case: DeletedUserDep
):
    await use_case.execute(request)

    return {
        "message": "UserDeleted event published"
    }