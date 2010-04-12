using System;
using System.Data;
using System.IO;
using Rhino.Etl.Core.Reactive.Activators;
using Rhino.Etl.Core.Reactive.Operations;
using Rhino.Etl.Core.Reactive.Operations.Database;
using Rhino.Etl.Core.Reactive.Operations.File;
using System.Collections.Generic;

namespace Rhino.Etl.Core.Reactive
{
    /// <summary>
    /// Helper call for starting a pipeline process
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// Apply a database query command
        /// </summary>
        /// <param name="connStr">Name of a connection string defined in the application configuration file</param>
        /// <param name="CommandText">Text of the command</param>
        /// <returns>command operation</returns>
        public static InputCommandOperation Query(string connStr, string CommandText)
        {
            return Command(connStr, CommandText, true, null);
        }

        /// <summary>
        /// Apply a database query command
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="CommandText">Text of the command</param>
        /// <returns>command operation</returns>
        public static InputCommandOperation Query(IDbConnection connection, string CommandText)
        {
            return Command(connection, CommandText, true, null);
        }

        /// <summary>
        /// Apply a database non query command
        /// </summary>
        /// <param name="connStr">Name of a connection string defined in the application configuration file</param>
        /// <param name="CommandText">Text of the command</param>
        /// <returns>command operation</returns>
        public static InputCommandOperation NonQuery(string connStr, string CommandText)
        {
            return Command(connStr, CommandText, false, null);
        }

        /// <summary>
        /// Apply a database non query command
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="CommandText">Text of the command</param>
        /// <returns>command operation</returns>
        public static InputCommandOperation NonQuery(IDbConnection connection, string CommandText)
        {
            return Command(connection, CommandText, false, null);
        }

        /// <summary>
        /// Apply a database command
        /// </summary>
        /// <param name="activator">command parameters</param>
        /// <returns>command operation</returns>
        public static InputCommandOperation Command(CommandActivator activator)
        {
            InputCommandOperation cmd = new InputCommandOperation(activator);
            return cmd;
        }

        /// <summary>
        /// Apply a database command
        /// </summary>
        /// <param name="connStr">Name of a connection string defined in the application configuration file</param>
        /// <param name="CommandText">Text of the command</param>
        /// <param name="isQuery">Indicate if the command is a query</param>
        /// <param name="Prepare">Callback method to prepare the command</param>
        /// <returns>Command operation</returns>
        public static InputCommandOperation Command(string connStr, string CommandText, bool isQuery, Action<IDbCommand, Row> Prepare)
        {
            CommandActivator activator = new CommandActivator();
            activator.ConnStringName = connStr;
            activator.CommandText = CommandText;
            activator.Prepare = Prepare;
            activator.IsQuery = isQuery;

            return Command(activator);
        }

        /// <summary>
        /// Apply a database command
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="CommandText">Text of the command</param>
        /// <param name="isQuery">Indicate if the command is a query</param>
        /// <param name="Prepare">Callback method to prepare the command</param>
        /// <returns>Command operation</returns>
        public static InputCommandOperation Command(IDbConnection connection, string CommandText, bool isQuery, Action<IDbCommand, Row> Prepare)
        {
            CommandActivator activator = new CommandActivator();
            activator.Connection = connection;
            activator.CommandText = CommandText;
            activator.Prepare = Prepare;
            activator.IsQuery = isQuery;

            return Command(activator);
        }

        /// <summary>
        /// Input data from a file
        /// </summary>
        /// <typeparam name="T">type of the object used to read the file content</typeparam>
        /// <param name="filename">full path to the file</param>
        /// <returns>file read operation</returns>
        public static InputFileOperation<T> ReadFile<T>(string filename)
        {
            return new InputFileOperation<T>(filename);
        }

        /// <summary>
        /// Input data from a file
        /// </summary>
        /// <typeparam name="T">type of the object used to read the file content</typeparam>
        /// <param name="strm">file stream</param>
        /// <returns>file read operation</returns>
        public static InputFileOperation<T> ReadFile<T>(Stream strm)
        {
            return new InputFileOperation<T>(strm);
        }

        /// <summary>
        /// Input data from a file
        /// </summary>
        /// <typeparam name="T">type of the object used to read the file content</typeparam>
        /// <param name="reader">file stream</param>
        /// <returns>file read operation</returns>
        public static InputFileOperation<T> ReadFile<T>(StreamReader reader)
        {
            return new InputFileOperation<T>(reader);
        }

        /// <summary>
        /// Input data from an enumerable
        /// </summary>
        /// <typeparam name="T">type of the object used in the enumerable</typeparam>
        /// <param name="source">data source</param>
        /// <returns>enumerable operation</returns>
        public static InputEnumerableOperation<T> From<T>(IEnumerable<T> source) where T : class
        {
            return new InputEnumerableOperation<T>(source);
        }
    }
}