using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Interfaces
{
    public interface IGameManager
    {
        void GameManagerStateChanged();

        void LoadMainScene();
    }
}
