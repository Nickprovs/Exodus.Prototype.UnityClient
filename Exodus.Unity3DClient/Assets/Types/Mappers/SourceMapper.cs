using Exodus.Common.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Types.Mappers
{
    public static class SourceMapper
    {
        public static UcSource GetUcSourceFromDtoSource(SourceDto dtoSource)
        {
            return new UcSource(dtoSource.Name, dtoSource.DefaultWidth, dtoSource.DefaultHeight, dtoSource.HexColor, dtoSource.Id);
        }

        public static SourceDto GetDtoSourceFromUcSource(UcSource UcSource)
        {
            return new SourceDto
            {
                Id = UcSource.SourceId,
                Name = UcSource.Name,
                HexColor = UcSource.Color.ToString(),
                DefaultWidth = UcSource.DefaultWidth,
                DefaultHeight = UcSource.DefaultHeight
            };
        }

        public static ObservableCollection<UcSource> GetUcSourceListFromDtoSourceList(IEnumerable<SourceDto> dtoSourceList)
        {

            ObservableCollection<UcSource> UcSourceList = new ObservableCollection<UcSource>();
            foreach (var dtoSource in dtoSourceList)
                UcSourceList.Add(GetUcSourceFromDtoSource(dtoSource));

            return UcSourceList;
        }

        public static List<SourceDto> GetDtoSourceListFromUcSourceList(IEnumerable<UcSource> UcSourceList)
        {

            List<SourceDto> dtoSourceList = new List<SourceDto>();
            foreach (var UcSource in UcSourceList)
                dtoSourceList.Add(GetDtoSourceFromUcSource(UcSource));

            return dtoSourceList;
        }
    }
}
