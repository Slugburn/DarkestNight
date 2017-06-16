using Ninject;
using Slugburn.DarkestNight.Rules;

namespace Slugburn.DarkestNight
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = Bootstrapper.Configure();
            var gameFactory = kernel.Get<GameFactory>();
            var game = gameFactory.Create();
        }
    }
}
