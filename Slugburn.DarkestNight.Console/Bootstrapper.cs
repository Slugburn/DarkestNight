using Ninject;
using Slugburn.DarkestNight.Ninject;

namespace Slugburn.DarkestNight.Console
{
    public static class Bootstrapper
    {
        public static StandardKernel Kernel { get; private set; }

        public static void Configure()
        {
            Kernel = new StandardKernel();
            Kernel.Load<HeroModule>();
        }
    }
}
