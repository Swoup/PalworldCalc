using NUnit.Framework;

namespace PalworldCalculator
{
    public class TypingTests
    {
        [Test]
        public void Should_find_monotype()
        {
            Typing typing = new(PalTypes.Fire, PalTypes.None);
            Assert.That(typing.Type, Is.EqualTo(PalTypes.Fire));
        }

         [Test]
        public void Should_find_dualtype()
        {
            Typing typing = new(PalTypes.Fire, PalTypes.Dragon);
            Assert.That(typing.Type, Is.EqualTo((PalTypes)20));
        }

        [Test]
        public void Should_find_weakness_monotype()
        {
            Typing typing = new(PalTypes.Normal, PalTypes.None);
            Assert.That(typing.Weaknesses, Is.EqualTo(PalTypes.Dark));
        }

        [Test]
        public void Should_find_weaknesses_dualtype()
        {
            Typing typing = new(PalTypes.Normal, PalTypes.Water);
            Assert.That(typing.Weaknesses, Is.EqualTo((PalTypes)66));
        }

        [Test]
        public void Should_cancel_out_weaknesses_dualtype()
        {
            Typing typing = new(PalTypes.Leaf, PalTypes.Water);
            Assert.That(typing.Weaknesses, Is.EqualTo(PalTypes.Electricity));

            typing = new(PalTypes.Ice, PalTypes.Dragon);
            Assert.That(typing.Weaknesses, Is.EqualTo((PalTypes)24));
        }

        [Test]
        public void Should_find_effectivness_monotype()
        {
            Typing typing = new(PalTypes.Water, PalTypes.None);
            Assert.That(typing.Strengths, Is.EqualTo(PalTypes.Fire));
        }

         [Test]
        public void Should_find_normal_dark_is_effective_to_only_one_type()
        {
            Typing typing = new(PalTypes.Normal, PalTypes.Dark);
            Assert.That(typing.Strengths, Is.EqualTo(PalTypes.Normal));
        }

        [Test]
        public void Should_find_effectivness_Fire()
        {
            Typing typing = new(PalTypes.Fire, PalTypes.None);
            Assert.That(typing.Strengths, Is.EqualTo((PalTypes)264));
        }

        [Test]
        public void Should_find_effectivness_dual_type()
        {
            Typing typing = new(PalTypes.Fire, PalTypes.Dragon);
            Assert.That(typing.Strengths, Is.EqualTo((PalTypes)266));

            typing = new(PalTypes.Ice, PalTypes.Dragon);
            Assert.That(typing.Strengths, Is.EqualTo((PalTypes)6));
        }

        [Test]
        public void Should_find_team_offensive_coverage()
        {
            List<Typing> team = new()
            {
                new Typing(PalTypes.Normal, PalTypes.None),
                new Typing(PalTypes.Ice, PalTypes.None),
                new Typing(PalTypes.Electricity, PalTypes.None),
                new Typing(PalTypes.Dragon, PalTypes.Electricity),
                new Typing(PalTypes.Earth, PalTypes.None)
            };


            Assert.That(team.GetCoverage(x => x.Strengths), Is.EqualTo((PalTypes)102));
        }
         [Test]
        public void Should_find_team_defensive_coverage()
        {
            List<Typing> team = new()
            {
                new Typing(PalTypes.Normal, PalTypes.None),
                new Typing(PalTypes.Ice, PalTypes.None),
                new Typing(PalTypes.Electricity, PalTypes.None),
                new Typing(PalTypes.Dragon, PalTypes.Electricity),
                new Typing(PalTypes.Leaf, PalTypes.Water)
            };
            Assert.That(team.GetCoverage(x => x.Weaknesses), Is.EqualTo((PalTypes)218));
        }
    }
}