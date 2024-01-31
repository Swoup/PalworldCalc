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
        public void Should_find_weaknesses_related_dualtype()
        {
            Typing typing = new(PalTypes.Leaf, PalTypes.Water);
            Assert.That(typing.Weaknesses, Is.EqualTo(PalTypes.Electricity));
        }

        [Test]
        public void Should_find_effectivness_monotype()
        {
            Typing typing = new(PalTypes.Water, PalTypes.None);
            Assert.That(typing.Strengths, Is.EqualTo(PalTypes.Fire));
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
        }
    }
}