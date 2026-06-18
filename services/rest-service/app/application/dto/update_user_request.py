from pydantic import BaseModel, Field, EmailStr, model_validator 

class UpdateUserRequest(BaseModel):
    name: str | None = Field(default = None, min_length = 2,max_length = 100)
    email: EmailStr | None = Field(default = None)
    age : int | None = Field(default = None, ge = 18)

    @model_validator(mode = "after")
    def check_at_least_one_field(self) -> "UpdateUserRequest":
        if self.name is None and self.email is None and self.age is None:
            raise ValueError("At least one field must be filled in for updating.")
        return self
