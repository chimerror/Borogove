using Auth0.Core;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Borogove.Exceptions
{
    [Serializable]
    public class UnableToResolveUserException : Exception
    {
        public IEnumerable<User> ReturnedUsers { get; } = null;

        public UnableToResolveUserException() : base()
        {
        }

        public UnableToResolveUserException(string s) : base(s)
        {
        }
        public UnableToResolveUserException(string s, Exception e) : base(s, e)
        {
        }

        protected UnableToResolveUserException(SerializationInfo info, StreamingContext cxt) : base(info, cxt)
        {
        }

        public UnableToResolveUserException(IEnumerable<User> returnedUsers) : base()
        {
            ReturnedUsers = returnedUsers;
        }

        public UnableToResolveUserException(IEnumerable<User> returnedUsers, string s) : base(s)
        {
            ReturnedUsers = returnedUsers;
        }
        public UnableToResolveUserException(IEnumerable<User> returnedUsers, string s, Exception e) : base(s, e)
        {
            ReturnedUsers = returnedUsers;
        }

        protected UnableToResolveUserException(IEnumerable<User> returnedUsers, SerializationInfo info, StreamingContext cxt)
            : base(info, cxt)
        {
            ReturnedUsers = returnedUsers;
        }
    }
}
