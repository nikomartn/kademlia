using System;
using System.Threading.Tasks;

namespace TaskExtensionsLib
{
    /// <summary>
    ///  John Thiriet's Extension : https://github.com/brminnick/AsyncAwaitBestPractices
    ///  It allows to safely run Fire and Forguet Tasks and handling the exceptions. 
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Safely execute the ValueTask without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="task">ValueTask.</param>
        /// <param name="onException">If an exception is thrown in the ValueTask, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public static async void SafeFireAndForget(this Task task,
                                                   bool continueOnCapturedContext = true,
                                                   Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (Exception e) when (onException != null)
            {
                onException(e);
            }
        }
    }
}
