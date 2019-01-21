using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Types
{
    public class UcSpaceSession : UcSession
    {
        #region Fields

        /// <summary>
        /// The digital wall in this space session.
        /// </summary>
        private UcDigitalWall _digitalWall;

        #endregion

        #region Properties

        /// <summary>
        /// The digital wall in this space session
        /// </summary>
        public UcDigitalWall DigitalWall { get; set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// The constructor. Takes in all parameters individually.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="digitalWall"></param>
        public UcSpaceSession(string name, UcDigitalWall digitalWall, int sessionId = 0) : base(name, sessionId)
        {
            this.DigitalWall = digitalWall;
        }

        /// <summary>
        /// The constructor. Takes in parent object, but all child properties are taken in individually/seperately. 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="digitalWall"></param>
        public UcSpaceSession(UcSession session, UcDigitalWall digitalWall, int sessionId = 0) : base(session.Name, session.SessionId)
        {
            this.DigitalWall = digitalWall;
        }

        #endregion
    }
}
