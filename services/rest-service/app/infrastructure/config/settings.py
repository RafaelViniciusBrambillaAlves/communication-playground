from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):

    # Environment
    ENVIRONMENT: str

    # Mongo Database
    MONGODB_URL: str
    MONGODB_DATABASE: str


    model_config = SettingsConfigDict(
        env_file = ".env",
        extra = "ignore"
    )

settings = Settings()