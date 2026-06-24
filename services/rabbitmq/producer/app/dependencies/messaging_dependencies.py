from app.application.use_cases.publish_user_created_use_case import PublishUserCreatedUseCase
from app.application.use_cases.publish_user_updated_use_case import PublishUserUpdatedUseCase
from app.application.use_cases.publish_user_deleted_use_case import PublishUserDeletedUseCase
from app.infrastructure.messaging.rabbitmq_producer import RabbitMQProducer


def get_producer() -> RabbitMQProducer:
    return RabbitMQProducer()


def get_publish_user_created_use_case() -> PublishUserCreatedUseCase:
    return PublishUserCreatedUseCase(get_producer())


def get_publish_user_updated_use_case() -> PublishUserUpdatedUseCase:
    return PublishUserUpdatedUseCase(get_producer())


def get_publish_user_deleted_use_case() -> PublishUserDeletedUseCase:
    return PublishUserDeletedUseCase(get_producer())

