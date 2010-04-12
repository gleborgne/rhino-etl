using System;

namespace Rhino.Etl.Core.Reactive.Operations
{
    /// <summary>
    /// Operation for transforming the current <see cref="Row"/>
    /// </summary>
    public class TransformOperation : AbstractOperation
    {
        private Func<Row, Row> _transformact;

        /// <summary>
        /// Transform operation constructor
        /// </summary>
        /// <param name="transformact">callback method for transforming the <see cref="Row"/></param>
        public TransformOperation(Func<Row, Row> transformact)
        {
            _transformact = transformact;
        }

        /// <summary>
        /// Method called by OnNext > Dispatch to process the notified value. This method just return the value and could be overriden in subclasses
        /// </summary>
        /// <param name="value">pipelined value</param>
        /// <returns>treated row</returns>
        protected override Row TreatRow(Row value)
        {
            return _transformact(value);
        }
    }
}