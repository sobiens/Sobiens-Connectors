#if General
// QueuedBackgroundWorkerWithoutAbusingConstructorInfo
//
// Refer to the comments in QueuedBackgroundWorker.cs for a description of the
// QueuedBackgroundWorker class, from which the code in this file was cloned.
//
// If after reading the comments associated with the RunWorkerCompletedEventArgsWithUserState
// class at the bottom of QueuedBackgroundWorker.cs, you want to use a
// a variation of the QueuedBackgroundWorker class that doesn't bend the rules
// with respect to how ConstructorInfo was used, then you can use the
// QueuedBackgroundWorker class defined in this file instead.
//
// As packaged in the original sample, the build action for this source file is set to be none,
// while the build action for QueuedBackgroundWorker.cs is set to compile.  So this file
// is ignored by the compiler in the original sample project, and exists for reference only.
//
// Mike Woodring
// http://www.bearcanyon.com
// http://www.pluralsight.com/mike
//
using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

namespace Sobiens.Connectors.Common.Threading
{
    public delegate void RunWorkerCompletedWithUserStateEventHandler(object sender, RunWorkerCompletedEventArgsWithUserState e);

    public class QueuedBackgroundWorker : Component
    {
        Queue<OperationInfo> _operationQueue = new Queue<OperationInfo>();  // Holds pending (possibly canceled) operation requests.
        Hashtable _userStateToOperationMap = new Hashtable();               // Maps user-supplied keys onto pending OperationInfo.
        object _collectionsLock = new object();                             // Used to synchronize all access to both of the above two collections.
        
        bool _supportsProgress;                                             // Set at construction.  Indicates whether this instance supports calls to ReportProgress.
        bool _supportsCancellation;                                         // Set at construction.  Indicates whether this instance supports calls to CancelAsync/CancelAllAsync.
        bool _cancelAllPending = false;

        delegate RunWorkerCompletedEventArgsWithUserState OperationHandlerDelegate(OperationInfo opInfo);

        // OperationRequest
        //
        // This class represents everything this component needs to know about
        // in order to carry out a single operation as requested by a call to
        // RunWorkerAsync.
        //
        class OperationRequest
        {
            internal readonly object UserState;
            internal readonly AsyncOperation AsyncOperation;
            internal readonly OperationHandlerDelegate OperationHandler;
            bool _cancelPending = false;

            internal OperationRequest(object userState, OperationHandlerDelegate operationHandler)
            {
                UserState = userState;
                OperationHandler = operationHandler;
                AsyncOperation = AsyncOperationManager.CreateOperation(this);
            }

            internal bool CancelPending
            {
                get { return (_cancelPending); }
            }

            internal void Cancel()
            {
                _cancelPending = true;
            }
        }

        // OperationInfo
        //
        // This class combines a request (OperationRequest) with a result
        // (RunWorkerCompletedEventArgsWithUserStateWithoutAbusingConstructorInfo), and is what's queued up
        // and processed by this component.
        //
        class OperationInfo
        {
            OperationRequest _request;
            RunWorkerCompletedEventArgsWithUserState _result;

            internal OperationInfo(OperationRequest request)
            {
                _request = request;
                _result = null;
            }

            internal OperationRequest OperationRequest
            {
                get { return (_request); }
            }

            internal RunWorkerCompletedEventArgsWithUserState OperationResult
            {
                get
                {
                    if (_result == null)
                    {
                        throw new InvalidOperationException("The operation result has not been set yet.");
                    }

                    return(_result);
                }

                set
                {
                    if (_result != null)
                    {
                        throw new InvalidOperationException("The operation result has already been set.");
                    }

                    _result = value;
                }
            }
        }

        // Context: client.
        //
        public QueuedBackgroundWorker()
            : this(true, true)
        {
        }

        public QueuedBackgroundWorker(bool supportsProgress, bool supportsCancellation)
        {
            _supportsProgress = supportsProgress;
            _supportsCancellation = supportsCancellation;
        }

        // Context: client.
        //
        public void RunWorkerAsync(object userState)
        {
            if (userState == null)
            {
                throw new ArgumentNullException("userState cannot be null.");
            }

            int prevCount;
            OperationRequest opRequest = new OperationRequest(userState, OperationHandler);
            OperationInfo opInfo = new OperationInfo(opRequest);

            lock (_collectionsLock)
            {
                if (_userStateToOperationMap.ContainsKey(userState))
                {
                    throw new InvalidOperationException("The specified userKey has already been used to identify a pending operation.  Each userState parameter must be unique.");
                }

                // Make a note of the current pending queue size.  If it's zero at this point,
                // we'll need to kick off an operation.
                //
                prevCount = _operationQueue.Count;

                // Place the new work item on the queue & also in the userState-to-OperationInfo map.
                //
                _operationQueue.Enqueue(opInfo);
                _userStateToOperationMap[userState] = opInfo;
            }

            if (prevCount == 0)
            {
                // We just queued up the first item - kick off the operation.
                //
                opRequest.OperationHandler.BeginInvoke(opInfo, OperationHandlerDone, opInfo);
            }
        }

        // Context: client | async.
        //
        public void ReportProgress(int percentComplete, object userState)
        {
            if (!_supportsProgress)
            {
                throw new InvalidOperationException("This instance of the QueuedBackgroundWorker does not support progress notification.");
            }

            OperationInfo opInfo;

            lock (_collectionsLock)
            {
                opInfo = _userStateToOperationMap[userState] as OperationInfo;
            }

            if (opInfo != null)
            {
                RaiseProgressChangedEventFromAsyncContext(percentComplete, userState, opInfo);
            }
        }

        public bool SupportsProgressReports
        {
            get { return (_supportsProgress); }
        }

        public bool SupportsCancellation
        {
            get { return (_supportsCancellation); }
        }

        // DoWork event support.
        //
        DoWorkEventHandler _doWork;
        object _doWorkLock = new object();

        public event DoWorkEventHandler DoWork
        {
            add
            {
                lock (_doWorkLock)
                {
                    _doWork += value;
                }
            }

            remove
            {
                lock (_doWorkLock)
                {
                    _doWork -= value;
                }
            }
        }

        void RaiseDoWorkEventFromAsyncContext(DoWorkEventArgs eventArgs)
        {
            Delegate[] targets;

            lock (_doWorkLock)
            {
                targets = _doWork.GetInvocationList();
            }

            foreach (DoWorkEventHandler handler in targets)
            {
                handler(this, eventArgs);
            }
        }

        // ProgressChanged event support.
        //
        ProgressChangedEventHandler _operationProgressChanged;
        object _operationProgressChangedLock = new object();

        public event ProgressChangedEventHandler ProgressChanged
        {
            add
            {
                lock (_operationProgressChangedLock)
                {
                    _operationProgressChanged += value;
                }
            }

            remove
            {
                lock (_operationProgressChangedLock)
                {
                    _operationProgressChanged -= value;
                }
            }
        }

        void RaiseProgressChangedEventFromAsyncContext(int percentComplete, object userState, OperationInfo opInfo)
        {
            ProgressChangedEventArgs eventArgs = new ProgressChangedEventArgs(percentComplete, userState);
            opInfo.OperationRequest.AsyncOperation.Post(RaiseProgressChangedEventFromClientContext, eventArgs);
        }

        void RaiseProgressChangedEventFromClientContext(object state)
        {
            ProgressChangedEventArgs eventArgs = (ProgressChangedEventArgs)state;
            Delegate[] targets;

            lock (_operationProgressChangedLock)
            {
                targets = _operationProgressChanged.GetInvocationList();
            }

            foreach (ProgressChangedEventHandler handler in targets)
            {
                try
                {
                    handler(this, eventArgs);
                }
                catch
                {
                }
            }
        }

        // RunWorkerCompleted event support.
        //
        RunWorkerCompletedWithUserStateEventHandler _operationCompleted;
        object _operationCompletedLock = new object();

        public event RunWorkerCompletedWithUserStateEventHandler RunWorkerCompleted
        {
            add
            {
                lock (_operationCompletedLock)
                {
                    _operationCompleted += value;
                }
            }

            remove
            {
                lock (_operationCompletedLock)
                {
                    _operationCompleted -= value;
                }
            }
        }

        void RaiseWorkCompletedEventFromAsyncContext(OperationInfo opInfo)
        {
            opInfo.OperationRequest.AsyncOperation.PostOperationCompleted(RaiseWorkCompletedEventFromClientContext, opInfo);
        }

        void RaiseWorkCompletedEventFromClientContext(object state)
        {
            OperationInfo opInfo = (OperationInfo)state;
            RunWorkerCompletedEventArgsWithUserState eventArgs = opInfo.OperationResult;
            Delegate[] targets;

            lock (_operationCompletedLock)
            {
                targets = _operationCompleted.GetInvocationList();
            }

            foreach (RunWorkerCompletedWithUserStateEventHandler handler in targets)
            {
                try
                {
                    handler(this, eventArgs);
                }
                catch
                {
                }
            }

            // Now that we're done calling back to the client to let them know
            // that the operation has completed, remove this operation from the
            // queue and check to see if we need to start another operation.
            //
            OperationRequest opRequest = opInfo.OperationRequest;
            OperationInfo nextOp = null;

            lock (_collectionsLock)
            {
                if ((_operationQueue.Peek() != opInfo) || !_userStateToOperationMap.ContainsKey(opRequest.UserState))
                {
                    throw new InvalidOperationException("Something freaky happened.");
                }

                _operationQueue.Dequeue();
                _userStateToOperationMap.Remove(opInfo);

                if (_operationQueue.Count > 0)
                {
                    nextOp = _operationQueue.Peek();
                }
                else
                {
                    _cancelAllPending = false;
                }
            }

            if (nextOp != null)
            {
                // We have more work items pending.  Kick off another operation.
                //
                nextOp.OperationRequest.OperationHandler.BeginInvoke(nextOp, OperationHandlerDone, nextOp);
            }
        }

        // Context: client | async.
        //
        public void CancelAsync(object userState)
        {
            if (!_supportsCancellation)
            {
                throw new InvalidOperationException("This instance of the QueuedBackgroundWorker does not support cancellation.");
            }

            OperationInfo opInfo = GetOperationForUserKey(userState);

            if (opInfo != null)
            {
                opInfo.OperationRequest.Cancel();
            }
        }

        // Context: client | async.
        //
        public void CancelAllAsync()
        {
            if (!_supportsCancellation)
            {
                throw new InvalidOperationException("This instance of the QueuedBackgroundWorker does not support cancellation.");
            }

            lock (_collectionsLock)
            {
                _cancelAllPending = true;

                foreach (object key in _userStateToOperationMap.Keys)
                {
                    OperationInfo opInfo = _userStateToOperationMap[key] as OperationInfo;

                    if (opInfo != null)
                    {
                        opInfo.OperationRequest.Cancel();
                    }
                }
            }
        }

        // Context: client | async.
        //
        public bool IsCancellationPending(object userState)
        {
            if (!_supportsCancellation)
            {
                return (false);
            }

            if (_cancelAllPending)
            {
                return (true);
            }

            OperationInfo opInfo = GetOperationForUserKey(userState);
            return (opInfo != null ? opInfo.OperationRequest.CancelPending : false);
        }

        OperationInfo GetOperationForUserKey(object userKey)
        {
            lock (_collectionsLock)
            {
                return (_userStateToOperationMap[userKey] as OperationInfo);
            }
        }

        // Context: async.
        //
        RunWorkerCompletedEventArgsWithUserState OperationHandler(OperationInfo opInfo)
        {
            object userState = opInfo.OperationRequest.UserState;
            DoWorkEventArgs eventArgs = new DoWorkEventArgs(userState);
            
            try
            {
                RaiseDoWorkEventFromAsyncContext(eventArgs);

                if (eventArgs.Cancel)
                {
                    opInfo.OperationRequest.Cancel(); // For the sake of completeness.
                    return new RunWorkerCompletedEventArgsWithUserState(null, null, true, userState);
                }
                else
                {
                    return new RunWorkerCompletedEventArgsWithUserState(eventArgs.Result, null, false, userState);
                }
            }
            catch( Exception err )
            {
                return new RunWorkerCompletedEventArgsWithUserState(null, err, false, userState);
            }
        }

        // Context: async.
        //
        void OperationHandlerDone(IAsyncResult ar)
        {
            OperationInfo opInfo = (OperationInfo)ar.AsyncState;
            opInfo.OperationResult = opInfo.OperationRequest.OperationHandler.EndInvoke(ar);
            RaiseWorkCompletedEventFromAsyncContext(opInfo);
        }

        // Context: client.
        //
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CancelAllAsync();
            }

            base.Dispose(disposing);
        }
    }

    public class RunWorkerCompletedEventArgsWithUserState
    {
        public RunWorkerCompletedEventArgsWithUserState(object result, Exception error, bool cancelled, object userState)
        {
            _result = result;
            _error = error;
            _cancelled = cancelled;
            _userState = userState;
        }

        object _result;
        Exception _error;
        bool _cancelled;
        object _userState;

        public object Result { get { return _result; } }
        public Exception Error { get { return _error; } }
        public bool Cancelled { get { return _cancelled; } }
        public object UserState { get { return _userState; } }
    }
}
#endif
