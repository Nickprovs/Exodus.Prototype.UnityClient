using Exodus.Common.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Types.Mappers
{
    public static class SpaceSessionMapper
    {
        public static UcSpaceSession GetUcSpaceSessionFromUcDigitalWallAndDtoSpaceSession(UcDigitalWall appropriateDigitalWall, SpaceSessionDto dtoSpaceSession)
        {
            return new UcSpaceSession(dtoSpaceSession.Name, appropriateDigitalWall, dtoSpaceSession.Id);
        }

        public static SpaceSessionDto GetDtoSpaceSessionFromUcSpaceSession(UcSpaceSession UcSpaceSession)
        {
            return new SpaceSessionDto
            {
                Id = UcSpaceSession.SessionId,
                Name = UcSpaceSession.Name,
                DigitalWallId = UcSpaceSession.DigitalWall.WallId
            };
        }

        public static ObservableCollection<UcSpaceSession> GetUcSpaceSessionListFromUcDigitalWallListAndDtoSpaceSessionList(IEnumerable<UcDigitalWall> UcDigitalWallList, IEnumerable<SpaceSessionDto> dtoSpaceSessionList)
        {

            ObservableCollection<UcSpaceSession> UcSpaceSessionList = new ObservableCollection<UcSpaceSession>();
            foreach (var dtoSpaceSession in dtoSpaceSessionList)
            {
                var appropriateWall = UcDigitalWallList.FirstOrDefault(d => d.WallId == dtoSpaceSession.DigitalWallId);
                if (appropriateWall == null)
                    continue;
                UcSpaceSessionList.Add(GetUcSpaceSessionFromUcDigitalWallAndDtoSpaceSession(appropriateWall, dtoSpaceSession));
            }

            return UcSpaceSessionList;
        }

        public static List<SpaceSessionDto> GetDtoSpaceSessionListFromUcSpaceSessionList(ICollection<UcSpaceSession> UcSpaceSessionList)
        {

            List<SpaceSessionDto> dtoSpaceSessionList = new List<SpaceSessionDto>();
            foreach (var UcSpaceSession in UcSpaceSessionList)
                dtoSpaceSessionList.Add(GetDtoSpaceSessionFromUcSpaceSession(UcSpaceSession));

            return dtoSpaceSessionList;
        }

    }
}
