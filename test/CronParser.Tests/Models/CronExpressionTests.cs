using CronParser.Constants;
using CronParser.Exceptions;
using CronParser.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CronParser.Tests.Models
{

    public class CronExpressionTests
    {
        [Fact]
        public void Given_null_expression_When_creating_the_expression_Then_should_throw()
        {
            // Given
            // When            
            var result = () => new CronExpression(null!);

            // Then
            result.Should().Throw<ArgumentNullException>().WithParameterName("expression");
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("*/15")]
        [InlineData("*/15 0")]
        [InlineData("*/15 0 1,15")]
        [InlineData("*/15 0 1,15 *")]
        [InlineData("*/15 0 1,15 * 1-5")]
        public void Given_invalid_expression_When_creating_the_expression_Then_should_throw(string expression)
        {
            // Given
            // When            
            var result = () => new CronExpression(expression);

            // Then
            result.Should().Throw<FormatException>();
        }

        [Theory]
        [InlineData("0,160 0 1,15 * 1-5 /usr/bin/find")]
        [InlineData("*/15 24 1,15 * 1-5 /usr/bin/find")]
        [InlineData("*/15 0 2-32 * 1-5 /usr/bin/find")]
        [InlineData("*/15 0 1,15 13 1-5 /usr/bin/find")]
        [InlineData("*/15 0 1,15 * 0-3 /usr/bin/find")]
        [InlineData("abda 0 1,15 * 1-5 /usr/bin/find")]
        [InlineData("*/15 0 1,15 edf 1-5 /usr/bin/find")]
        [InlineData("*/15 0 1,15 * rty-gfhr /usr/bin/find")]
        public void Given_invalid_range_When_creating_the_expression_Then_should_throw(string expression)
        {
            // Given
            // When            
            var result = () => new CronExpression(expression);

            // Then
            result.Should().Throw<ParseException>();
        }

        [Fact]
        public void Given_valid_expression_When_creating_the_expression_Then_should_return_the_expected_result()
        {
            // Given
            var expression = "*/15 0 1,15 * 1-5 /usr/bin/find";

            // When            
            var result = new CronExpression(expression);

            // Then
            result.Minute.Should().BeEquivalentTo(new CronField(CronFieldNames.Minute, new List<CronFieldRange> { new CronFieldRange { Start = 0, End = 59, Step = 15 } }));
            result.Hour.Should().BeEquivalentTo(new CronField(CronFieldNames.Hour, new List<CronFieldRange> { new CronFieldRange { Start = 0, End = 0, Step = 1 } }));
            result.DayOfMonth.Should().BeEquivalentTo(new CronField(CronFieldNames.DayOfMonth, new List<CronFieldRange> { new CronFieldRange { Start = 1, End = 1, Step = 1 }, 
                new CronFieldRange { Start = 15, End = 15, Step = 1 } }));
            result.Month.Should().BeEquivalentTo(new CronField(CronFieldNames.Month, new List<CronFieldRange> { new CronFieldRange { Start = 1, End = 12, Step = 1 } }));
            result.DayOfWeek.Should().BeEquivalentTo(new CronField(CronFieldNames.DayOfWeek, new List<CronFieldRange> { new CronFieldRange { Start = 1, End = 5, Step = 1 } }));
            result.Command.Should().Be("/usr/bin/find");
        }

        [Fact]
        public void Given_alternative_values_for_month_and_day_of_week_When_creating_the_expression_Then_should_return_the_expected_result()
        {
            // Given
            var expression = "*/15 0 1,15 JUN-DEC SUN,FRI /usr/bin/find";

            // When            
            var result = new CronExpression(expression);

            // Then            
            result.Month.Should().BeEquivalentTo(new CronField(CronFieldNames.Month, new List<CronFieldRange> { new CronFieldRange { Start = 6, End = 12, Step = 1 } }));
            result.DayOfWeek.Should().BeEquivalentTo(new CronField(CronFieldNames.DayOfWeek, new List<CronFieldRange> { new CronFieldRange { Start = 1, End = 1, Step = 1 }, 
                new CronFieldRange { Start = 6, End = 6, Step = 1 } }));            
        } 
    }
}