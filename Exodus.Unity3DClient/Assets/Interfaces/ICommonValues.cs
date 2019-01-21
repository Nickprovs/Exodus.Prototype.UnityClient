using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Interfaces
{
    public interface ICommonValues
    {
        Vector3 WallCenter { get; set; }

        float PixelToUnityUnitScale { get; set; }
    }
}
