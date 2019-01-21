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
    public static class PhysicalWallMapper
    {
        public static UcPhysicalWall GetUcPhysicalWallFromDtoPhysicalWallAndSourceInstances(PhysicalWallDto dtoPhysicalWall, ObservableCollection<UcSourceInstance> sourceInstances, float pixelToUnityUnitScale, Vector3 wallCenterLocation, IUnityMainThreadDispatcher dispatcher)
        {
            return new UcPhysicalWall(dtoPhysicalWall.Name, dtoPhysicalWall.Width, dtoPhysicalWall.Height, sourceInstances, pixelToUnityUnitScale, wallCenterLocation, dispatcher, dtoPhysicalWall.Id);
        }

        public static PhysicalWallDto GetDtoPhysicalWallFromUcPhysicalWall(UcPhysicalWall UcPhysicalWall)
        {
            return new PhysicalWallDto
            {
                Id = UcPhysicalWall.WallId,
                Name = UcPhysicalWall.Name,
                Width = UcPhysicalWall.Width,
                Height = UcPhysicalWall.Height
            };
        }

        public static ObservableCollection<UcPhysicalWall> GetUcPhysicalWallListFromDtoPhysicalWallList(IEnumerable<UcSourceInstance> sourceInstances, IEnumerable<PhysicalWallDto> dtoPhysicalWallList, float pixelToUnityUnitScale, Vector3 wallCenterLocation, IUnityMainThreadDispatcher dispatcher)
        {

            ObservableCollection<UcPhysicalWall> UcPhysicalWallList = new ObservableCollection<UcPhysicalWall>();
            foreach (var dtoPhysicalWall in dtoPhysicalWallList)
            {
                var matchingSourceInstanceList = sourceInstances.Where(s => s.WallId == dtoPhysicalWall.Id);
                var matchingSourceInstanceOc = new ObservableCollection<UcSourceInstance>(matchingSourceInstanceList);
                UcPhysicalWallList.Add(PhysicalWallMapper.GetUcPhysicalWallFromDtoPhysicalWallAndSourceInstances(dtoPhysicalWall, matchingSourceInstanceOc, pixelToUnityUnitScale, wallCenterLocation, dispatcher));
            }

            return UcPhysicalWallList;
        }

        public static List<PhysicalWallDto> GetDtoPhysicalWallListFromUcPhysicalWallList(IEnumerable<UcPhysicalWall> UcPhysicalWallList)
        {

            List<PhysicalWallDto> dtoPhysicalWallList = new List<PhysicalWallDto>();
            foreach (var UcPhysicalWall in UcPhysicalWallList)
                dtoPhysicalWallList.Add(PhysicalWallMapper.GetDtoPhysicalWallFromUcPhysicalWall(UcPhysicalWall));

            return dtoPhysicalWallList;
        }

    }
}
