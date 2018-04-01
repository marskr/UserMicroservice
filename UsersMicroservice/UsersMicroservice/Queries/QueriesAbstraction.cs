namespace UsersMicroservice.Queries
{
    public abstract class AbstractQueriesFactory<T, U>
                    where T : class
                    where U : class
    {
        public abstract T APIGet(string email_s, U context);
        public abstract void APIPost(T newUser, U context);
        public abstract void APIPut(T updatedUser, T newUser, U context);
        public abstract void APIDelete(T deletedUser, U context);
    }
}
