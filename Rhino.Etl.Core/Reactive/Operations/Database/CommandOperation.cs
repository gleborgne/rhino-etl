using System;
using Rhino.Etl.Core.Reactive.Activators;

namespace Rhino.Etl.Core.Reactive.Operations.Database
{
    /// <summary>
    /// Operation that apply a database command. If this operation is a starting point (it does not observe anything), calling the process method will execute the command and start the pipeline.
    /// </summary>
    public class CommandOperation : AbstractOperation
    {
        private CommandActivator _activator;
        
        /// <summary>
        /// Command operation constructor
        /// </summary>
        /// <param name="activator">command parameters</param>
        public CommandOperation(CommandActivator activator)
        {
            _activator = activator;
        }

        /// <summary>
        /// Notifies the observer of a new value in the sequence. It's best to override Dispatch or TreatRow than this method because this method contains pipelining logic.
        /// </summary>
        public override void OnNext(Row value)
        {
            CountTreated++;
            if (Observers.Count > 0 && value != null)
            {
                base.OnNext(value);
            }

            try
            {
                _activator.UseCommand(currentCommand =>
                {
                    if (_activator.Prepare != null)
                        _activator.Prepare(currentCommand, value);

                    log4net.LogManager.GetLogger(this.GetType()).Info(DisplayName + " Execute command " + currentCommand.CommandText);
                    
                    currentCommand.ExecuteNonQuery();
                });
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger(this.GetType()).Error("command execution error", ex);
                OnError(ex);
            }

            if (_activator.IsQuery && value == null)
            {
                OnCompleted();
            }
        }

        /// <summary>
        /// Notifies the observers of the end of the sequence.
        /// </summary>
        public override void OnCompleted()
        {
            _activator.Release();
            base.OnCompleted();
        }
    }
}