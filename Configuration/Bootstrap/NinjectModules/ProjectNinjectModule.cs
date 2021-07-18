using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.Configuration.Bootstrap.NinjectModules
{
    public class ProjectNinjectModule : INinjectModule
    {
        public string Name => throw new NotImplementedException();

        public IKernel Kernel => throw new NotImplementedException();

        public void OnLoad(IKernel kernel)
        {
            throw new NotImplementedException();
        }

        public void OnUnload(IKernel kernel)
        {
            throw new NotImplementedException();
        }

        public void OnVerifyRequiredModules()
        {
            throw new NotImplementedException();
        }
    }
}