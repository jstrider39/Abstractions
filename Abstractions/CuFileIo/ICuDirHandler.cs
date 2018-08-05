using System.Collections.Generic;
using Abstractions.Handler;

namespace Abstractions.CuFileIo
{
  // implementing IHandlerWithHost<T> allows us to get the Host as a "sort of like a" callback to the parent
  public interface ICuDirHandler
  {
    IEnumerable<ICuFile> GetFiles();
    IEnumerable<ICuFile> GetFiles(string pattern);
    void DeleteFiles(string pattern);
    void Create();
    bool HasDir();

    IEnumerable<ICuDir> GetCurrentBakDirectories();

    
    //void BackUpFiles(string pattern);
  }

  public interface ICuDirHandlerWithHost : IHandlerWithHost<ICuDir>, ICuDirHandler
  {

  }

}