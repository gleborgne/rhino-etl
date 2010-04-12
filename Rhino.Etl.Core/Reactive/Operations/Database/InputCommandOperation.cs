using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Rhino.Etl.Core.Reactive.Activators;

namespace Rhino.Etl.Core.Reactive.Operations
{
    /// <summary>
    /// Observable list of elements
    /// </summary>
    public class InputCommandOperation : AbstractObservableOperation
    {
        private CommandActivator _activator;

        /// <summary>
        /// Constructor of input command operation
        /// </summary>
        /// <param name="activator"></param>
        public InputCommandOperation(CommandActivator activator)
        {
            _activator = activator;    
        }

        /// <summary>
        /// Notifies the observer of a new value in the sequence. It's best to override Dispatch or TreatRow than this method because this method contains pipelining logic.
        /// </summary>
        public override void Trigger()
        {
            try
            {
                _activator.UseCommand(currentCommand =>
                {
                    if (_activator.Prepare != null)
                        _activator.Prepare(currentCommand, null);

                    log4net.LogManager.GetLogger(this.GetType()).Info(DisplayName + " Execute command " + currentCommand.CommandText);

                    if (_activator.IsQuery)
                    {
                        using (IDataReader reader = currentCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Observers.PropagateOnNext(_activator.CreateRowFromReader(reader));
                            }

                        }
                    }
                    else
                    {
                        currentCommand.ExecuteNonQuery();
                    }
                });
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger(this.GetType()).Error("Operation error", ex);
                Observers.PropagateOnError(ex);
            }
            finally
            {
                _activator.Release();
            }
            
            Completed = true;
            Observers.PropagateOnCompleted();
        }
    }
}
