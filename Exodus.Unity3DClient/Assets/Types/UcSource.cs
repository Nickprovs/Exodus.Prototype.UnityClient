using Assets.Interfaces;
using System;
using UnityEngine;

namespace Assets.Types
{
    public class UcSource : UcBase
    {

        #region Fields
        /// <summary>
        /// The Id.
        /// </summary>
        private int _sourceId;

        /// <summary>
        /// The name
        /// </summary>
        private string _name;

        /// <summary>
        /// The default width
        /// </summary>
        private int _defaultWidth;

        /// <summary>
        /// The default height
        /// </summary>
        private int _defaultHeight;

        /// <summary>
        /// The color of the source
        /// </summary>
        private Color _color;

        #endregion

        #region Properties

        /// <summary>
        /// The Id.
        /// </summary>
        public int SourceId
        {
            get { return this._sourceId; }
            set
            {
                if (this._sourceId == value)
                    return;

                this._sourceId = value;

                this.OnPropertyChanged();
            }

        }

        /// <summary>
        /// The name
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
        /// The default width
        /// </summary>
        public int DefaultWidth
        {
            get { return this._defaultWidth; }
            set
            {
                if (this._defaultWidth == value)
                    return;

                this._defaultWidth = value;

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The default height
        /// </summary>
        public int DefaultHeight
        {
            get { return this._defaultHeight; }
            set
            {
                if (this._defaultHeight == value)
                    return;

                this._defaultHeight = value;

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The color of the source
        /// </summary>
        public Color Color
        {
            get { return this._color; }
            set
            {
                if (this._color == value)
                    return;

                this._color = value;

                this.OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// The constructor for a DC source. Takes in a name, default size, and color as a hex string.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultWidth"></param>
        /// <param name="defaultHeight"></param>
        /// <param name="hexColor"></param>
        public UcSource(string name, int defaultWidth, int defaultHeight, string hexColor, int sourceId = 0)
        {
            this.SourceId = sourceId;
            this.Name = name;
            this.DefaultWidth = defaultWidth;
            this.DefaultHeight = defaultHeight;

            Color unityColor;
            try
            {
                //Correct the colors for unity's consumption.
                bool success = ColorUtility.TryParseHtmlString(hexColor, out unityColor);
                this.Color = unityColor;
            }
            catch (Exception e)
            {
                Debug.LogError("Error convertering to unity color. Probably not dispatching to main thread.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultWidth"></param>
        /// <param name="defaultHeight"></param>
        /// <param name="color"></param>
        public UcSource(string name, int defaultWidth, int defaultHeight, Color color, int sourceId = 0)
        {
            this.SourceId = sourceId;
            this.Name = name;
            this.DefaultWidth = defaultWidth;
            this.DefaultHeight = defaultHeight;
            this.Color = color;
        }

        #endregion

    }
}
