namespace Brakt.Rest.Database.Sqlite
{
    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        /// <summary>
        /// Returns a default instance of <see cref="SqliteConnectionFactory"/>
        /// </summary>
        public static SqliteConnectionFactory Default => new SqliteConnectionFactory();

        /// <inheritdoc/>
        public IConnection BuildConnection(string connectionString)
        {
            return new SqliteConnection(connectionString);
        }
    }
}
