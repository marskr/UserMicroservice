﻿namespace UsersMicroservice.Queries
{
    public abstract class AbstractQueriesFactory<T, U>
                    where T : class
                    where U : class
    {
        public abstract T APIGetByEmail(string email_s, U context);
        public abstract T APIGetById(int id_i, U context);
        public abstract void APIPost(T newUser, U context);
        public abstract void APIPut(T updatedUser, T newUser, U context);
        public abstract void APIDelete(T deletedUser, U context);
        public abstract string APICreateToken(string email_s, string password_s, U context);
    }
}
