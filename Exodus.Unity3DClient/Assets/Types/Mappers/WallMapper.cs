using Assets.Interfaces;
using Exodus.Common.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Types.Mappers
{
    public static class WallMapper
    {
        //Very bad how i'm not injecting the main thread dispatcher... but this is a prototype and I'm running out of time.
        public static UcWall GetUcWallFromDtoWall(WallDto dtoWall, float pixelToUnityUnitScale, Vector3 wallCenterLocation, IUnityMainThreadDispatcher unityMainThreadDispatcher)
        {
            return new UcWall(dtoWall.Name, dtoWall.Width, dtoWall.Height, new ObservableCollection<UcSourceInstance>(), pixelToUnityUnitScale, wallCenterLocation, unityMainThreadDispatcher, dtoWall.Id);
        }

        public static WallDto GetDtoWallFromUcWall(UcWall UcWall)
        {
            return new WallDto
            {
                Id = UcWall.WallId,
                Name = UcWall.Name,
                Width = UcWall.Width,
                Height = UcWall.Height
            };
        }

        public static ObservableCollection<UcWall> GetUcWallListFromDtoWallList(IEnumerable<WallDto> dtoWallList, float pixelToUnityUnitScale,  Vector3 wallCenterLocation, IUnityMainThreadDispatcher dispatcher)
        {

            ObservableCollection<UcWall> UcWallList = new ObservableCollection<UcWall>();
            foreach (var dtoWall in dtoWallList)
                UcWallList.Add(GetUcWallFromDtoWall(dtoWall, pixelToUnityUnitScale, wallCenterLocation, dispatcher));

            return UcWallList;
        }

        public static List<WallDto> GetDtoWallListFromUcWallList(IEnumerable<UcWall> UcWallList)
        {

            List<WallDto> dtoWallList = new List<WallDto>();
            foreach (var UcWall in UcWallList)
                dtoWallList.Add(GetDtoWallFromUcWall(UcWall));

            return dtoWallList;
        }


    }
}
