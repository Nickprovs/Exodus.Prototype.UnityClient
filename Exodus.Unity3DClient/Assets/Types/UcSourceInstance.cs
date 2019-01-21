using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Types
{
    //TODO: Implement INotifyPropertyChanged in all types. So when color property in ucsource changes... we can do something with the game object in here.
    public class UcSourceInstance : UcSource
    {
        #region Fields

        private ExodusSourceInstanceGameObject _exodusSourceInstanceGameObject;

        private int _sourceInstanceId;

        private int _wallId;

        private int _x;

        private int _y;

        private int _width;

        private int _height;

        private float _pixelToUnityUnitScale;

        #endregion

        #region Properties

        public int SourceInstanceId
        {
            get{ return this._sourceInstanceId; }
            set
            {
                if (this._sourceInstanceId == value)
                    return;

                this._sourceInstanceId = value;
                this.OnPropertyChanged();
            }
        }

        public int WallId
        {
            get { return this._wallId; }
            set
            {
                if(this._wallId == value)
                    return;

                this._wallId = value;
                this.OnPropertyChanged();
            }
        }

        public int X
        {
            get { return this._x; }
            set
            {
                if (this._x == value)
                    return;

                this._x = value;
                this.OnPropertyChanged();
            }
        }

        public int Y
        {
            get { return this._y; }
            set
            {
                if (this._y == value)
                    return;

                this._y = value;
                this.OnPropertyChanged();
            }
        }

        public int Width
        {
            get { return this._width; }
            set
            {
                if (this._width == value)
                    return;

                this._width = value;
                this.OnPropertyChanged();
            }
        }

        public int Height
        {
            get { return this._height; }
            set
            {
                if (this._height == value)
                    return;

                this._height = value;
                this.OnPropertyChanged();
            }
        }

        public float PixelToUnityUnitScale
        {
            get { return this._pixelToUnityUnitScale; }
            set
            {
                if (this._pixelToUnityUnitScale == value)
                    return;

                this._pixelToUnityUnitScale = value;
                this.OnPropertyChanged();
            }
        }

        public ExodusSourceInstanceGameObject ExodusSourceInstanceGameObject
        {
            get { return this._exodusSourceInstanceGameObject; }
            set
            {
                if (this._exodusSourceInstanceGameObject == value)
                    return;

                this._exodusSourceInstanceGameObject = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors and Destructors

        public UcSourceInstance(int sourceId, int wallId, string name, int defaultWidth, int defaultHeight, string hexColor, int x, int y, int width, int height, float pixelToUnityUnitScale, IUnityMainThreadDispatcher dispatcher, int sourceInstanceId = 0)
            : base(name, defaultWidth, defaultHeight, hexColor, sourceId)
        {

            this.ExodusSourceInstanceGameObject = new ExodusSourceInstanceGameObject(dispatcher,this.Color);
            this.PixelToUnityUnitScale = pixelToUnityUnitScale;

            this.SourceInstanceId = sourceInstanceId;
            this.WallId = wallId;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public UcSourceInstance(UcSource source, int wallId, int x, int y, int width, int height, float pixelToUnityUnitScale, IUnityMainThreadDispatcher dispatcher, int sourceInstanceId = 0)
            : base(source.Name, source.DefaultWidth, source.DefaultHeight, source.Color, source.SourceId)
        {
            this.ExodusSourceInstanceGameObject = new ExodusSourceInstanceGameObject(dispatcher, this.Color);

            this.SourceInstanceId = sourceInstanceId;
            this.WallId = wallId;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.PixelToUnityUnitScale = pixelToUnityUnitScale;
        }
        #endregion

    }
}
