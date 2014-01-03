using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Seer : Hero
    {
        public Seer()
        {
            Name = "Seer";
            DefaultGrace = 4;
            DefaultSecrecy = 6;
            Powers=new IPower[]
                   {
                       
                   };
        }
    }
}