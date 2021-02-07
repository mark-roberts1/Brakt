using System.Data;

namespace Brakt.Rest.Database.Sqlite
{
    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SqliteCommandFactory : IDbCommandFactory
    {
        /// <summary>
        /// Returns a default instance of <see cref="SqliteCommandFactory"/>
        /// </summary>
        public static SqliteCommandFactory Default => new SqliteCommandFactory();

        /// <inheritdoc/>
        public ICommand BuildCommand(string commandText, CommandType commandType, IConnection connection, params IParameter[] parameters)
        {
            commandText.ThrowIfNull(nameof(commandText));
            connection.ThrowIfNull(nameof(connection));

            
            var command = new SqliteCommand
            {
                CommandText = commandText,
                CommandType = commandType,
                Connection = connection
            };

            if (parameters != null && parameters.Length > 0)
            {
                foreach (var param in parameters)
                    command.Parameters.Add(param);
            }

            return command;
        }
    }
}
