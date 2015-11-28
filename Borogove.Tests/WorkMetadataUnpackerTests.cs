using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSubstitute;
using Wyam.Common;

namespace Borogove.Tests
{
    [TestFixture]
    public class WorkMetadataUnpackerTests
    {
        [Test]
        public void DefaultConstructorWorks()
        {
            string testFrontMatter = @"Identifier: 8f0e9e4b-544d-4357-a6e9-f38d47812209
Title: Your Friendly Neighborhood Acme Inc. Salescoyote
Description: A lonely housewife gets a visit from her friendly neighborhood Acme Inc. salesman, who happens to be a coyote with some exciting products.
Creator:
  - role: Author
    text: Hank Raven
    file-as: Jason Mitchell
  - role: Copyright Holder
    text: Jason Mitchell
Rights: ©2015 Jason Mitchell, CC BY-NC
Language: en-US
Work Type: writing
Non-Borogove Metadata: 1337
Content Rating: T12
Content Descriptors: Alcohol Reference, Crude Humor, Drug Reference, Partial Nudity, Cartoon Violence, Suggestive Themes
Tags: transformation, cartoon, furry, coyote, rabbit, magic, fantasy, satire, male, female, mind shift, reality shift
Parent: 47983827-2601-46c3-aee9-4f9a198d5d19
Previous: bc243614-e943-4e7d-a3b2-574a928adb06
Next: 51cd27cc-79ab-403f-8866-2aabb19a17dd
Draft of: c01b57d8-cca0-4dd3-ab0c-99acbd8e3343
Draft identifier: 1.3
Artifact of: c4b90fc3-c0ac-48c7-a709-5c9177e817af
Comments on: aa370c52-3cf7-4693-b32c-cd39f63e9b61
Created date: 2015-09-13T01:21:13
Modified date: 2015-09-15T01:21:13
Published date: 2015-09-16T01:21:13 #Can set things up to publish on a certain date
";
            string testContent = @"

# The Story...

Doesn't _exist_ yet. :p";
            string testDocument = testFrontMatter + "---" + testContent;

            var inputDocumentMock = Substitute.For<IDocument>();
            var frontMatterDocumentMock = Substitute.For<IDocument>();
            var frontMatterResultDocumentMock = Substitute.For<IDocument>();
            var resultDocumentMock = Substitute.For<IDocument>();
            var finalDocumentMock = Substitute.For<IDocument>();
            Dictionary<string, object> resultMetadata = null;
            Dictionary<string, object> finalMetadata = null;

            inputDocumentMock.Content
                .Returns(testDocument);
            inputDocumentMock.Clone(Arg.Any<string>(), Arg.Any<IEnumerable<KeyValuePair<string, object>>>())
                .Returns(frontMatterDocumentMock);
            frontMatterDocumentMock.Content
                .Returns(testFrontMatter);
            frontMatterDocumentMock.Clone(Arg.Any<Dictionary<string, object>>())
                .Returns(frontMatterResultDocumentMock)
                .AndDoes(ci => resultMetadata = ci.Arg<Dictionary<string, object>>());
            frontMatterResultDocumentMock.Clone(Arg.Any<string>(), Arg.Any<IEnumerable<KeyValuePair<string, object>>>())
                .Returns(resultDocumentMock);
            resultDocumentMock.GetEnumerator().Returns(
                ci =>
                {
                    Assert.NotNull(resultMetadata);
                    return resultMetadata.GetEnumerator();
                });
            resultDocumentMock.Get(Arg.Any<string>()).Returns(
                ci =>
                {
                    var key = ci.Arg<string>();
                    Assert.That(resultMetadata.ContainsKey(key));
                    return resultMetadata[key];
                });
            resultDocumentMock.Source
                .Returns("DefaultConstructorWorks.md");
            resultDocumentMock.Content
                .Returns(testContent);
            resultDocumentMock.Clone(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, object>>())
                .Returns(finalDocumentMock)
                .AndDoes(ci => finalMetadata = ci.Arg<Dictionary<string, object>>());
            var inputDocuments = new List<IDocument>() { inputDocumentMock };

            var executionContextMock = Substitute.For<IExecutionContext>();
            executionContextMock.Execute(Arg.Any<IEnumerable<IModule>>(), Arg.Any<IEnumerable<IDocument>>())
                .Returns(
                    ci =>
                    {
                        var modules = ci.Arg<IEnumerable<IModule>>();
                        Assert.That(modules, Is.Not.Null);
                        Assert.That(modules, Has.Length.EqualTo(1));

                        var module = modules.First();
                        Assert.That(module, Is.Not.Null);

                        var inputs = ci.Arg<IEnumerable<IDocument>>();
                        Assert.That(inputs, Is.Not.Null);

                        return module.Execute(inputs.ToList(), executionContextMock).ToList();
                    });

            var target = new WorkMetadataUnpacker();
            var result = target.Execute(inputDocuments, executionContextMock).ToList();

            Assert.That(resultMetadata, Is.Not.Null);
            Assert.That(resultMetadata, Has.Count.EqualTo(1));

            Assert.That(finalMetadata, Is.Not.Null);

            dynamic resultBorogoveObject = resultMetadata[WorkMetadataUnpacker.DefaultKeyName];
            Assert.NotNull(resultBorogoveObject);
        }
    }
}
