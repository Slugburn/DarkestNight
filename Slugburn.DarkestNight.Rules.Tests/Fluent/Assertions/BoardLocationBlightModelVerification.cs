using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions {
    public class BoardLocationBlightModelVerification : ChildVerification
    {
        private readonly string _blightName;
        private bool? _isSupressed;
        private int? _might;

        public BoardLocationBlightModelVerification(IVerifiable parent, string blightName) 
            : base(parent)
        {
            _blightName = blightName;
        }

        public override void Verify(ITestRoot root)
        {
            var location = root.Get<LocationModel>();
            var blight = location.Blights.Single(x => x.Name == _blightName);
            blight.IsSupressed.ShouldBeIfNotNull(_isSupressed, "IsSupressed");
            blight.Might.ShouldBeIfNotNull(_might, "MIght");
        }

        public BoardLocationBlightModelVerification IsSupressed(bool expected = true)
        {
            _isSupressed = expected;
            return this;
        }

        public BoardLocationBlightModelVerification Might(int expected)
        {
            _might = expected;
            return this;
        }
    }
}