using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Types
{
    public class ExodusWallGameObject : ExodusGameObject
    {
        #region Constructors and Destructors

        public ExodusWallGameObject(Interfaces.IUnityMainThreadDispatcher _dispatcher, PrimitiveType primitiveType = PrimitiveType.Cube) : base("#1e1e1e", _dispatcher, primitiveType) { }

        #endregion

        #region Methods

        public override void Render()
        {
            base.Render();

            this.UnityMainThreadDispatcher.Enqueue(() =>
            {
                //Remove Physics
                var rigidBody = this.GameObject.AddComponent<Rigidbody>();
                rigidBody.useGravity = false;
                rigidBody.detectCollisions = false;
            });
        }

        #endregion
    }
}
