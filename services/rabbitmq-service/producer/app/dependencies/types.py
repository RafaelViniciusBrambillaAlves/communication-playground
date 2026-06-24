from typing import Annotated
from fastapi import Depends

from app.application.use_cases.publish_user_created_use_case import PublishUserCreatedUseCase
from app.application.use_cases.publish_user_updated_use_case import PublishUserUpdatedUseCase
from app.application.use_cases.publish_user_deleted_use_case import PublishUserDeletedUseCase
from app.dependencies.messaging_dependencies import (
    get_publish_user_created_use_case,
    get_publish_user_updated_use_case, 
    get_publish_user_deleted_use_case
)


CreateUserDep = Annotated[PublishUserCreatedUseCase, Depends(get_publish_user_created_use_case)]

UpdateUserDep = Annotated[PublishUserUpdatedUseCase, Depends(get_publish_user_updated_use_case)]

DeletedUserDep = Annotated[PublishUserDeletedUseCase, Depends(get_publish_user_deleted_use_case)]
