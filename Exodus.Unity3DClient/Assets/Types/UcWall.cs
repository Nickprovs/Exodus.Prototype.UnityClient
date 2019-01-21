using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Types
{
    public class UcWall : UcBase
    {
        #region Fields

        /// <summary>
        /// The wall gameobject.
        /// </summary>
        private ExodusWallGameObject _exodusWallGameObject;

        /// <summary>
        /// The wall id
        /// </summary>
        private int _wallId;

        /// <summary>
        /// The wall name
        /// </summary>
        private string _name;

        /// <summary>
        /// The wall width
        /// </summary>
        private int _width;

        /// <summary>
        /// The wall height
        /// </summary>
        private int _height;

        /// <summary>
        /// The collection of source instances for this wall.
        /// </summary>
        private ObservableCollection<UcSourceInstance> _sourceInstances;

        /// <summary>
        /// The pixel to unity unit scale.
        /// </summary>
        private float _pixelToUnityUnitScale;

        /// <summary>
        /// The wall center location
        /// </summary>
        private Vector3 _wallCenterLocation;

        #endregion

        #region Properties

        public int WallId
        {
            get { return this._wallId; }
            set
            {
                if (this._wallId == value)
                    return;

                this._wallId = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The wall name
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set
            {
                if (this._name == value)
                    return;

                this._name = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The wall width
        /// </summary>
        public int Width
        {
            get { return this._width; }
            set
            {
                if (this._width == value)
                    return;

                this._width = value;

                this.ExodusWallGameObject.Width = this._width * this.PixelToUnityUnitScale;

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The wall height
        /// </summary>
        public int Height
        {
            get { return this._height; }
            set
            {
                if (this._height == value)
                    return;

                this._height = value;

                this.ExodusWallGameObject.Height = this._height * this.PixelToUnityUnitScale;

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The collection of source instances for this wall.
        /// </summary>
        public ObservableCollection<UcSourceInstance> SourceInstances
        {
            get { return this._sourceInstances; }
            set
            {
                if (this._sourceInstances == value)
                    return;

                //If we've got a different set of source instances and the previous set was not null, unsubscribe from the old listeners
                if(this._sourceInstances != null)
                {
                    foreach (var sourceInstance in this._sourceInstances)
                    {
                        sourceInstance.PropertyChanged -= this.SourceInstance_PropertyChanged;
                    }
                }         

                this._sourceInstances = value;

                //Subscribe to the new source instances
                if (this._sourceInstances != null)
                {
                    foreach (var sourceInstance in this._sourceInstances)
                    {
                        this.CalculateInitialSourceInstancePositionGameObjectValues(sourceInstance);
                    }

                    this._sourceInstances.CollectionChanged += _sourceInstances_CollectionChanged;
                }

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

        public ExodusWallGameObject ExodusWallGameObject
        {
            get { return this._exodusWallGameObject; }
            set
            {
                if (this._exodusWallGameObject == value)
                    return;

                this._exodusWallGameObject = value;

                this.OnPropertyChanged();
            }
        }

        public Vector3 WallCenterLocation
        {
            get { return this._wallCenterLocation; }
            set
            {
                if (this._wallCenterLocation == value)
                    return;

                this._wallCenterLocation = value;

                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// The wall constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="sourceInstances"></param>
        public UcWall(string name, int width, int height, ObservableCollection<UcSourceInstance> sourceInstances, float pixelToUnityUnitScale, Vector3 wallCenterLocation, IUnityMainThreadDispatcher unityMainThreadDispatcher, int wallId = 0)
        {
            this.PixelToUnityUnitScale = pixelToUnityUnitScale;
            this.WallCenterLocation = wallCenterLocation;
            this.ExodusWallGameObject = new ExodusWallGameObject(unityMainThreadDispatcher);
           

            this.WallId = wallId;
            this.Name = name;
            this.Width = width;
            this.Height = height;

            this.InitializeWallGameObject();
            this.SourceInstances = sourceInstances;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Instantiates the game object and sets default values that don't change. 
        /// </summary>
        public void InitializeWallGameObject()
        {
            this.ExodusWallGameObject.Depth = 1;
            this.ExodusWallGameObject.X = this._wallCenterLocation.x ;
            this.ExodusWallGameObject.Y = ((this.Height * this.PixelToUnityUnitScale) / 2) + this.WallCenterLocation.y;
            this.ExodusWallGameObject.Z = this.WallCenterLocation.z;
        }

        public void Render()
        {
            this.ExodusWallGameObject.Render();
            foreach(UcSourceInstance srcInstance in this.SourceInstances)
            {
                srcInstance.ExodusSourceInstanceGameObject.Render();
            }
        }

        public void Unrender()
        {
            this.ExodusWallGameObject.Unrender();
            foreach (UcSourceInstance srcInstance in this.SourceInstances)
            {
                srcInstance.ExodusSourceInstanceGameObject.Unrender();
            }
        }

        private void CalculateInitialSourceInstancePositionGameObjectValues(UcSourceInstance sourceInstance)
        {
            sourceInstance.PropertyChanged += this.SourceInstance_PropertyChanged;

            sourceInstance.ExodusSourceInstanceGameObject.Width = sourceInstance.Width * this._pixelToUnityUnitScale;
            sourceInstance.ExodusSourceInstanceGameObject.Height = sourceInstance.Height * this._pixelToUnityUnitScale;
            sourceInstance.ExodusSourceInstanceGameObject.Depth = 0.5f;

            sourceInstance.ExodusSourceInstanceGameObject.X = (sourceInstance.X * this._pixelToUnityUnitScale) + (sourceInstance.ExodusSourceInstanceGameObject.Width / 2) + (this.ExodusWallGameObject.X - this.ExodusWallGameObject.Width / 2);
            sourceInstance.ExodusSourceInstanceGameObject.Y = (this.ExodusWallGameObject.Y * 2) - (sourceInstance.ExodusSourceInstanceGameObject.Height / 2) - (sourceInstance.Y * this._pixelToUnityUnitScale);
            sourceInstance.ExodusSourceInstanceGameObject.Z = 29.5f;
        }

        private void SourceInstance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UcSourceInstance sourceInstance = sender as UcSourceInstance;
            if (sourceInstance == null)
                return;

            switch (e.PropertyName.ToLowerInvariant())
            {
                case "x":
                    sourceInstance.ExodusSourceInstanceGameObject.X = (sourceInstance.X * this._pixelToUnityUnitScale) + (sourceInstance.ExodusSourceInstanceGameObject.Width / 2) + (this.ExodusWallGameObject.X - this.ExodusWallGameObject.Width / 2);
                    break;
                case "y":
                    sourceInstance.ExodusSourceInstanceGameObject.Y = (this.ExodusWallGameObject.Y * 2) - (sourceInstance.ExodusSourceInstanceGameObject.Height / 2) - (sourceInstance.Y * this._pixelToUnityUnitScale);
                    break;
                case "z":
                    sourceInstance.ExodusSourceInstanceGameObject.Z = 29f;
                    break;
                case "width":
                    sourceInstance.ExodusSourceInstanceGameObject.Width = sourceInstance.Width * this._pixelToUnityUnitScale;
                    sourceInstance.ExodusSourceInstanceGameObject.X = (sourceInstance.X * this._pixelToUnityUnitScale) + (sourceInstance.ExodusSourceInstanceGameObject.Width / 2) + (this.ExodusWallGameObject.X - this.ExodusWallGameObject.Width / 2);
                    break;
                case "height":
                    sourceInstance.ExodusSourceInstanceGameObject.Height = sourceInstance.Height * this._pixelToUnityUnitScale;
                    sourceInstance.ExodusSourceInstanceGameObject.Y = (this.ExodusWallGameObject.Y * 2) - (sourceInstance.ExodusSourceInstanceGameObject.Height / 2) - (sourceInstance.Y * this._pixelToUnityUnitScale);
                    break;
                case "depth":
                    sourceInstance.ExodusSourceInstanceGameObject.Depth = 0.2f;
                    break;
            }
        }

        private void _sourceInstances_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems != null)
            {
                foreach(UcSourceInstance oldSourceInstance in e.OldItems)
                {
                    oldSourceInstance.PropertyChanged -= this.SourceInstance_PropertyChanged;
                    oldSourceInstance.ExodusSourceInstanceGameObject.Unrender();
                }
            }

            if (e.NewItems != null)
            {
                foreach (UcSourceInstance newSourceInstance in e.NewItems)
                {
                    newSourceInstance.ExodusSourceInstanceGameObject.Render();
                    newSourceInstance.PropertyChanged += this.SourceInstance_PropertyChanged;
                    this.CalculateInitialSourceInstancePositionGameObjectValues(newSourceInstance);
                }
            }
        }

        #endregion

    }
}
