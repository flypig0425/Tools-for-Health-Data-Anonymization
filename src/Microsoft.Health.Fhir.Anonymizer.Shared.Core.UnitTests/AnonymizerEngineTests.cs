﻿using System.IO;
using Fhir.Anonymizer.Shared.Core;
using Hl7.Fhir.Serialization;
using Microsoft.Health.Fhir.Anonymizer.Core.AnonymizerConfigurations;
using Xunit;

namespace Microsoft.Health.Fhir.Anonymizer.Core.UnitTests
{
    public class AnonymizerEngineTests
    {
        public AnonymizerEngineTests()
        {
            AnonymizerEngine.InitializeFhirPathExtensionSymbols();
        }

        [Fact]
        public void GivenIsPrettyOutputSetTrue_WhenAnonymizeJson_PrettyJsonOutputShouldBeReturned()
        {
            AnonymizerEngine engine = new AnonymizerEngine(Path.Combine("TestConfigurations", "configuration-test-sample.json"));
            var settings = new AnonymizerSettings()
            {
                IsPrettyOutput = true
            };
            var result = engine.AnonymizeJson(TestPatientSample, settings);
            Assert.Equal(PrettyOutputTarget, result);
        }

        [Fact]
        public void GivenIsPrettyOutputSetFalse_WhenAnonymizeJson_OneLineJsonOutputShouldBeReturned()
        {
            AnonymizerEngine engine = new AnonymizerEngine(Path.Combine("TestConfigurations", "configuration-test-sample.json"));

            var result = engine.AnonymizeJson(TestPatientSample);
            Assert.Equal(OneLineOutputTarget, result);
        }

        [Fact]
        public void GivenAnonymizerEngine_WhenUpdateFhirLibrary_ThenEngineSupportLibraryVersionShouldBeUpdated()
        {
            Assert.Equal(typeof(FhirJsonParser).Assembly.GetName().Version.ToString(3), AnonymizerConstants.FhirLibraryVersion);
        }

        private const string TestPatientSample =
@"{
  ""resourceType"": ""Patient"",
  ""id"": ""example"",
  ""name"": [
    {
      ""use"": ""official"",
      ""family"": ""Chalmers"",
      ""given"": [
        ""Peter"",
        ""James""
      ]
    }
  ]
}";

        private const string PrettyOutputTarget =
@"{
  ""resourceType"": ""Patient"",
  ""id"": ""example"",
  ""meta"": {
    ""security"": [
      {
        ""system"": ""http://terminology.hl7.org/CodeSystem/v3-ObservationValue"",
        ""code"": ""REDACTED"",
        ""display"": ""redacted""
      }
    ]
  }
}";

        private const string OneLineOutputTarget = "{\"resourceType\":\"Patient\",\"id\":\"example\",\"meta\":{\"security\":[{\"system\":\"http://terminology.hl7.org/CodeSystem/v3-ObservationValue\",\"code\":\"REDACTED\",\"display\":\"redacted\"}]}}";
    }
}
