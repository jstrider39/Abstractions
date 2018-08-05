using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Core;
using Abstractions.CuFileIo;
using NUnit.Framework;
using Unity;
using Unity.Injection;
using Unity.Resolution;
using ICuFile = Abstractions.ICuFile;

namespace Tests
{

  public interface ICare {
    int Age { get; set; }
  }

  public class Care: ICare
  {
    public int Age { get; set; }
  }

  [TestFixture]
  public class CuDirTest
  {
    public static Care Factory()
    {
      return new Care {Age = 34};
    }

    [Test]
    public void TestUnityFactoryMethod()
    {
      
    }

    [Test]
    public void TestFiles()
    {
      var c = new UnityContainer();
      c.RegisterType<ICuDir, CuDir>(new InjectionProperty(CuBase.ContainPropStr, c));//ICuFileHandlerWithHost
      c.RegisterType<ICuDirHandlerWithHost, CuDirOnDiskHandler>();//

      var cuDir = c.Resolve<ICuDir>(new ParameterOverride("path",@"J:\tmp\test") );
      Assert.AreEqual(@"test", cuDir.LastPart);
      Assert.AreEqual(@"TEST", cuDir.LastPartAsUpper);
      Assert.IsTrue(@"J:\tmp" == cuDir.Parent.Path);

      //c.RegisterType<ICuFileHandlerWithHost,  >();//
      //// Need builder to make Handler and put into context
      //// builder could look like my Zring config ??

      //Assert.AreEqual(1,1);
    }

  }
}
