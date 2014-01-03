using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Rogue : Hero
    {
        public Rogue()
        {
            Name = "Rogue";
            DefaultGrace = 4;
            DefaultSecrecy = 7;
            Powers=new IPower[]
                   {
                       
                   };
        }
    }
}