using Assets.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Interfaces
{
    public interface IUcProfile
    {
        ObservableCollection<UcSource> Sources { get; set; }

        ObservableCollection<UcDigitalWall> DigitalWalls { get; set; }

        ObservableCollection<UcSpaceSession> SpaceSessions { get; set; }

        UcWall SelectedWall { get; set;}
    }
}
