﻿using System;
using System.IO;
using System.Collections;
using Rhino.Etl.Core.Files;

namespace Rhino.Etl.Core.Reactive.Operations.File
{
    /// <summary>
    /// Operation for reading a file
    /// </summary>
    /// <typeparam name="T">type of the objects in the file</typeparam>
    public class InputFileOperation<T> : AbstractObservableOperation
    {
        private string _filename;
        private Stream _strm;
        private StreamReader _strmReader;

        /// <summary>
        /// File read constructor
        /// </summary>
        /// <param name="filename">full path to the file</param>
        public InputFileOperation(string filename)
        {
            _filename = filename;
        }

        /// <summary>
        /// File read constructor
        /// </summary>
        /// <param name="strm">Stream to the file</param>
        public InputFileOperation(Stream strm)
        {
            _strm = strm;
        }

        /// <summary>
        /// File read constructor
        /// </summary>
        /// <param name="strmReader">Stream to the file</param>
        public InputFileOperation(StreamReader strmReader)
        {
            _strmReader = strmReader;
        }

        /// <summary>
        /// Notifies the observer of a new value in the sequence. It's best to override Dispatch or TreatRow than this method because this method contains pipelining logic.
        /// </summary>
        public override void Trigger()
        {
            CountTreated++;

            try
            {
                IEnumerator fList = null;

                if (_strm != null)
                {
                    using (StreamReader reader = new StreamReader(_strm))
                    {
                        fList = FluentFile.For<T>().From(reader).GetEnumerator();                        
                    }
                }
                else if (_strmReader != null)
                {
                    fList = FluentFile.For<T>().From(_strmReader).GetEnumerator();                    
                }
                else if (_filename != null)
                {
                    fList = FluentFile.For<T>().From(_filename).GetEnumerator();                    
                }
                IterateElements(fList);
                Completed = true;
                Observers.PropagateOnCompleted();
            }
            catch (Exception ex)
            {
                Completed = true;
                Observers.PropagateOnError(ex);
            }

            
        }

        private void IterateElements(IEnumerator fList)
        {
            while (fList.MoveNext())
            {
                Observers.PropagateOnNext(Row.FromObject(fList.Current));
            }
        }
    }
}