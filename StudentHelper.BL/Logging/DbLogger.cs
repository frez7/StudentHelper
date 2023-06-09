﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using StudentHelper.Model.Models.Common.Other;
using StudentHelper.Model.Models.Common;
using Microsoft.Extensions.DependencyInjection;

namespace StudentHelper.BL.Logging
{
    public class DbLogger : ILogger
    {
        private readonly DbLoggerProvider _dbLoggerProvider;
        public DbLogger([NotNull] DbLoggerProvider dbLoggerProvider)
        {
            _dbLoggerProvider = dbLoggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, 
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var threadId = Thread.CurrentThread.ManagedThreadId;
            if (eventId == 20101 || eventId == 30000 || eventId == 14)
            {
                return;
            }
            using (var connection = new SqlConnection(_dbLoggerProvider.Options.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = string.Format("INSERT INTO {0} " +
                        "([LogLevel], [ThreadId], [EventId], [EventName], [Message], [ExceptionMessage], [ExceptionStackTrace], [ExceptionSource], [Created]) " +
                        "VALUES (@LogLevel, @ThreadId, @EventId, @EventName, @Message, @ExceptionMessage, @ExceptionStackTrace, @ExceptionSource, @Created)",
                        _dbLoggerProvider.Options.Logs);
                    if (!string.IsNullOrWhiteSpace(logLevel.ToString()))
                    {
                        command.Parameters.Add(new SqlParameter("@LogLevel", $"{logLevel.ToString()}"));
                        
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@LogLevel", $"{LogLevel.Debug.ToString()}"));
                    }
                    

                    command.Parameters.Add(new SqlParameter("@ThreadId", $"{threadId}"));
                    command.Parameters.Add(new SqlParameter("@EventId", $"{eventId.Id}"));

                    if (!string.IsNullOrWhiteSpace(eventId.Name))
                    {
                        command.Parameters.Add(new SqlParameter("@EventName", $"{eventId.Name}"));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@EventName", $"[IS NULL]"));
                    }

                    command.Parameters.Add(new SqlParameter("@Message", formatter(state, exception)));

                    if (exception != null)
                    {
                        if (!string.IsNullOrWhiteSpace(exception.Message))
                        {
                            command.Parameters.Add(new SqlParameter("@ExceptionMessage", $"{exception.Message}"));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ExceptionMessage", $"[IS NULL]"));
                        }
                        if (!string.IsNullOrWhiteSpace(exception.StackTrace))
                        {
                            command.Parameters.Add(new SqlParameter("@ExceptionStackTrace", $"{exception.StackTrace}"));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ExceptionStackTrace", $"[IS NULL]"));
                        }
                        if (!string.IsNullOrWhiteSpace(exception.Message))
                        {
                            command.Parameters.Add(new SqlParameter("@ExceptionSource", $"{exception.Message}"));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ExceptionSource", $"[IS NULL]"));
                        }
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@ExceptionMessage", $"[IS NULL]"));
                        command.Parameters.Add(new SqlParameter("@ExceptionStackTrace", $"[IS NULL]"));
                        command.Parameters.Add(new SqlParameter("@ExceptionSource", $"[IS NULL]"));
                        
                    }
                    command.Parameters.Add(new SqlParameter("@Created", DateTimeOffset.Now));

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
