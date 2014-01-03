using Ninject.Modules;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Heroes.Impl;

namespace Slugburn.DarkestNight.Ninject
{
    public class HeroModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IHero>().To<Acolyte>();
            Bind<IHero>().To<Druid>();
            Bind<IHero>().To<Knight>();
            Bind<IHero>().To<Priest>();
            Bind<IHero>().To<Prince>();
            Bind<IHero>().To<Rogue>();
            Bind<IHero>().To<Scholar>();
            Bind<IHero>().To<Seer>();
            Bind<IHero>().To<Wizard>();
        }
    }
}
