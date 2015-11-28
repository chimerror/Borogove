using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Borogove.Model;

namespace Borogove.Tests
{
    [TestFixture]
    public class TagSetTests
    {
        [Test]
        public void CanAddAndResolveSingleBareTag()
        {
            var tagString = "Music";
            var canonicalizedTagName = Tag.CanonicalizeTagName(tagString);
            var target = new TagSet(tagString);

            var result = target.ResolveTag(tagString);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(canonicalizedTagName));

            var resultAliases = result.Aliases.ToList();
            Assert.That(resultAliases, Is.Empty);

            var resultImplications = result.Implications.ToList();
            Assert.That(resultImplications, Is.Empty);
        }

        [Test]
        public void CanAddAndResolveMultipleBareTags()
        {
            var tagString = "Wine, Women,song";
            var target = new TagSet(tagString);

            var inputStrings = new string[] { "Wine", "Women", "Song" };

            foreach (var inputString in inputStrings)
            {
                var canonicalizedTagName = Tag.CanonicalizeTagName(inputString);
                var result = target.ResolveTag(inputString);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo(canonicalizedTagName));

                var resultAliases = result.Aliases.ToList();
                Assert.That(resultAliases, Is.Empty);

                var resultImplications = result.Implications.ToList();
                Assert.That(resultImplications, Is.Empty);
            }
        }

        [Test]
        public void CanAddAndResolveSingleAliasSpecification()
        {
            var tagString = "Puma = Cougar";
            var expectedAliasName = Tag.CanonicalizeTagName("Puma");
            var expectedTagName = Tag.CanonicalizeTagName("Cougar");
            var target = new TagSet(tagString);

            var result1 = target.ResolveTag("Puma");
            Assert.That(result1, Is.Not.Null);
            Assert.That(result1.Name, Is.EqualTo(expectedTagName));

            var resultAliases1 = result1.Aliases.ToList();
            Assert.That(resultAliases1, Has.Count.EqualTo(1));
            Assert.That(resultAliases1.First(), Is.EqualTo(expectedAliasName));

            var resultImplications1 = result1.Implications.ToList();
            Assert.That(resultImplications1, Is.Empty);

            var result2 = target.ResolveTag("Cougar");
            Assert.That(result2, Is.Not.Null);
            Assert.That(result2.Name, Is.EqualTo(expectedTagName));

            var resultAliases2 = result2.Aliases.ToList();
            Assert.That(resultAliases2, Has.Count.EqualTo(1));
            Assert.That(resultAliases2.First(), Is.EqualTo(expectedAliasName));

            var resultImplications2 = result2.Implications.ToList();
            Assert.That(resultImplications2, Is.Empty);
        }

        [Test]
        public void CanAddAndResolveMultipleAliasSpecifications()
        {
            var tagString = "Puma=Cougar, Panther = Cougar, Cougar = Catamount";
            var target = new TagSet(tagString);
            var expectedTagName = Tag.CanonicalizeTagName("Cougar");
            var expectedAliasNames = new string[] { "Puma", "panther", "Catamount" }
                .Select(a => Tag.CanonicalizeTagName(a))
                .ToList();

            var resultTag = target.ResolveTag("cougar");
            Assert.That(resultTag, Is.Not.Null);
            Assert.That(resultTag.Name, Is.EqualTo(expectedTagName));

            var resultAliases = resultTag.Aliases.ToList();
            Assert.That(resultAliases, Is.EquivalentTo(expectedAliasNames));

            var resultImplications = resultTag.Implications.ToList();
            Assert.That(resultImplications, Is.Empty);

            foreach (var aliasString in expectedAliasNames)
            {
                var aliasResult = target.ResolveTag(aliasString);
                Assert.That(aliasResult, Is.Not.Null);
                Assert.That(aliasResult.Name, Is.EqualTo(expectedTagName));

                var aliasAliases = aliasResult.Aliases.ToList();
                Assert.That(aliasAliases, Is.EquivalentTo(expectedAliasNames));

                var aliasImplications = aliasResult.Implications.ToList();
                Assert.That(aliasImplications, Is.Empty);
            }
        }

        [Test]
        public void CanAddAndResolveSingleImplicationSpecification()
        {
            var tagString = "Mammal > Animal";
            var expectedTagName = Tag.CanonicalizeTagName("Mammal");
            var expectedImplicationName = Tag.CanonicalizeTagName("Animal");
            var target = new TagSet(tagString);

            var result = target.ResolveTag("Mammal");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedTagName));

            var resultAliases = result.Aliases.ToList();
            Assert.That(resultAliases, Is.Empty);

            var resultImplications = result.Implications.ToList();
            Assert.That(resultImplications, Has.Count.EqualTo(1));

            var impliedTag = resultImplications.First();
            Assert.That(impliedTag, Is.Not.Null);
            Assert.That(impliedTag.Name, Is.EqualTo(expectedImplicationName));

            var impliedAliases = impliedTag.Aliases.ToList();
            Assert.That(impliedAliases, Is.Empty);

            var impliedImplications = impliedTag.Implications.ToList();
            Assert.That(impliedImplications, Is.Empty);

            var directImpliedTag = target.ResolveTag("Animal");
            Assert.That(directImpliedTag, Is.Not.Null);
            Assert.That(directImpliedTag.Name, Is.EqualTo(expectedImplicationName));

            var directImpliedAliases = directImpliedTag.Aliases.ToList();
            Assert.That(directImpliedAliases, Is.Empty);

            var directImpliedImplications = directImpliedTag.Implications.ToList();
            Assert.That(directImpliedImplications, Is.Empty);
        }

        [Test]
        public void CanAddAndResolveMultipleImplicationSpecifications()
        {
            var tagString = "Mammal > Animal,Canine>Mammal, Feline > mammal";
            var expectedFirstTagName = Tag.CanonicalizeTagName("Canine");
            var expectedSecondTagName = Tag.CanonicalizeTagName("Feline");
            var expectedFirstImplicationName = Tag.CanonicalizeTagName("Mammal");
            var expectedSecondImplicationName = Tag.CanonicalizeTagName("Animal");
            var target = new TagSet(tagString);

            var firstResult = target.ResolveTag("canine");
            Assert.That(firstResult, Is.Not.Null);
            Assert.That(firstResult.Name, Is.EqualTo(expectedFirstTagName));

            var firstAliases = firstResult.Aliases.ToList();
            Assert.That(firstAliases, Is.Empty);

            var firstImplications = firstResult.Implications.Select(i => i.Name).ToList();
            Assert.That(firstImplications, Has.Count.EqualTo(2));
            var expectedFirstImplications =
                new List<string>() { expectedFirstImplicationName, expectedSecondImplicationName };
            Assert.That(firstImplications, Is.EquivalentTo(expectedFirstImplications));

            var secondResult = target.ResolveTag("Feline");
            Assert.That(secondResult, Is.Not.Null);
            Assert.That(secondResult.Name, Is.EqualTo(expectedSecondTagName));

            var secondAliases = secondResult.Aliases.ToList();
            Assert.That(secondAliases, Is.Empty);

            var secondImplications = secondResult.Implications.Select(i => i.Name).ToList();
            Assert.That(secondImplications, Has.Count.EqualTo(2));
            var expectedSecondImplications =
                new List<string>() { expectedFirstImplicationName, expectedSecondImplicationName };
            Assert.That(secondImplications, Is.EquivalentTo(expectedSecondImplications));
        }

        [Test]
        public void CanAddAndResolveCircularImplicationSpecifications()
        {
            var tagString = "foo > bar, bar > baz, baz > foo";
            var expectedFirstTagName = Tag.CanonicalizeTagName("foo");
            var expectedSecondTagName = Tag.CanonicalizeTagName("bar");
            var expectedThirdTagName = Tag.CanonicalizeTagName("baz");
            var expectedTagNames =
                new List<string>() { expectedFirstTagName, expectedSecondTagName, expectedThirdTagName };
            var target = new TagSet(tagString);

            foreach (string expectedTagName in expectedTagNames)
            {
                var actualTag = target.ResolveTag(expectedTagName);
                Assert.That(actualTag, Is.Not.Null);
                Assert.That(actualTag.Name, Is.EqualTo(expectedTagName));

                var actualTagAliases = actualTag.Aliases.ToList();
                Assert.That(actualTagAliases, Is.Empty);

                var actualTagImplications = actualTag.Implications.ToList();
                Assert.That(actualTagImplications.Select(t => t.Name),
                    Is.EquivalentTo(expectedTagNames.Where(tn => !tn.Equals(expectedTagName))));
            }
        }

        [Test]
        public void CanAddAndResolveTagWithMultipleImplications()
        {
            var tagString = "gryphon > hybrid, gryphon > lion, gryphon > eagle";
            var expectedRootTagName = Tag.CanonicalizeTagName("gryphon");
            var expectedImplications = new List<string>() { "hybrid", "lion", "eagle" }
                .Select(tn => Tag.CanonicalizeTagName(tn))
                .ToList();
            var target = new TagSet(tagString);

            var rootTag = target.ResolveTag("Gryphon");
            Assert.That(rootTag, Is.Not.Null);
            Assert.That(rootTag.Name, Is.EqualTo(expectedRootTagName));

            var rootTagAliases = rootTag.Aliases.ToList();
            Assert.That(rootTagAliases, Is.Empty);

            var rootTagImplications = rootTag.Implications.ToList();
            Assert.That(rootTagImplications, Has.Count.EqualTo(3));
            Assert.That(rootTagImplications.Select(t => t.Name), Is.EquivalentTo(expectedImplications));

            foreach (var implication in rootTagImplications)
            {
                var implicationAliases = implication.Aliases.ToList();
                Assert.That(implicationAliases, Is.Empty);

                var implicationImplications = implication.Implications.ToList();
                Assert.That(implicationImplications, Is.Empty);
            }
        }

        [Test]
        public void CanAddAndResolveMixedSpecifications()
        {
            var tagString = "Puma, Mountain Lion = Puma, Puma > Feline, Canine";
            var expectedRootTagName = Tag.CanonicalizeTagName("puma");
            var expectedAlias = Tag.CanonicalizeTagName("__mountain__Lion__");
            var expectedImplicationTagName = Tag.CanonicalizeTagName("feline");
            var expectedOtherTagName = Tag.CanonicalizeTagName("canine");
            var target = new TagSet(tagString);

            var rootResult = target.ResolveTag("mountain lion");
            Assert.That(rootResult, Is.Not.Null);
            Assert.That(rootResult.Name, Is.EqualTo(expectedRootTagName));

            var rootAliases = rootResult.Aliases.ToList();
            Assert.That(rootAliases, Has.Count.EqualTo(1));
            Assert.That(rootAliases.First(), Is.EqualTo(expectedAlias));

            var rootImplications = rootResult.Implications.ToList();
            Assert.That(rootImplications, Has.Count.EqualTo(1));

            var implicationTag = rootImplications.First();
            Assert.That(implicationTag, Is.Not.Null);
            Assert.That(implicationTag.Name, Is.EqualTo(expectedImplicationTagName));

            var implicationAliases = implicationTag.Aliases.ToList();
            Assert.That(implicationAliases, Is.Empty);

            var implicationImplications = implicationTag.Implications.ToList();
            Assert.That(implicationImplications, Is.Empty);

            var otherTag = target.ResolveTag("Canine");
            Assert.That(otherTag, Is.Not.Null);
            Assert.That(otherTag.Name, Is.EqualTo(expectedOtherTagName));

            var otherAliases = otherTag.Aliases.ToList();
            Assert.That(otherAliases, Is.Empty);

            var otherImplications = otherTag.Implications.ToList();
            Assert.That(otherImplications, Is.Empty);
        }

        [Test]
        public void CanMakeUpdatesToTagSet()
        {
            var tagString1 = "lion, tiger, bear";
            var target = new TagSet(tagString1);

            var firstExpectedTagNames = new List<string>() { "lion", "tiger", "bear", }
                .Select(Tag.CanonicalizeTagName);
            var firstActualTagNames = target.Select(t => t.Name);
            Assert.That(firstActualTagNames, Is.EquivalentTo(firstExpectedTagNames));

            var tagString2 = "tiger, lion, leopard";
            target.ResolveTagList(tagString2, true, false);

            var secondExpectedTagNames = new List<string>() { "lion", "tiger", "bear", "leopard" }
                .Select(Tag.CanonicalizeTagName);
            var secondActualTagNames = target.Select(t => t.Name);
            Assert.That(secondActualTagNames, Is.EquivalentTo(secondExpectedTagNames));
        }

        [Test]
        public void AliasUpdatesAreReflectedInPreviousTags()
        {
            var target = new TagSet();
            var firstTagString = "puma";
            var expectedCanonicalTagName = Tag.CanonicalizeTagName("puma");

            var firstTag = target.ResolveTag(firstTagString, true);
            Assert.That(firstTag, Is.Not.Null);
            Assert.That(firstTag.Name, Is.EqualTo(expectedCanonicalTagName));

            var firstAliasesBeforeUpdate = firstTag.Aliases.ToList();
            Assert.That(firstAliasesBeforeUpdate, Is.Empty);

            var expectedAliasTagName = Tag.CanonicalizeTagName("mountain lion");
            var secondTag = target.ResolveTag("mountain lion = puma", true);
            Assert.That(secondTag, Is.Not.Null);
            Assert.That(secondTag.Name, Is.EqualTo(expectedCanonicalTagName));

            var secondAliases = secondTag.Aliases.ToList();
            Assert.That(secondAliases, Has.Count.EqualTo(1));
            Assert.That(secondAliases.First(), Is.EqualTo(expectedAliasTagName));

            var firstAliasesAfterUpdate = firstTag.Aliases.ToList();
            Assert.That(firstAliasesAfterUpdate, Has.Count.EqualTo(1));
            Assert.That(firstAliasesAfterUpdate.First(), Is.EqualTo(expectedAliasTagName));
        }

        [Test]
        public void ImplicationUpdatesAreReflectedInPreviousTags()
        {
            var target = new TagSet();
            var firstTagString = "puma";
            var expectedCanonicalTagName = Tag.CanonicalizeTagName("puma");

            var firstTag = target.ResolveTag(firstTagString, true);
            Assert.That(firstTag, Is.Not.Null);
            Assert.That(firstTag.Name, Is.EqualTo(expectedCanonicalTagName));

            var firstImplicationsBeforeUpdate = firstTag.Implications.ToList();
            Assert.That(firstImplicationsBeforeUpdate, Is.Empty);

            var expectedImplicationTagName = Tag.CanonicalizeTagName("feline");
            var secondTag = target.ResolveTag("puma > feline", true);
            Assert.That(secondTag, Is.Not.Null);
            Assert.That(secondTag.Name, Is.EqualTo(expectedCanonicalTagName));

            var secondImplications = secondTag.Implications.ToList();
            Assert.That(secondImplications, Has.Count.EqualTo(1));

            var secondImplicationTag = secondImplications.First();
            Assert.That(secondImplicationTag.Name, Is.EqualTo(expectedImplicationTagName));

            var firstImplicationsAfterUpdate = firstTag.Implications.ToList();
            Assert.That(firstImplicationsAfterUpdate, Has.Count.EqualTo(1));

            var firstImplicationTag = firstImplicationsAfterUpdate.First();
            Assert.That(firstImplicationTag.Name, Is.EqualTo(expectedImplicationTagName));
        }

        [Test]
        public void CanResolveTagStringToRootsWithUpdates()
        {
            var tagString = "Puma, Mountain Lion = Puma, Puma > Feline, Canine";
            var expectedRootTagName = Tag.CanonicalizeTagName("puma");
            var expectedAlias = Tag.CanonicalizeTagName("mountain lion");
            var expectedImplicationTagName = Tag.CanonicalizeTagName("feline");
            var expectedOtherTagName = Tag.CanonicalizeTagName("canine");
            var expectedResultTagNames = new List<string>() { expectedRootTagName, expectedOtherTagName };
            var target = new TagSet();

            var result = target.ResolveTagList(tagString, true, false).ToList();
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Select(t => t.Name), Is.EquivalentTo(expectedResultTagNames));

            var rootTag = result.First(t => t.Name.Equals(expectedRootTagName));
            var rootTagAliases = rootTag.Aliases.ToList();
            Assert.That(rootTagAliases, Has.Count.EqualTo(1));
            Assert.That(rootTagAliases.First(), Is.EqualTo(expectedAlias));

            var rootTagImplications = rootTag.Implications.ToList();
            Assert.That(rootTagImplications, Has.Count.EqualTo(1));

            var implicationTag = rootTagImplications.First();
            Assert.That(implicationTag, Is.Not.Null);
            Assert.That(implicationTag.Name, Is.EqualTo(expectedImplicationTagName));

            var implicationAliases = implicationTag.Aliases.ToList();
            Assert.That(implicationAliases, Is.Empty);

            var implicationImplications = implicationTag.Implications.ToList();
            Assert.That(implicationImplications, Is.Empty);

            var otherTag = result.First(t => t.Name.Equals(expectedOtherTagName));
            var otherAliases = otherTag.Aliases.ToList();
            Assert.That(otherAliases, Is.Empty);

            var otherImplications = otherTag.Implications.ToList();
            Assert.That(otherImplications, Is.Empty);
        }

        [Test]
        public void CanResolveTagStringToImplicationsWithUpdates()
        {
            var tagString = "Puma, Mountain Lion = Puma, Puma > Feline, Canine";
            var expectedRootTagName = Tag.CanonicalizeTagName("puma");
            var expectedAlias = Tag.CanonicalizeTagName("mountain lion");
            var expectedImplicationTagName = Tag.CanonicalizeTagName("feline");
            var expectedOtherTagName = Tag.CanonicalizeTagName("canine");
            var expectedResultTagNames =
                new List<string>() { expectedRootTagName, expectedOtherTagName, expectedImplicationTagName };
            var target = new TagSet();

            var result = target.ResolveTagList(tagString, true, true).ToList();
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result.Select(t => t.Name), Is.EquivalentTo(expectedResultTagNames));

            var rootTag = result.First(t => t.Name.Equals(expectedRootTagName));
            var rootTagAliases = rootTag.Aliases.ToList();
            Assert.That(rootTagAliases, Has.Count.EqualTo(1));
            Assert.That(rootTagAliases.First(), Is.EqualTo(expectedAlias));

            var rootTagImplications = rootTag.Implications.ToList();
            Assert.That(rootTagImplications, Has.Count.EqualTo(1));

            var implicationTag = rootTagImplications.First();
            Assert.That(implicationTag, Is.Not.Null);
            Assert.That(implicationTag.Name, Is.EqualTo(expectedImplicationTagName));
            Assert.That(implicationTag, Is.EqualTo(result.First(t => t.Name.Equals(expectedImplicationTagName))));

            var implicationAliases = implicationTag.Aliases.ToList();
            Assert.That(implicationAliases, Is.Empty);

            var implicationImplications = implicationTag.Implications.ToList();
            Assert.That(implicationImplications, Is.Empty);

            var otherTag = result.First(t => t.Name.Equals(expectedOtherTagName));
            var otherAliases = otherTag.Aliases.ToList();
            Assert.That(otherAliases, Is.Empty);

            var otherImplications = otherTag.Implications.ToList();
            Assert.That(otherImplications, Is.Empty);
        }
    }
}
