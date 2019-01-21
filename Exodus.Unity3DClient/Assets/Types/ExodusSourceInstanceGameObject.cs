using Assets.Interfaces;
using UnityEngine;

namespace Assets.Types
{
    public class ExodusSourceInstanceGameObject : ExodusGameObject
    {
        #region Constructors and Destructors

        public ExodusSourceInstanceGameObject(IUnityMainThreadDispatcher dispatcher, string hexColor, PrimitiveType primitiveType = PrimitiveType.Cube) : base(hexColor, dispatcher, primitiveType) {}

        public ExodusSourceInstanceGameObject(IUnityMainThreadDispatcher dispatcher, Color color, PrimitiveType primitiveType = PrimitiveType.Cube) : base(color, dispatcher, primitiveType) { }

        #endregion

        #region Methods

        public override void Render()
        {
            base.Render();

            this.UnityMainThreadDispatcher.Enqueue(() =>
            {
                //Remove physics
                var rigidBody = this.GameObject.AddComponent<Rigidbody>();
                rigidBody.useGravity = false;
                rigidBody.detectCollisions = false;
            });
        }

        #endregion
    }
}
