using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Brakt.Rest.Database.Sqlite
{
    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SqliteConnection : IConnection
    {
        private bool disposed;

        /// <summary>
        /// The underlying <see cref="SQLiteConnection"/>
        /// </summary>
        public SQLiteConnection Connection { get; }

        /// <summary>
        /// Constructs an instance of <see cref="SqliteConnection"/> using the provided connection string.
        /// </summary>
        /// <param name="connectionString">Information describing how to connect to the database.</param>
        public SqliteConnection(string connectionString)
        {
            Connection = new SQLiteConnection(connectionString);
        }

        /// <summary>
        /// Constructs an instance of <see cref="SqliteConnection"/> using an existing <see cref="SQLiteConnection"/>
        /// </summary>
        /// <param name="connection">An existing connection to a SQLite database.</param>
        public SqliteConnection(SQLiteConnection connection)
        {
            Connection = connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                Connection.Dispose();
                disposed = true;
            }
        }

        /// <inheritdoc/>
        public async Task OpenAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new ConnectionException($"An error occurred while opening a connection to {Connection.Database} on {Connection.DataSource}", ex);
            }
        }

        /// <inheritdoc/>
        public async Task OpenAsync()
        {
            try
            {
                await Connection.OpenAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new ConnectionException($"An error occurred while opening a connection to {Connection.Database} on {Connection.DataSource}", ex);
            }
        }

        /// <inheritdoc/>
        public void Open()
        {
            try
            {
                Connection.Open();
            }
            catch (Exception ex)
            {
                throw new ConnectionException($"An error occurred while opening a connection to {Connection.Database} on {Connection.DataSource}", ex);
            }
        }

        /// <inheritdoc/>
        public void Close()
        {
            Connection.Close();
        }

        /// <inheritdoc/>
        public void ChangeDatabase(string database)
        {
            Connection.ChangeDatabase(database);
        }
    }
}
