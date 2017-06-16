using Ninject.Modules;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Heroes.Impl;

namespace Slugburn.DarkestNight.Ninject
{
    public class HeroModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Hero>().To<Acolyte>();
            Bind<Hero>().To<Druid>();
            Bind<Hero>().To<Knight>();
            Bind<Hero>().To<Priest>();
            Bind<Hero>().To<Prince>();
            Bind<Hero>().To<Rogue>();
            Bind<Hero>().To<Scholar>();
            Bind<Hero>().To<Seer>();
            Bind<Hero>().To<Wizard>();
        }
    }
}
