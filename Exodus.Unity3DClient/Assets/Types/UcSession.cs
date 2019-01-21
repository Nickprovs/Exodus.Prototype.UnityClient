using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Types
{
    public class UcSession : UcBase
    {
        #region Fields

        /// <summary>
        /// The session id
        /// </summary>
        private int _sessionId;

        /// <summary>
        /// The name
        /// </summary>
        private string _name;

        #endregion

        #region Properties

        /// <summary>
        /// The session id
        /// </summary>
        public int SessionId { get; set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// The constructor
        /// </summary>
        public UcSession(string name, int sessionId = 0)
        {
            this.SessionId = sessionId;
            this.Name = name;
        }

        #endregion
    }
}
