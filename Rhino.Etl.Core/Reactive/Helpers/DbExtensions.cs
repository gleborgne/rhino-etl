using System;
using System.Data;
using Rhino.Etl.Core.Reactive.Activators;
using Rhino.Etl.Core.Reactive.Operations.Database;

namespace Rhino.Etl.Core.Reactive
{
    /// <summary>
    /// Extension methods for database operations
    /// </summary>
    public static class DbExtensions
    {
        /// <summary>
        /// Apply a command operation
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="activator">command parameters</param>
        /// <returns>command operation</returns>
        public static CommandOperation Command(this IOperation observed, CommandActivator activator)
        {
            CommandOperation cmd = new CommandOperation(activator);
            observed.Subscribe(cmd);
            return cmd;
        }

        /// <summary>
        /// Apply a command operation
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="connStr">Name of a connection string defined in the application configuration file</param>
        /// <param name="CommandText">text of the command</param>
        /// <returns>command operation</returns>
        public static CommandOperation Command(this IOperation observed, string connStr, string CommandText)
        {
            return observed.Command(connStr, CommandText, false, null);
        }

        /// <summary>
        /// Apply a command operation
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="connection">Database connection</param>
        /// <param name="CommandText">text of the command</param>
        /// <returns>command operation</returns>
        public static CommandOperation Command(this IOperation observed, IDbConnection connection, string CommandText)
        {
            return observed.Command(connection, CommandText, false, null);
        }

        /// <summary>
        /// Apply a command operation
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="connStr">name of a connection string defined in the application configuration file</param>
        /// <param name="prepare">callback method to prepare the command</param>
        /// <returns>command operation</returns>
        public static CommandOperation Command(this IOperation observed, string connStr, Action<IDbCommand, Row> prepare)
        {
            return observed.Command(connStr, null, false, prepare);
        }

        /// <summary>
        /// Apply a command operation
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="connection">Database connection</param>
        /// <param name="prepare">callback method to prepare the command</param>
        /// <returns>command operation</returns>
        public static CommandOperation Command(this IOperation observed, IDbConnection connection, Action<IDbCommand, Row> prepare)
        {
            return observed.Command(connection, null, false, prepare);
        }

        /// <summary>
        /// Apply a command operation
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="connStr">Name of a connection string defined in the application configuration file</param>
        /// <param name="CommandText">text of the command</param>
        /// <param name="prepare">callback method to prepare the command</param>
        /// <returns>command operation</returns>
        public static CommandOperation Command(this IOperation observed, string connStr, string CommandText, Action<IDbCommand, Row> prepare)
        {
            return observed.Command(connStr, null, false, prepare);
        }

        /// <summary>
        /// Apply a command operation
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="connection">Database connection</param>
        /// <param name="CommandText">text of the command</param>
        /// <param name="prepare">callback method to prepare the command</param>
        /// <returns>command operation</returns>
        public static CommandOperation Command(this IOperation observed, IDbConnection connection, string CommandText, Action<IDbCommand, Row> prepare)
        {
            return observed.Command(connection, null, false, prepare);
        }

        /// <summary>
        /// Apply a command operation
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="connection">Database connection</param>
        /// <param name="CommandText">text of the command</param>
        /// <param name="isQuery">indicate if the command is a query</param>
        /// <param name="prepare">callback method to prepare the command</param>
        /// <returns>command operation</returns>
        public static CommandOperation Command(this IOperation observed, IDbConnection connection, string CommandText, bool isQuery, Action<IDbCommand, Row> prepare)
        {
            CommandActivator activator = new CommandActivator();

            activator.Connection = connection;
            activator.CommandText = CommandText;
            activator.Prepare = prepare;
            activator.IsQuery = isQuery;

            return observed.Command(activator);
        }

        /// <summary>
        /// Apply a command operation
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="connStr">Name of a connection string defined in the application configuration file</param>
        /// <param name="CommandText">text of the command</param>
        /// <param name="isQuery">indicate if the command is a query</param>
        /// <param name="prepare">callback method to prepare the command</param>
        /// <returns>command operation</returns>
        public static CommandOperation Command(this IOperation observed, string connStr, string CommandText, bool isQuery, Action<IDbCommand, Row> prepare)
        {
            CommandActivator activator = new CommandActivator();

            activator.ConnStringName = connStr;
            activator.CommandText = CommandText;
            activator.Prepare = prepare;
            activator.IsQuery = isQuery;

            return observed.Command(activator);
        }
    }
}