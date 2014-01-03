using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Druid : Hero
    {
        public Druid()
        {
            Name = "Druid";
            DefaultGrace = 5;
            DefaultSecrecy = 6;
            Powers=new IPower[]
                   {
                       
                   };
        }
    }
}