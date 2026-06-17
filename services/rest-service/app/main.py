from fastapi import FastAPI
from fastapi import APIRouter
from app.api.v1 import health, users

app = FastAPI(
    title = "Rest Service",
    version = "1.0.0"
)

app.include_router(
    health.router,
    prefix = "/api/v1"
)


app.include_router(
    users.router,
    prefix = "/api/v1"
)
