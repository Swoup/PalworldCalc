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
            Assert.That(typing.Type, Is.EqualTo(PalTypes.Fire | PalTypes.Dragon));
        }

        [Test]
        public void Should_find_weakness_monotype()
        {
            Typing typing = new(PalTypes.Normal, PalTypes.None);
            Assert.That(typing.Weaknesses, Is.EqualTo(PalTypes.Dark));
        }

        [Test]
        public void Should_find_weaknesses_unrelated_dualtype()
        {
            Typing typing = new(PalTypes.Normal, PalTypes.Water);
            Assert.That(typing.Weaknesses, Is.EqualTo(PalTypes.Dark | PalTypes.Electricity));
        }

        [Test]
        public void Should_cancel_out_weaknesses_dualtype()
        {
            Typing typing = new(PalTypes.Leaf, PalTypes.Water);
            Assert.That(typing.Weaknesses, Is.EqualTo(PalTypes.Electricity));

            typing = new(PalTypes.Ice, PalTypes.Dragon);
            Assert.That(typing.Weaknesses, Is.EqualTo(PalTypes.Fire));
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
            Assert.That(typing.Strengths, Is.EqualTo(PalTypes.Leaf | PalTypes.Ice));
        }

        [Test]
        public void Should_find_effectivness_dual_type()
        {
            Typing typing = new(PalTypes.Fire, PalTypes.Dragon);
            Assert.That(typing.Strengths, Is.EqualTo(PalTypes.Ice | PalTypes.Leaf | PalTypes.Dark));

            typing = new(PalTypes.Ice, PalTypes.Dragon);
            Assert.That(typing.Strengths, Is.EqualTo(PalTypes.Dragon | PalTypes.Dark));
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


            Assert.That(team.GetCoverage(x => x.Strengths), Is.EqualTo(PalTypes.Dragon | PalTypes.Water | PalTypes.Dark | PalTypes.Electricity));
        }
         [Test]
        public void Should_find_team_defensive_coverage()
        {
            List<Typing> team = new()
            {
                new Typing(PalTypes.Normal, PalTypes.None),
                new Typing(PalTypes.Ice, PalTypes.None),
                new Typing(PalTypes.Electricity, PalTypes.None),
                new Typing(PalTypes.Dragon, PalTypes.Ice),
                new Typing(PalTypes.Leaf, PalTypes.Water)
            };
            Assert.That(team.GetCoverage(x => x.Weaknesses), Is.EqualTo(PalTypes.Dark | PalTypes.Fire | PalTypes.Earth | PalTypes.Electricity));
        }

        [Test]
        public void Should_find_that_grass_ground_is_same_as_ground_grass()
        {
            Typing grassGround = new(PalTypes.Leaf, PalTypes.Earth);
            Typing groundGrass = new(PalTypes.Earth, PalTypes.Leaf);

            Assert.That(groundGrass, Is.EqualTo(grassGround));
        }

        [Test]
        public void Test()
        {
            List<Typing> team1 = new()
            {
                new Typing(PalTypes.Ice, PalTypes.Dark),
                new Typing(PalTypes.Dragon, PalTypes.Fire),
                new Typing(PalTypes.Electricity, PalTypes.None),
                new Typing(PalTypes.Leaf, PalTypes.Water),
                new Typing(PalTypes.Leaf, PalTypes.Earth)
            };

             List<Typing> team2 = new()
             {
                new Typing(PalTypes.Ice, PalTypes.Dark),
                new Typing(PalTypes.Dragon, PalTypes.Fire),
                new Typing(PalTypes.Electricity, PalTypes.None),
                new Typing(PalTypes.Leaf, PalTypes.Water),
                new Typing(PalTypes.Earth, PalTypes.Leaf),
            };

            var offensiveCoverage1 = team1.Aggregate(PalTypes.None, (a,b) => a | b.Strengths);
            var offensiveCoverage2 = team2.Aggregate(PalTypes.None, (a,b) => a | b.Strengths);

            var defensiveWeaknesses1 = team1.Aggregate(PalTypes.None, (a,b) => a | b.Weaknesses);
            var defensiveWeaknesses2 = team2.Aggregate(PalTypes.None, (a,b) => a | b.Weaknesses);

            Assert.That(offensiveCoverage1, Is.EqualTo(offensiveCoverage2));
            Assert.That(defensiveWeaknesses1, Is.EqualTo(defensiveWeaknesses2));
        }
    }
}