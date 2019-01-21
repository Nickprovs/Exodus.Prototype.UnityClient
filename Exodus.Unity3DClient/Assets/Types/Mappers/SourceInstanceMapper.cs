using Assets.Interfaces;
using Exodus.Common.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Types.Mappers
{
    public static class SourceInstanceMapper
    {
        public static UcSourceInstance GetUcSourceInstanceFromUcSourceAndDtoSourceInstance(UcSource appropriateSource, SourceInstanceDto dtoSourceInstance, float pixelToUnityUnitScale, IUnityMainThreadDispatcher dispatcher)
        {
            return new UcSourceInstance(appropriateSource, dtoSourceInstance.WallId, dtoSourceInstance.X, dtoSourceInstance.Y, dtoSourceInstance.Width, dtoSourceInstance.Height, pixelToUnityUnitScale, dispatcher, dtoSourceInstance.Id);
        }

        public static SourceInstanceDto GetDtoSourceInstanceFromUcSourceInstance(UcSourceInstance UcSourceInstance)
        {
            return new SourceInstanceDto
            {
                Id = UcSourceInstance.SourceInstanceId,
                SourceId = UcSourceInstance.SourceId,
                WallId = UcSourceInstance.WallId,
                X = UcSourceInstance.X,
                Y = UcSourceInstance.Y,
                Width = UcSourceInstance.Width,
                Height = UcSourceInstance.Height
            };
        }

        public static ObservableCollection<UcSourceInstance> GetUcSourceInstanceListFromUcSourceListAndDtoSourceInstanceList(IEnumerable<UcSource> UcSourceList, IEnumerable<SourceInstanceDto> dtoSourceInstanceList, float pixelToUnityUnitScale, IUnityMainThreadDispatcher dispatcher)
        {

            ObservableCollection<UcSourceInstance> UcSourceInstanceList = new ObservableCollection<UcSourceInstance>();
            foreach (var dtoSourceInstance in dtoSourceInstanceList)
            {
                UcSource appropriateSource = UcSourceList.FirstOrDefault(s => s.SourceId == dtoSourceInstance.SourceId);
                if (appropriateSource == null)
                    continue;
                UcSourceInstanceList.Add(GetUcSourceInstanceFromUcSourceAndDtoSourceInstance(appropriateSource, dtoSourceInstance, pixelToUnityUnitScale, dispatcher));
            }

            return UcSourceInstanceList;
        }

        public static List<SourceInstanceDto> GetDtoSourceInstanceListFromUcSourceInstanceList(ICollection<UcSourceInstance> UcSourceInstanceList)
        {

            List<SourceInstanceDto> dtoSourceInstanceList = new List<SourceInstanceDto>();
            foreach (var UcSourceInstance in UcSourceInstanceList)
                dtoSourceInstanceList.Add(GetDtoSourceInstanceFromUcSourceInstance(UcSourceInstance));

            return dtoSourceInstanceList;
        }

    }
}
