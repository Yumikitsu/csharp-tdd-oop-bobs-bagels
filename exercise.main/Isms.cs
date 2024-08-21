using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main
{
    public interface Isms
    {
        public Task<string> Send(Basket basket);
    }
}
