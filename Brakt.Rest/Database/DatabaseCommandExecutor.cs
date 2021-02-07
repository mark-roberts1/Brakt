using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Database
{
    /// <summary>
    /// Used to queries against a database
    /// </summary>
    public class DatabaseCommandExecutor : ICommandExecutor
    {
        private readonly IDataMapper _dataMapper;

        /// <summary>
        /// Constructs an instance of <see cref="DatabaseCommandExecutor"/>
        /// </summary>
        /// <param name="dataMapper">Maps data to models from a DataReader</param>
        public DatabaseCommandExecutor(IDataMapper dataMapper)
        {
            _dataMapper = dataMapper.ThrowIfNull(nameof(dataMapper));
        }

        /// <inheritdoc/>
        public IEnumerable<T> Execute<T>(ICommand command) where T : class, new()
        {
            command.ThrowIfNull(nameof(command));

            IEnumerable<T> results;

            command.Connection.Open();

            using (var reader = command.ExecuteReader())
            {
                results = _dataMapper.Map<T>(reader);
            }

            return results;
        }

        public IEnumerable<T> Execute<T>(ICommand command, Func<IDataReader, T> rowMapper)
        {
            command.ThrowIfNull(nameof(command));
            rowMapper.ThrowIfNull(nameof(rowMapper));

            var results = new List<T>();

            command.Connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(rowMapper.Invoke(reader));
                }
            }

            return results;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ExecuteAsync<T>(ICommand command, CancellationToken cancellationToken) where T : class, new()
        {
            command.ThrowIfNull(nameof(command));

            IEnumerable<T> results;

            await command.Connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                results = _dataMapper.Map<T>(reader);
            }

            return results;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ExecuteAsync<T>(ICommand command) where T : class, new()
        {
            command.ThrowIfNull(nameof(command));

            IEnumerable<T> results;

            await command.Connection.OpenAsync().ConfigureAwait(false);

            using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                results = _dataMapper.Map<T>(reader);
            }

            return results;
        }

        public async Task<IEnumerable<T>> ExecuteAsync<T>(ICommand command, Func<IDataReader, T> rowMapper, CancellationToken cancellationToken)
        {
            command.ThrowIfNull(nameof(command));
            rowMapper.ThrowIfNull(nameof(rowMapper));

            var results = new List<T>();

            await command.Connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    results.Add(rowMapper.Invoke(reader));
                }
            }

            return results;
        }

        public async Task<IEnumerable<T>> ExecuteAsync<T>(ICommand command, Func<IDataReader, T> rowMapper)
        {
            command.ThrowIfNull(nameof(command));
            rowMapper.ThrowIfNull(nameof(rowMapper));

            var results = new List<T>();

            await command.Connection.OpenAsync().ConfigureAwait(false);
            using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    results.Add(rowMapper.Invoke(reader));
                }
            }

            return results;
        }

        /// <inheritdoc/>
        public int ExecuteNonQuery(ICommand command)
        {
            command.ThrowIfNull(nameof(command));

            command.Connection.Open();

            return command.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteNonQueryAsync(ICommand command, CancellationToken cancellationToken)
        {
            command.ThrowIfNull(nameof(command));

            await command.Connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteNonQueryAsync(ICommand command)
        {
            command.ThrowIfNull(nameof(command));

            await command.Connection.OpenAsync().ConfigureAwait(false);

            return await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public T ExecuteScalar<T>(ICommand command)
        {
            command.ThrowIfNull(nameof(command));

            command.Connection.Open();

            object scalarValue = command.ExecuteScalar();

            return (T)Convert.ChangeType(scalarValue, typeof(T));
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteScalarAsync<T>(ICommand command, CancellationToken cancellationToken)
        {
            command.ThrowIfNull(nameof(command));

            await command.Connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            object scalarValue = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);

            return (T)Convert.ChangeType(scalarValue, typeof(T));
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteScalarAsync<T>(ICommand command)
        {
            command.ThrowIfNull(nameof(command));

            await command.Connection.OpenAsync().ConfigureAwait(false);
            object scalarValue = await command.ExecuteScalarAsync().ConfigureAwait(false);

            return (T)Convert.ChangeType(scalarValue, typeof(T));
        }

        /// <inheritdoc/>
        public T ExecuteSingle<T>(ICommand command) where T : class, new()
        {
            command.ThrowIfNull(nameof(command));

            T result;

            command.Connection.Open();

            using (var reader = command.ExecuteReader())
            {
                result = _dataMapper.MapSingle<T>(reader);
            }

            return result;
        }

        public T ExecuteSingle<T>(ICommand command, Func<IDataReader, T> rowMapper)
        {
            command.ThrowIfNull(nameof(command));
            rowMapper.ThrowIfNull(nameof(rowMapper));

            T result = default;

            command.Connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = rowMapper.Invoke(reader);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteSingleAsync<T>(ICommand command, CancellationToken cancellationToken) where T : class, new()
        {
            command.ThrowIfNull(nameof(command));

            T result;

            await command.Connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                result = _dataMapper.MapSingle<T>(reader);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteSingleAsync<T>(ICommand command) where T : class, new()
        {
            command.ThrowIfNull(nameof(command));

            T result;

            await command.Connection.OpenAsync().ConfigureAwait(false);

            using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                result = _dataMapper.MapSingle<T>(reader);
            }

            return result;
        }

        public async Task<T> ExecuteSingleAsync<T>(ICommand command, Func<IDataReader, T> rowMapper, CancellationToken cancellationToken)
        {
            command.ThrowIfNull(nameof(command));
            rowMapper.ThrowIfNull(nameof(rowMapper));

            T result = default;

            await command.Connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                if (reader.Read())
                {
                    result = rowMapper.Invoke(reader);
                }
            }

            return result;
        }

        public async Task<T> ExecuteSingleAsync<T>(ICommand command, Func<IDataReader, T> rowMapper)
        {
            command.ThrowIfNull(nameof(command));
            rowMapper.ThrowIfNull(nameof(rowMapper));

            T result = default;

            await command.Connection.OpenAsync().ConfigureAwait(false);
            using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                if (reader.Read())
                {
                    result = rowMapper.Invoke(reader);
                }
            }

            return result;
        }
    }
}
