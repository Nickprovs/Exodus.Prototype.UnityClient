using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Interfaces
{
    public interface IExodusGameObjectable
    {
        float X { get; set; }
        float Y { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        void Render();
        void Unrender();

    }
}
