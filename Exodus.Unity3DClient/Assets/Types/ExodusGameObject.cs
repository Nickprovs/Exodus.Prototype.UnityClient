using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Types
{
    public abstract class ExodusGameObject
    {
        #region Fields

        protected GameObject GameObject;

        protected PrimitiveType PrimitiveType = PrimitiveType.Cube;
        
        protected bool TeleportOnPropertyChange = true;

        protected bool ResizeOnPropertyChange = true;

        private float _x;

        private float _y;

        private float _z;

        private float _width;

        private float _height;

        private float _depth;

        private Color _color;

        protected IUnityMainThreadDispatcher UnityMainThreadDispatcher;

        #endregion

        #region Properties

        public float X
        {
            get { return this._x; }
            set
            {
                if (this._x == value)
                    return;

                this._x = value;

                if (this.TeleportOnPropertyChange)
                    this.Teleport(this._x, this._y, this._z);
            }
        }

        public float Y
        {
            get { return this._y; }
            set
            {
                if (this._y == value)
                    return;

                this._y = value;

                if (this.TeleportOnPropertyChange)
                    this.Teleport(this._x, this._y, this._z);
            }
        }

        public float Z
        {
            get { return this._z; }
            set
            {
                if (this._z == value)
                    return;

                this._z = value;

                if (this.TeleportOnPropertyChange)
                    this.Teleport(this._x, this._y, this._z);
            }
        }

        public float Width
        {
            get { return this._width; }
            set
            {
                if (this._width == value)
                    return;

                this._width = value;
                if (this.ResizeOnPropertyChange)
                    this.Resize(this._width, this._height, this._depth);
            }
        }

        public float Height
        {
            get { return this._height; }
            set
            {
                if (this._height == value)
                    return;

                this._height = value;

                if (this.ResizeOnPropertyChange)
                    this.Resize(this._width, this._height, this._depth);
            }
        }

        public float Depth
        {
            get { return this._depth; }
            set
            {
                if (this._depth == value)
                    return;

                this._depth = value;

                if (this.ResizeOnPropertyChange)
                    this.Resize(this._width, this._height, this._depth);
            }
        }

        public Color Color
        {
            get { return this._color; }
            set
            {
                if (this._color == value)
                    return;

                this._color = value;
                this.ChangeColor(this._color);
            }

        }

        #endregion

        #region Constructors and Destructors

        public ExodusGameObject(string color, IUnityMainThreadDispatcher dispatcher, PrimitiveType primitiveType = PrimitiveType.Cube)
        {
            this.UnityMainThreadDispatcher = dispatcher;
            this.ChangeColor(color);
            this.PrimitiveType = primitiveType;
        }

        public ExodusGameObject(Color color, IUnityMainThreadDispatcher dispatcher, PrimitiveType primitiveType = PrimitiveType.Cube)
        {
            this.UnityMainThreadDispatcher = dispatcher;
            this.Color = color;
            this.PrimitiveType = primitiveType;
        }

        #endregion

        #region Methods

        public virtual void Render()
        {
            this.UnityMainThreadDispatcher.Enqueue(() => 
            {
                //Creating the object
                this.GameObject = GameObject.CreatePrimitive(this.PrimitiveType);

                //Sizing the object
                this.GameObject.transform.localScale = new Vector3(this.Width, this.Height, this.Depth);

                //Moving the object to correct position
                this.GameObject.transform.position = new Vector3(this.X, this.Y, this.Z);

                //Set the object's color
                Renderer rend = this.GameObject.GetComponent<Renderer>();
                rend.material.SetColor("_Color", this.Color);
            });
        }

        public virtual void Unrender()
        {
            this.UnityMainThreadDispatcher.Enqueue(() =>
            {
                if (this.GameObject != null)
                {
                    UnityEngine.Object.Destroy(this.GameObject);
                }
            });

        }

        public void Teleport(float x, float y, float z)
        {
            this.UnityMainThreadDispatcher.Enqueue(() =>
            {
                if (this.GameObject == null)
                    return;

                //If any of the position parameters differ from the current position, teleport the object.
                if (this.GameObject.transform.position.x != this.X ||
                    this.GameObject.transform.position.y != this.Y ||
                    this.GameObject.transform.position.z != this.Z)
                    this.GameObject.transform.position = new Vector3(this.X, this.Y, this.Z);
            });
        }

        public void Resize(float width, float height, float depth)
        {
            this.UnityMainThreadDispatcher.Enqueue(() =>
            {
                if (this.GameObject == null)
                    return;

                //If any of the scaled size parameters differ from the current scaled size, scale the object.
                if (this.GameObject.transform.localScale.x != this.Width ||
                    this.GameObject.transform.localScale.y != this.Height ||
                    this.GameObject.transform.localScale.z != this.Depth)
                    this.GameObject.transform.localScale = new Vector3(this.Width, this.Height, this.Depth);
            });
        }

        public void ChangeColor(string hexColor)
        {
            this.UnityMainThreadDispatcher.Enqueue(() =>
            {
                Color unityColor;
                try
                {
                    //Correct the colors for unity's consumption.
                    bool success = ColorUtility.TryParseHtmlString(hexColor, out unityColor);
                    if (!success)
                    {
                        Debug.LogError("Error convertering to unity color.");
                        return;
                    }

                    //The setter will take us into the appropriate method from here to apply the color change.
                    this.Color = unityColor;
                }
                catch (Exception)
                {
                    Debug.LogError("Error convertering to unity color. Probably not dispatching to main thread.");
                }
            });
        }

        public void ChangeColor(Color color)
        {
            this.UnityMainThreadDispatcher.Enqueue(() =>
            {
                try
                {
                    if (this.GameObject != null)
                    {
                        Renderer rend = this.GameObject.GetComponent<Renderer>();
                        rend.material.SetColor("_Color", color);
                    }
                }
                catch (Exception)
                {
                    Debug.LogError("Error changing unity color. Probably not dispatching to main thread.");
                }
            });
        }
        #endregion
    }
}
