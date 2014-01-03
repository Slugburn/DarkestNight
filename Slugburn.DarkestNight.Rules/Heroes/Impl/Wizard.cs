using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Wizard : Hero
    {
        public Wizard()
        {
            Name = "Wizard";
            DefaultGrace = 3;
            DefaultSecrecy = 5;
            Powers=new IPower[]
                   {
                       
                   };
        }
    }
}