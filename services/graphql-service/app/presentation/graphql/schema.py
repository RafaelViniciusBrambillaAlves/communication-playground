import strawberry

from app.presentation.graphql.queries.user_query import UserQuery
from app.presentation.graphql.mutations.user_mutation import UserMutation

schema= strawberry.Schema(
    query = UserQuery,
    mutation = UserMutation
)