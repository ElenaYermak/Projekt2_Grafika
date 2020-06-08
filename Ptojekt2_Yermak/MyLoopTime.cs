using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptojekt2_Yermak
{
    class MyLoopTime
    {
        private _3D _3D;

        public bool run { get; private set; }

        public void Load(_3D _3D)
        {
            this._3D = _3D;
        }

        public async void Run()
        {
            run = true;
            DateTime dateTime = DateTime.Now;
            while (run)
            {
                TimeSpan timeSpan = DateTime.Now - dateTime;
                dateTime = dateTime + timeSpan;
                _3D.Uzytkownik(timeSpan);
                await Task.Delay(8);
            }
        }

        public void Stop()
        {
            run = false;
        }
    }
}
