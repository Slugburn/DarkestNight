using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Scholar : Hero
    {
        public Scholar()
        {
            Name = "Scholar";
            DefaultGrace = 4;
            DefaultSecrecy = 6;
            Powers=new IPower[]
                   {
                       
                   };
        }
    }
}