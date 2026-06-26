from fastapi import FastAPI

from app.api.v1 import events, health

def register_routes(app: FastAPI) -> None:
    app.include_router(health.router, prefix = "/api/v1")
    app.include_router(events.router, prefix = "/api/v1")