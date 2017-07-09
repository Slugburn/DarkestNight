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
        private int? _defaultGrace;
        private int? _grace;
        private int? _lostGrace;
        private int? _defaultSecrecy;
        private int? _secrecy;
        private int? _lostSecrecy;
        private string _location;
        private string[] _inventory;

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
            view.Status.Grace.Value.ShouldBeIfNotNull(_grace, "Grace");
            view.Status.Grace.Default.ShouldBeIfNotNull(_defaultGrace, "Default Grace");
            view.Status.Secrecy.Value.ShouldBeIfNotNull(_secrecy, "Secrecy");
            view.Status.Secrecy.Default.ShouldBe(hero.DefaultSecrecy);
            view.Status.Location.ShouldBe(hero.Location.ToString());
            if (_inventory != null)
                view.Inventory.Select(x=>x.Name).ShouldBeEquivalent(_inventory);
            _commands.Verify(root);
        }

        private void SetExpected(Hero hero)
        {
            _defaultGrace = _defaultGrace ?? hero.DefaultGrace;
            _grace = _grace ?? (hero.DefaultGrace - _lostGrace ?? hero.Grace);
            _defaultSecrecy = _defaultSecrecy ?? hero.DefaultSecrecy;
            _secrecy = _secrecy ?? (hero.DefaultSecrecy - _lostSecrecy ?? hero.Secrecy);
        }

        public PlayerHeroVerification DefaultGrace(int expected)
        {
            _defaultGrace = expected;
            return this;
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

        public PlayerHeroVerification LostSecrecy(int expected)
        {
            _lostSecrecy = expected;
            return this;
        }

        public PlayerHeroVerification Inventory(params string[] expected)
        {
            _inventory = expected;
            return this;
        }
    }
}