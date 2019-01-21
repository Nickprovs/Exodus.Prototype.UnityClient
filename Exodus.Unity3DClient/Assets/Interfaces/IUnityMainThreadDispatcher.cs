using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Interfaces
{
    public interface IUnityMainThreadDispatcher 
    {
        void Update();

        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        void Enqueue(IEnumerator action);


        /// <summary>
        /// Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        void Enqueue(Action action);
    }
}
