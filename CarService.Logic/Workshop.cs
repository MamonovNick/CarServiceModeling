using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    abstract class Workshop
    {
        public abstract int getWorkTime();
    }

    class TechInspection : Workshop
    {
        public override int getWorkTime() { return 0; }
    }
}
