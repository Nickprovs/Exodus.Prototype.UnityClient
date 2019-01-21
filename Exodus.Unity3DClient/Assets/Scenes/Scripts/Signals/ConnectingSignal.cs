using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scenes.Scripts.Signals
{
    public class ConnectingSignal
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

        public ConnectingSignal(ConnectionState oldState, ConnectionState newState)
        {
            this.OldState = oldState;
            this.NewState = newState;
        }

        #endregion
    }
}
