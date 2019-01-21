using Microsoft.AspNet.SignalR.Client;

namespace Assets.Scenes.Scripts.Signals
{
    public class ConnectedSignal
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

        public ConnectedSignal(ConnectionState oldState, ConnectionState newState)
        {
            this.OldState = oldState;
            this.NewState = newState;
        }

        #endregion
    }
}
