using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PlayerHeroVerification : ChildVerification
    {
        private readonly string _heroName;
        private readonly PlayerHeroCommandVerification _commands;
        private string _name;
        private int? _grace;
        private int _defaultGrace;
        private int? _secrecy;
        private int _defaultSecrecy;
        private string _location;
        private int? _lostGrace;

        public PlayerHeroVerification(IVerifiable parent, string heroName) : base(parent)
        {
            _heroName = heroName;
            _commands = new PlayerHeroCommandVerification(this);
        }

        public PlayerHeroCommandVerification Commands => _commands;

        public override void Verify(ITestRoot root)
        {
            var view = root.Get<FakePlayer>().Heroes.Single(x=>x.Name == _heroName);
            var hero = root.Get<Game>().GetHero(_heroName);
            root.Set(view);
            root.Set(hero);
            SetExpected(hero);
            view.Name.ShouldBe(hero.Name);
            view.Status.Grace.Value.ShouldBe(_grace.Value);
            view.Status.Grace.Default.ShouldBe(hero.DefaultGrace);
            view.Status.Secrecy.Value.ShouldBe(_secrecy.Value);
            view.Status.Secrecy.Default.ShouldBe(hero.DefaultSecrecy);
            view.Status.Location.ShouldBe(hero.Location.ToString());
            _commands.Verify(root);
        }

        private void SetExpected(Hero hero)
        {
            _grace = _grace ?? (hero.DefaultGrace - _lostGrace ?? hero.Grace);
            _secrecy = _secrecy ?? hero.Secrecy;
        }

        public PlayerHeroVerification Grace(int expected)
        {
            _grace = expected;
            return this;
        }

        public PlayerHeroVerification Secrecy(int expected)
        {
            _secrecy = expected;
            return this;
        }

        public PlayerHeroVerification LostGrace(int expected)
        {
            _lostGrace = expected;
            return this;
        }
    }
}