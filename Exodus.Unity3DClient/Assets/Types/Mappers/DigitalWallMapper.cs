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
    public static class DigitalWallMapper
    {
        public static UcDigitalWall GetUcDigitalWallFromDtoDigitalWallAndSourceInstances(DigitalWallDto dtoDigitalWall, ObservableCollection<UcSourceInstance> sourceInstances, float pixelToUnityUnitScale, Vector3 wallCenterLocation, IUnityMainThreadDispatcher dispatcher)
        {
            return new UcDigitalWall(dtoDigitalWall.Name, dtoDigitalWall.Width, dtoDigitalWall.Height, sourceInstances, pixelToUnityUnitScale, wallCenterLocation, dispatcher, dtoDigitalWall.Id);
        }

        public static DigitalWallDto GetDtoDigitalWallFromUcDigitalWall(UcDigitalWall UcDigitalWall)
        {
            return new DigitalWallDto
            {
                Id = UcDigitalWall.WallId,
                Name = UcDigitalWall.Name,
                Width = UcDigitalWall.Width,
                Height = UcDigitalWall.Height
            };
        }

        public static ObservableCollection<UcDigitalWall> GetUcDigitalWallListFromDtoDigitalWallList(IEnumerable<UcSourceInstance> sourceInstances, IEnumerable<DigitalWallDto> dtoDigitalWallList, float pixelToUnityUnitScale, Vector3 wallCenterLocation, IUnityMainThreadDispatcher dispatcher)
        {

            ObservableCollection<UcDigitalWall> UcDigitalWallList = new ObservableCollection<UcDigitalWall>();
            foreach (var dtoDigitalWall in dtoDigitalWallList)
            {
                var matchingSourceInstanceList = sourceInstances.Where(s => s.WallId == dtoDigitalWall.Id);
                var matchingSourceInstanceOc = new ObservableCollection<UcSourceInstance>(matchingSourceInstanceList);
                UcDigitalWallList.Add(DigitalWallMapper.GetUcDigitalWallFromDtoDigitalWallAndSourceInstances(dtoDigitalWall, matchingSourceInstanceOc, pixelToUnityUnitScale, wallCenterLocation, dispatcher));
            }

            return UcDigitalWallList;
        }

        public static List<DigitalWallDto> GetDtoDigitalWallListFromUcDigitalWallList(IEnumerable<UcDigitalWall> UcDigitalWallList)
        {

            List<DigitalWallDto> dtoDigitalWallList = new List<DigitalWallDto>();
            foreach (var UcDigitalWall in UcDigitalWallList)
                dtoDigitalWallList.Add(DigitalWallMapper.GetDtoDigitalWallFromUcDigitalWall(UcDigitalWall));

            return dtoDigitalWallList;
        }

    }
}
