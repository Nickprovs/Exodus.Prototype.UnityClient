using Exodus.Common.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Types.Mappers
{
    public static class SessionMapper
    {
        public static UcSession GetUcSessionFromDtoSession(SessionDto dtoSession)
        {
            return new UcSession(dtoSession.Name, dtoSession.Id);
        }

        public static SessionDto GetDtoSessionFromUcSession(UcSession UcSession)
        {
            return new SessionDto
            {
                Id = UcSession.SessionId,
                Name = UcSession.Name
            };
        }

        public static ObservableCollection<UcSession> GetUcSessionListFromDtoSessionList(IEnumerable<SessionDto> dtoSessionList)
        {

            ObservableCollection<UcSession> UcSessionList = new ObservableCollection<UcSession>();
            foreach (var dtoSession in dtoSessionList)
                UcSessionList.Add(GetUcSessionFromDtoSession(dtoSession));

            return UcSessionList;
        }

        public static List<SessionDto> GetDtoSessionListFromUcSessionList(IEnumerable<UcSession> UcSessionList)
        {

            List<SessionDto> dtoSessionList = new List<SessionDto>();
            foreach (var UcSession in UcSessionList)
                dtoSessionList.Add(GetDtoSessionFromUcSession(UcSession));

            return dtoSessionList;
        }
    }
}
