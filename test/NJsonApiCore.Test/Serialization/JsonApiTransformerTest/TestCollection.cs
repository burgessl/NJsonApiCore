﻿using NJsonApi.Infrastructure;
using NJsonApi.Serialization.Documents;
using NJsonApi.Serialization.Representations;
using NJsonApi.Serialization.Representations.Resources;
using NJsonApi.Test.Builders;
using NJsonApi.Test.TestControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NJsonApi.Test.Serialization.JsonApiTransformerTest
{
    public class TestCollection
    {
        private readonly List<string> reservedKeys = new List<string> { "id", "type", "href", "links" };

        [Fact]
        public void Creates_CompondDocument_for_collection_not_nested_class_and_propertly_map_resourceName()
        {
            // Arrange
            var context = CreateContext();
            IEnumerable<SampleClass> objectsToTransform = CreateObjectToTransform();
            var transformer = new JsonApiTransformerBuilder()
                .With(CreateConfiguration())
                .Build();

            // Act
            CompoundDocument result = transformer.Transform(objectsToTransform, context);

            // Assert
            Assert.NotNull(result.Data);
            var transformedObject = result.Data as ResourceCollection;
            Assert.NotNull(transformedObject);
        }

        [Fact]
        public void Creates_CompondDocument_for_collection_not_nested_single_class()
        {
            // Arrange
            var configuration = CreateContext();
            IEnumerable<SampleClass> objectsToTransform = CreateObjectToTransform().Take(1);
            var transformer = new JsonApiTransformerBuilder()
                .With(CreateConfiguration())
                .Build();

            // Act
            CompoundDocument result = transformer.Transform(objectsToTransform, configuration);

            // Assert
            Assert.NotNull(result.Data);
            var transformedObject = result.Data as ResourceCollection;
            Assert.NotNull(transformedObject);
        }

        [Fact]
        public void Creates_CompondDocument_for_collection_not_nested_class_and_propertly_map_id()
        {
            // Arrange
            var configuration = CreateContext();
            IEnumerable<SampleClass> objectsToTransform = CreateObjectToTransform();
            var transformer = new JsonApiTransformerBuilder()
                .With(CreateConfiguration())
                .Build();

            // Act
            CompoundDocument result = transformer.Transform(objectsToTransform, configuration);

            // Assert
            var transformedObject = result.Data as ResourceCollection;
            Assert.Equal(transformedObject[0].Id, objectsToTransform.First().Id.ToString());
            Assert.Equal(transformedObject[1].Id, objectsToTransform.Last().Id.ToString());
        }

        [Fact]
        public void Creates_CompondDocument_for_collection_not_nested_class_and_propertly_map_properties()
        {
            // Arrange
            var context = CreateContext();
            IEnumerable<SampleClass> objectsToTransform = CreateObjectToTransform();
            var transformer = new JsonApiTransformerBuilder()
                .With(CreateConfiguration())
                .Build();

            // Act
            CompoundDocument result = transformer.Transform(objectsToTransform, context);

            // Assert
            var transformedObject = result.Data as ResourceCollection;

            Action<SingleResource, SampleClass> assertSame = (actual, expected) =>
            {
                Assert.Equal(actual.Attributes["someValue"], expected.SomeValue);
                Assert.Equal(actual.Attributes["date"], expected.DateTime);
                Assert.Equal(actual.Attributes.Count(), 2);
            };

            assertSame(transformedObject[0], objectsToTransform.First());
            assertSame(transformedObject[1], objectsToTransform.Last());
        }


        [Fact]
        public void Creates_CompondDocument_for_collection_not_nested_class_and_propertly_map_type()
        {
            // Arrange
            var configuration = CreateContext();
            IEnumerable<SampleClass> objectsToTransform = CreateObjectToTransform();
            var transformer = new JsonApiTransformerBuilder()
                .With(CreateConfiguration())
                .Build();

            // Act
            CompoundDocument result = transformer.Transform(objectsToTransform, configuration);

            // Assert
            var transformedObject = result.Data as ResourceCollection;
            Assert.Equal(transformedObject[0].Type, "sampleClasses");
            Assert.Equal(transformedObject[1].Type, "sampleClasses");
        }

        [Fact]
        public void Creates_CompondDocument_for_collection_derived_class_and_properly_map_properties_and_type()
        {
            // Arrange
            var context = CreateContext();
            IEnumerable<SampleClass> objectsToTransform = CreateDerivedObjectList();
            var transformer = new JsonApiTransformerBuilder()
                .With(CreateConfiguration())
                .Build();

            // Act
            CompoundDocument result = transformer.Transform(objectsToTransform, context);

            // Assert
            var transformedObject = result.Data as ResourceCollection;

            Action<SingleResource, DerivedClass> assertSame = (actual, expected) =>
            {
                Assert.Equal(actual.Attributes["someValue"], expected.SomeValue);
                Assert.Equal(actual.Attributes["date"], expected.DateTime);
                Assert.Equal(actual.Attributes["derivedProperty"], expected.DerivedProperty);
                Assert.Equal(actual.Attributes.Count(), 3);
            };

            assertSame(transformedObject[0], objectsToTransform.First() as DerivedClass);
            assertSame(transformedObject[1], objectsToTransform.Last() as DerivedClass);
            Assert.Equal(transformedObject[0].Type, "derivedClasses");
            Assert.Equal(transformedObject[1].Type, "derivedClasses");
        }

        [Fact]
        public void Creates_CompondDocument_for_collectione_of_metadatawrapper_throws_notSupportedException()
        {
            // Arrange
            var configuration = CreateContext();
            var objectsToTransform = new List<MetaDataWrapper<SampleClass>>
            {
                new MetaDataWrapper<SampleClass>(new SampleClass())
            };
            var transformer = new JsonApiTransformerBuilder().Build();

            // Act => Assert
            Assert.Throws<NotSupportedException>(() => transformer.Transform(objectsToTransform, configuration));
        }

        private static IEnumerable<SampleClass> CreateObjectToTransform()
        {
            var objectToTransformOne = new SampleClass
            {
                Id = 1,
                SomeValue = "Somevalue text test string",
                DateTime = DateTime.UtcNow,
                NotMappedValue = "Should be not mapped"
            };

            var objectToTransformTwo = new SampleClass
            {
                Id = 2,
                SomeValue = "Somevalue text test string",
                DateTime = DateTime.UtcNow.AddDays(1),
                NotMappedValue = "Should be not mapped"
            };

            return new List<SampleClass>()
            {
                objectToTransformOne,
                objectToTransformTwo
            };
        }

        private static IEnumerable<SampleClass> CreateDerivedObjectList()
        {
            var objectToTransformOne = new DerivedClass
            {
                Id = 1,
                SomeValue = "Somevalue text test string",
                DateTime = DateTime.UtcNow,
                NotMappedValue = "Should be not mapped",
                DerivedProperty = "A value from the derived class"
            };

            var objectToTransformTwo = new DerivedClass
            {
                Id = 2,
                SomeValue = "Somevalue text test string",
                DateTime = DateTime.UtcNow.AddDays(1),
                NotMappedValue = "Should be not mapped",
                DerivedProperty = "A value from the derived class"
            };

            return new List<SampleClass>()
            {
                objectToTransformOne,
                objectToTransformTwo
            };
        }

        private Context CreateContext()
        {
            var requestUri = new Uri("http://fakeUri:1234/fakecontroller");

            return new Context(requestUri);
        }

        private IConfiguration CreateConfiguration()
        {
            var mapping = new ResourceMapping<SampleClass, DummyController>(c => c.Id);
            mapping.ResourceType = "sampleClasses";
            mapping.AddPropertyGetter("someValue", c => c.SomeValue);
            mapping.AddPropertyGetter("date", c => c.DateTime);

            var derivedMapping = new ResourceMapping<DerivedClass, DummyController>(c => c.Id);
            derivedMapping.ResourceType = "derivedClasses";
            derivedMapping.AddPropertyGetter("someValue", c => c.SomeValue);
            derivedMapping.AddPropertyGetter("date", c => c.DateTime);
            derivedMapping.AddPropertyGetter("derivedProperty", c => c.DerivedProperty);

            var config = new NJsonApi.Configuration();
            config.AddMapping(mapping);
            config.AddMapping(derivedMapping);
            return config;
        }

        private class SampleClass
        {
            public int Id { get; set; }
            public string SomeValue { get; set; }
            public DateTime DateTime { get; set; }
            public string NotMappedValue { get; set; }
        }

        private class DerivedClass : SampleClass
        {
            public string DerivedProperty { get; set; }
        }
    }
}