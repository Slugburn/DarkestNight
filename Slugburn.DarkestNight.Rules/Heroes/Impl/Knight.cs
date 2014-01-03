using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Knight : Hero
    {
        public Knight()
        {
            Name = "Knight";
            DefaultGrace = 5;
            DefaultSecrecy = 6;
            Powers=new IPower[]
                   {
                       
                   };
        }
    }
}
