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
    public class UcPhysicalWall: UcWall
    {
        /// <summary>
        /// The constructor. Takes in all properties indivudually.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="sourceInstances"></param>
        public UcPhysicalWall(string name, int width, int height, ObservableCollection<UcSourceInstance> sourceInstances, float pixelToUnityUnitScale, Vector3 wallCenterLocation, IUnityMainThreadDispatcher dispatcher, int wallId = 0)
            : base(name, width, height, sourceInstances, pixelToUnityUnitScale, wallCenterLocation, dispatcher, wallId) { }

        /// <summary>
        /// The constructor. Takes in base. The other child properties are takenindependently.
        /// </summary>
        /// <param name="wall"></param>
        public UcPhysicalWall(UcWall wall, IUnityMainThreadDispatcher dispatcher) : base(wall.Name, wall.Width, wall.Height, wall.SourceInstances, wall.PixelToUnityUnitScale, wall.WallCenterLocation, dispatcher, wall.WallId) { }
    }
}
