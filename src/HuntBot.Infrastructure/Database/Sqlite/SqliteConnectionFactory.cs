using System;
using System.Data;
using System.Threading;
using Microsoft.Data.Sqlite;
using Serilog;

namespace HuntBot.Infrastructure.Database.Sqlite
{
    /// <summary>
    /// Provides connectivity to a SQLite database using a singleton-driven multi-reader/single-writer strategy.
    /// </summary>
    public sealed class SqliteConnectionFactory
    {
        /// <summary>
        /// Indicates whether or not the resources have been disposed of.
        /// </summary>
        private bool _disposed;
        
        /// <summary>
        /// Singleton SQLite database connection.
        /// </summary>
        private static SqliteConnection _sqliteConnection;

        /// <summary>
        /// Singleton <see cref="ReaderWriterLock"/> instance with which read and write locks are placed on the database connection.
        /// </summary>
        private static ReaderWriterLockSlim _readerWriterLock;

        /// <summary>
        /// Singleton <see cref="SqliteConnectionFactory"/> instance.
        /// </summary>
        /// <returns>The singleton <see cref="SqliteConnectionFactory"/> instance.</returns>
        private static readonly SqliteConnectionFactory _sqliteConnectionFactoryInstance = new SqliteConnectionFactory();
        
        /// <summary>
        /// The mode of the currently locked connection.
        /// </summary>
        private static SqliteConnectionMode _connectionMode;

        /// <summary>
        /// Initializes a new instance of <see cref="SqliteConnectionFactory"/>.
        /// </summary>
        private SqliteConnectionFactory()
        {
            _readerWriterLock = new ReaderWriterLockSlim();
            _sqliteConnection = CreateConnection("huntbot.db");
            _connectionMode = SqliteConnectionMode.NotLocked;
        }

        /// <summary>
        /// Creates an opens a new <see cref="SqliteConnection"/> instance.
        /// </summary>
        /// <param name="databaseName">The name of the database to open.</param>
        /// <returns>A newly created <see cref="SqliteConnection"/> instance for the given database.</returns>
        private static SqliteConnection CreateConnection(string databaseName)
        {
            try
            {       
                SqliteConnection connection = new SqliteConnection($"Data Source={databaseName}");
                connection.Open();

                return connection;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Unable to open connection to SQLite database '{databaseName}'.", databaseName);
                return null;
            }
        }

        /// <summary>
        /// Gets the singleton instance of <see cref="SqliteConnectionFactory"/>.
        /// </summary>
        /// <returns>The singleton instance of <see cref="SqliteConnectionFactory"/>.</returns>
        public static SqliteConnectionFactory GetInstance()
        {
            return _sqliteConnectionFactoryInstance;
        }

        /// <summary>
        /// Attempts to get the open <see cref="SqliteConnection"/> instance.
        /// </summary>
        /// <param name="mode">The mode for which the connection is being retrieved.</param>
        /// <returns>The open <see cref="SqliteConnection"/> connection.</returns>
        public SqliteConnection GetConnection(SqliteConnectionMode mode)
        {
            try
            {
                if (_sqliteConnection is not null)
                {
                    Log.Logger.Information("Attempting to obtain SQLite connection lock.");

                    if (mode == SqliteConnectionMode.Read)
                    {
                        if (!_readerWriterLock.TryEnterReadLock(3000))
                        {
                            return null;
                        }

                        _connectionMode = SqliteConnectionMode.Read;
                    }
                    else
                    {
                        if (!_readerWriterLock.TryEnterWriteLock(3000))
                        {
                            return null;
                        }

                        _connectionMode = SqliteConnectionMode.Write;
                    }                    

                    Log.Logger.Information("Lock obtained."); 

                    return _sqliteConnection;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Unable to retrieve SQLite database connection.");
            }

            return null;
        }
        
        /// <summary>
        /// Releases the lock on the open connection.
        /// </summary>
        public void ReleaseConnection()
        {
            try
            {
                Log.Logger.Information("Releasing lock on SQLite connection");

                if (_connectionMode == SqliteConnectionMode.NotLocked)
                {
                    return;
                }
                else if (_connectionMode == SqliteConnectionMode.Read)
                {
                    _readerWriterLock.ExitReadLock();
                }
                else
                {
                    _readerWriterLock.ExitWriteLock();
                }

                _connectionMode = SqliteConnectionMode.NotLocked;
                Log.Logger.Information("Lock released.");
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to release SQLite connection.");
            }
        }

        /// <summary>
        /// Closes the open SQLite database connection.
        /// </summary>
        public void CloseConnection()
        {
            if (_sqliteConnection is not null && _sqliteConnection.State is not ConnectionState.Closed)
            {
                try
                {
                    _sqliteConnection.Close();
                    _sqliteConnection.Dispose();

                    Log.Logger.Information("SQLite database connection closed.");
                }
                catch (Exception ex)
                {
                    Log.Logger.Fatal(ex, "Unable to close SQLite database connection.");
                }
                finally
                {
                    _sqliteConnection = null;
                }
            }
        }

        /// <summary>
        /// Provides access to manually disposal of resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gracefully disposes of SQLite connection.
        /// </summary>
        /// <param name="disposing">Indicative of whether or not entering disposing state.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if(disposing)
                {
                    Log.Logger.Information("Disposing of SQLite database connection.");
                    CloseConnection();
                }

                _disposed = true;
            }
        }
        
        ~SqliteConnectionFactory()
        {
            Dispose(false);
        }
    }
}