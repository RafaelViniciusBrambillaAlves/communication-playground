import logging

from fastapi import Request, status
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse

logger = logging.getLogger(__name__)

async def validation_exception_handler(
    request: Request,
    exc: RequestValidationError
) -> JSONResponse:
    return JSONResponse(
        status_code = status.HTTP_422_UNPROCESSABLE_CONTENT,
        content = {
            "title": "Validation Error",
            "status": 422,
            "errors": exc.errors()
        }
    )


async def runtime_exception_handler(
    request: Request,
    exc: RuntimeError
) -> JSONResponse:
    logger.error("RuntimeError: %s", exc)

    return JSONResponse(
        status_code = status.HTTP_503_SERVICE_UNAVAILABLE,
        content = {
            "title": "Service Unavailable", 
            "status": 503,
            "detail": "Message broker is temporarily unavailable"
        }
    )


async def global_exception_handler(
    request: Request,
    exc: Exception
) -> JSONResponse:
    logger.exception("Unhandled exception")

    return JSONResponse(
        status_code = status.HTTP_500_INTERNAL_SERVER_ERROR,
        content = {
            "title": "Internal Server Error",
            "status": 500,
            "detail": "An unexpected error occurred"
        }
    )