using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scenes.Scripts.Signals
{
    public class ReconnectingSignal
    {
        #region Properties

        /// <summary>
        /// The old state
        /// </summary>
        public ConnectionState OldState { get; set; }

        /// <summary>
        /// The new state
        /// </summary>
        public ConnectionState NewState { get; set; }

        #endregion

        #region Constructors and Destructors

        public ReconnectingSignal(ConnectionState oldState, ConnectionState newState)
        {
            this.OldState = oldState;
            this.NewState = newState;
        }

        #endregion
    }
}
