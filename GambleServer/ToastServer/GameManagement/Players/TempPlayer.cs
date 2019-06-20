using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToastServer.GameManagement.Players
{
    class TempPlayer
    {
        public bool isPlaying = false;

        public TempPlayer(bool _isPlaying)
        {
            isPlaying = _isPlaying;
        }
    }
}
