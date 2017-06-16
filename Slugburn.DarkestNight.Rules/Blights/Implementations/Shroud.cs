using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class Shroud : Blight
    {
        public Shroud() : base(BlightType.Shroud)
        {
            Name = "Shroud";
            EffectText = "Other types of blights at the location of a Shroud cannot be destroyed (the Shroud must be destroyed first.)";
            Might = 5;
            DefenseText = "Wound.";
        }

        public override void Defend(Hero hero)
        {
            hero.TakeWound();
        }
    }
}
