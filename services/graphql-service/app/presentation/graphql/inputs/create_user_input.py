import strawberry

@strawberry.input
class CreateUserInput:
    name: str
    email: str
    age: int
