using Ninject;
using Slugburn.DarkestNight.Rules;

namespace Slugburn.DarkestNight.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Configure();
            var gameFactory = Bootstrapper.Kernel.Get<GameFactory>();
            var game = gameFactory.Create();
        }
    }
}
