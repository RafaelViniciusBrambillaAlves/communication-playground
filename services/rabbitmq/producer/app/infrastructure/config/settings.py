from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):

    ENVIRONMENT: str
    RABBITMQ_URL: str
    RABBITMQ_EXCHANGE: str
    
    model_config = SettingsConfigDict(
        env_file = ".env",
        extra = "ignore"
    )

settings = Settings()