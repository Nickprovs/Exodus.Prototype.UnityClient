using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Utilities
{
    public class CommonValues : ICommonValues
    {
        public Vector3 WallCenter { get; set; } = new Vector3(0f, 0f, 30f);

        public float PixelToUnityUnitScale { get; set; } = 0.00666f;
    }
}
