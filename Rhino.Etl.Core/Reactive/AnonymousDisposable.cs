using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhino.Etl.Core.Reactive
{
    class AnonymousDisposable : IDisposable
    {
        Action dispose;

        public AnonymousDisposable(Action dispose)
        {
            this.dispose = dispose;
        }

        public void Dispose()
        {
            dispose();
        }
    } 
}
