using System;
using Rhino.Etl.Core.Reactive.Operations;
using System.Threading;

namespace Rhino.Etl.Core.Reactive
{
    /// <summary>
    /// Extension methods for common operations
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Apply an action on the rows
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <returns>resulting operation</returns>
        public static EtlResult Start(this IObservableOperation observed)
        {
            var op = new StartOperation();
            observed.Subscribe(op);
            op.Trigger();
            return op.Result;
        }

        /// <summary>
        /// Start the operation in a thread. Start method calls are bubbled up through the pipeline
        /// </summary>
        public static EtlResult StartInThread(this IObservableOperation observed)
        {
            var op = new StartOperation();
            observed.Subscribe(op);            
            
            op.Result.Thread = new Thread(new ThreadStart(op.Trigger));
            op.Result.Thread.Start();
            
            return op.Result;
        }

        /// <summary>
        /// Execute the pipeline
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <returns>List of row resulting from the pipeline</returns>
        public static EtlFullResult Execute(this IObservableOperation observed)
        {
            RecordOperation record = new RecordOperation();
            observed.Subscribe(record);
            record.Start();
            return record.Result;
        }

        /// <summary>
        /// Execute the pipeline
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <returns>List of row resulting from the pipeline</returns>
        public static EtlFullResult ExecuteInThread(this IObservableOperation observed)
        {
            RecordOperation record = new RecordOperation();
            observed.Subscribe(record);
            record.Result.Thread = new Thread(new ThreadStart(record.Trigger));
            record.Result.Thread.Start();

            return record.Result;
        }
        
        /// <summary>
        /// Set the name of an operation
        /// </summary>
        /// <typeparam name="T">type of the operation</typeparam>
        /// <param name="operation">operation to name</param>
        /// <param name="name">name to apply</param>
        /// <returns>named operation</returns>
        public static T Named<T>(this T operation, string name) where T : IObservableOperation
        {
            operation.Name = name;
            return operation;
        }

        /// <summary>
        /// Operation that record values in a list. 
        /// Beware that this operation keep a list of all data in memory
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <returns>resulting operation</returns>
        public static RecordOperation Record(this IObservableOperation observed)
        {
            RecordOperation observer = new RecordOperation();
            observed.Subscribe(observer);
            return observer;
        }
        
        /// <summary>
        /// Add an operation in the pipeline.
        /// </summary>
        /// <typeparam name="T">type of the operation</typeparam>
        /// <param name="observed">observed operation</param>
        /// <param name="args">arguments for the constructor</param>
        /// <returns>resulting operation</returns>
        public static T Operation<T>(this IObservableOperation observed, params object[] args) where T : IOperation
        {
            T op = (T)Activator.CreateInstance(typeof(T), args);
            observed.Subscribe(op);
            return op;
        }

        /// <summary>
        /// Transform the pipelined row
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="transform">callback method for transforming the row</param>
        /// <returns>resulting operation</returns>
        public static TransformOperation Transform(this IObservableOperation observed, Func<Row, Row> transform)
        {
            TransformOperation op = new TransformOperation(transform);
            observed.Subscribe(op);
            return op;
        }

        /// <summary>
        /// Filter the rows
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="filterexpr">callback method for filtering</param>
        /// <returns>resulting operation</returns>
        public static FilterOperation Filter(this IObservableOperation observed, Predicate<Row> filterexpr)
        {
            FilterOperation op = new FilterOperation(filterexpr);
            observed.Subscribe(op);
            return op;
        } 
       
        /// <summary>
        /// Apply an action on the rows
        /// </summary>
        /// <param name="observed">observed operation</param>
        /// <param name="rowact">callback method for the action</param>
        /// <returns>resulting operation</returns>
        public static ApplyOperation Apply(this IObservableOperation observed, Action<Row> rowact)
        {
            ApplyOperation op = new ApplyOperation(rowact);
            observed.Subscribe(op);
            return op;
        }
    }
}
