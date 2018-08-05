using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions.Handler
{
  public interface IHandlerWithHost<THost>
  {
    THost Host { get; set; }
  }

}
