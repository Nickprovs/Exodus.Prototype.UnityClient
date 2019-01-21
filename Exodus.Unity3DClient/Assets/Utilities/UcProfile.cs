using Assets.Interfaces;
using Assets.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Utilities
{
    public class UcProfile : IUcProfile
    {
        public static double DefaultPixelToUnitySizeScale = 0.05;

        public UcWall SelectedWall { get; set; }

        public ObservableCollection<UcSource> Sources { get; set; }

        public ObservableCollection<UcDigitalWall> DigitalWalls { get; set; }

        public ObservableCollection<UcSpaceSession> SpaceSessions { get; set; }
    }
}
