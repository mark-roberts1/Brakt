using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Brakt.Rest.Database.Sqlite
{
    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SqliteCommand : ICommand
    {
        private bool disposed;

        /// <summary>
        /// Constructs an instance of <see cref="SqliteCommand"/>
        /// </summary>
        public SqliteCommand()
        {
            _command = new SQLiteCommand();
            Parameters = new List<IParameter>();
        }

        private readonly SQLiteCommand _command;
        
        /// <inheritdoc/>
        public string CommandText
        {
            get => _command.CommandText;
            set => _command.CommandText = value;
        }

        /// <inheritdoc/>
        public CommandType CommandType
        {
            get => _command.CommandType;
            set => _command.CommandType = value;
        }

        /// <inheritdoc/>
        public IConnection Connection
        {
            get => new SqliteConnection(_command.Connection);
            set
            {
                value.ThrowIf(_ => $"{typeof(SqliteConnection)} must be used with {typeof(SqliteCommand)}", value => !(value is SqliteConnection));
                
                _command.Connection = (value as SqliteConnection).Connection;
            }
        }

        /// <inheritdoc/>
        public IList<IParameter> Parameters { get; }

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
                _command.Dispose();
                disposed = true;
            }
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            LoadParameters();
            return await _command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
        {
            LoadParameters();
            return await _command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        {
            LoadParameters();
            return await _command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteNonQueryAsync()
        {
            LoadParameters();
            return await _command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IDataReader> ExecuteReaderAsync()
        {
            LoadParameters();
            return await _command.ExecuteReaderAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<object> ExecuteScalarAsync()
        {
            LoadParameters();
            return await _command.ExecuteScalarAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public int ExecuteNonQuery()
        {
            LoadParameters();
            return _command.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public IDataReader ExecuteReader()
        {
            LoadParameters();
            return _command.ExecuteReader();
        }

        /// <inheritdoc/>
        public object ExecuteScalar()
        {
            LoadParameters();
            return _command.ExecuteScalar();
        }
        
        private void LoadParameters()
        {
            _command.Parameters.Clear();

            if (Parameters.Any())
            {
                foreach (var param in Parameters)
                {
                    _command.Parameters.Add(new SQLiteParameter(param.Name, param.Value));
                }
            }
        }
    }
}
