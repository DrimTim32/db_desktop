using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DesktopApplication.Remote
{
    using MahApps.Metro.Controls;
    public class TileWithId : Tile
    {
        private int Id { get; set; }
        public TileWithId(int id)
        {
            Id = id;
        }
    }
}
