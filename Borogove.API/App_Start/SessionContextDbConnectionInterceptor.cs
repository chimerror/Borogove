using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Threading;
using System.Security.Claims;

namespace Borogove.API
{
    internal class SessionContextDbConnectionInterceptor : IDbConnectionInterceptor
    {
        public void Opened(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            var principal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (principal == null)
            {
                // No principal found, so just return.
                return;
            }

            var claims = principal.Claims.ToList();
            var userId = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                var userIdCommand = connection.CreateCommand();
                userIdCommand.CommandText = "EXEC sp_set_session_context @key=N'UserId', @value=@UserId";
                var userIdParameter = userIdCommand.CreateParameter();
                userIdParameter.ParameterName = "@UserId";
                userIdParameter.Value = userId;
                userIdCommand.Parameters.Add(userIdParameter);
                userIdCommand.ExecuteNonQuery();
            }

            var groups = claims.Where(c => c.Type.Equals(ClaimTypes.GroupSid)).Select(c => c.Value).ToList();
            if (groups.Count > 0)
            {
                var groupsString = "|" + string.Join("|", groups) + "|";
                var groupsCommand = connection.CreateCommand();
                groupsCommand.CommandText = "EXEC sp_set_session_context @key=N'Groups', @value=@GroupsString";
                var groupsParameter = groupsCommand.CreateParameter();
                groupsParameter.ParameterName = "@GroupsString";
                groupsParameter.Value = groupsString;
                groupsCommand.Parameters.Add(groupsParameter);
                groupsCommand.ExecuteNonQuery();
            }
        }

        public void BeganTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext)
        {
        }

        public void BeginningTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext)
        {
        }

        public void Closed(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }

        public void Closing(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }

        public void ConnectionStringGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void ConnectionStringGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void ConnectionStringSet(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext)
        {
        }

        public void ConnectionStringSetting(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext)
        {
        }

        public void ConnectionTimeoutGetting(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext)
        {
        }

        public void ConnectionTimeoutGot(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext)
        {
        }

        public void DatabaseGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void DatabaseGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void DataSourceGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void DataSourceGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void Disposed(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }

        public void Disposing(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }

        public void EnlistedTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext)
        {
        }

        public void EnlistingTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext)
        {
        }

        public void Opening(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }

        public void ServerVersionGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void ServerVersionGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void StateGetting(DbConnection connection, DbConnectionInterceptionContext<ConnectionState> interceptionContext)
        {
        }

        public void StateGot(DbConnection connection, DbConnectionInterceptionContext<ConnectionState> interceptionContext)
        {
        }
    }
}