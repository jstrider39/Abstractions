using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abstractions.Handler;
using Unity.Attributes;

namespace Abstractions.CuFileIo
{

  public interface ICuFileHandler
  {
    ICuFile Create();
  }

  public interface ICuFileHandlerWithHost : IHandlerWithHost<ICuFile>, ICuFileHandler
  {

  }

  public interface ICuFile
  {
    String Dir { get; }
    String Name { get; }
    String FullPath { get; }
  }

  public class CuFile : ICuFile
  {
    [Dependency]
    public Func<ICuFileHandlerWithHost> _handlerFactory
    {
      set => getHandler = new Lazy<ICuFileHandlerWithHost>(() =>
      {
        var instance = value();
        instance.Host = this; //TODO
        return instance;
      });
    }

    private Lazy<ICuFileHandlerWithHost> getHandler;
    public ICuFileHandler Handler
    {
      get
      {
        return getHandler.Value;
      }
    }

    public override string ToString()
    {
      return FullPath;
    }

    public static char Delimiter = Path.DirectorySeparatorChar;

    //public Object Tag;
    public String Name { get; }
    public String Dir { get; }
    public String FullPath { get; }
    private Lazy<String> _contentsRead;
    private Lazy<String> _firstLineRead;

    public String Contents
    {
      get
      {
        return _contentsRead.Value;
      }
    }

    public String FirstLineNotEmpty
    {
      get
      {
        return _firstLineRead.Value;
      }

    }

    private CuFile()
    {
      //_contentsRead = new Lazy<string>(() => this.FullPath.ReadAllText());
      _firstLineRead = new Lazy<string>(() => Regex.Split(Contents.Trim(), "\r\n|\r|\n")[0]); // not really efficient for large file
    }

    public CuFile(string name, string dir, string fullPath) : this()
    {
      Name = name;
      Dir = dir;
      FullPath = fullPath;
    }

    public CuFile(string name, string dir) : this()
    {
      Name = name;
      Dir = dir;
      FullPath = Dir + Delimiter + name;
    }

    public static implicit operator CuFile(string fullPath)
    {
      var idx = fullPath.LastIndexOf(Delimiter);
      if (idx == -1) throw new Exception("Invalid file Path");
      var name = fullPath.Substring(idx + 1);
      var path = fullPath.Substring(0, idx);
      return new CuFile(name, path, fullPath);
    }

    public static implicit operator string(CuFile p)
    {
      return p.FullPath;
    }
  }
}
