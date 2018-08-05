using System;
using System.IO;
using Abstractions.Core;

namespace Abstractions.CuFileIo
{
  public interface ICuDir: ICuSys
  {
    ICuDirHandler Handler { get; } // not sure??
    String Path { get; }
    string LastPart { get; }
    string LastPartAsUpper { get; }
    ICuDir Parent { get; }
  }
}