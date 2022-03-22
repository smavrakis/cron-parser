using CronParser.Constants;
using CronParser.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CronParser.Tests.Models
{
    public class CronFieldTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Given_empty_name_When_formatting_the_string_Then_should_keep_the_padded_length(string name)
        {
            // Given
            var range = new CronFieldRange { Start = 1, End = 2 };
            var sut = new CronField(name, new List<CronFieldRange> { range });

            // When
            var result = sut.ToFormattedString();

            // Then
            result.Should().Be($"{string.Empty.PadRight(CronFieldLimits.NameOutputLength)} 1 2");
        }

        [Fact]
        public void Given_null_ranges_When_formatting_the_string_Then_should_return_the_field_name_only()
        {
            // Given            

            var sut = new CronField(CronFieldNames.Minute, null!);

            // When
            var result = sut.ToFormattedString();

            // Then
            result.Should().Be(CronFieldNames.Minute);
        }

        [Fact]
        public void Given_empty_ranges_When_formatting_the_string_Then_should_return_the_field_name_only()
        {
            // Given            
            var sut = new CronField(CronFieldNames.Minute, new List<CronFieldRange>());

            // When
            var result = sut.ToFormattedString();

            // Then
            result.Should().Be(CronFieldNames.Minute);
        }

        [Fact]
        public void Given_various_ranges_When_formatting_the_string_Then_should_return_the_expected_result()
        {
            // Given
            var sut = new CronField(CronFieldNames.Minute, new List<CronFieldRange> { new CronFieldRange { Start = 1, End = 2 }, null!,
                new CronFieldRange { Start = 5, End = 10, Step = 2 }, new CronFieldRange { Start = 20, End = 20, Step = 1 },
                new CronFieldRange { Start = 30, End = 30, Step = -1 }, new CronFieldRange { Start = 30, End = 20 }});

            // When
            var result = sut.ToFormattedString();

            // Then
            result.Should().Be($"{CronFieldNames.Minute.PadRight(CronFieldLimits.NameOutputLength)} 1 2 5 7 9 20");
        }
    }
}