from fastapi import FastAPI
from fastapi.exceptions import RequestValidationError

from app.infrastructure.config.lifespan import lifespan
from app.infrastructure.config.exception_handlers import (
    global_exception_handler,
    runtime_exception_handler, 
    validation_exception_handler
)
from app.api.routers import register_routes

app = FastAPI(
    title = "RabbitMQ Producer Service",
    version = "1.0.0",
    lifespan = lifespan
)

app.add_exception_handler(
    RequestValidationError,
    validation_exception_handler
)

app.add_exception_handler(
    RuntimeError,
    runtime_exception_handler
)

app.add_exception_handler(
    Exception,
    global_exception_handler
)

register_routes(app)