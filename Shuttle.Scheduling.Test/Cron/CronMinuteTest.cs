using System;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Scheduling.Test
{
	[TestFixture]
	public class CronMinuteTest
	{
		[Test]
		public void Should_be_able_to_satisfy_asterisk_value()
		{
			var field = new CronMinute("*");

			var date = new DateTime(2011, 01, 01, 0, 0, 0);

			for (var i = 0; i < 60; i++)
			{
				Assert.AreEqual(date, field.SnapForward(date));

				date = date.AddMinutes(1);
			}
		}

		[Test]
		public void Should_be_able_to_satisfy_individual_values()
		{
			var field = new CronMinute("5,10,15,30,45");

			var control = new DateTime(2011, 01, 01, 0, 0, 0);
			var date = field.SnapForward(control);

			Assert.AreEqual(control.AddMinutes(5), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(10), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(15), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(30), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(45), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(65), date);
		}

		[Test]
		public void Should_be_able_to_satisfy_a_range_of_values()
		{
			var field = new CronMinute("5-10");

			var control = new DateTime(2011, 01, 01, 0, 0, 0);
			var date = field.SnapForward(control);

			Assert.AreEqual(control.AddMinutes(5), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(6), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(7), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(8), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(9), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(10), date);
		}

		[Test]
		public void Should_be_able_to_satisfy_a_stepped_values()
		{
			var field = new CronMinute("5-10/5");

			var control = new DateTime(2011, 01, 01, 0, 0, 0);
			var date = field.SnapForward(control);

			Assert.AreEqual(control.AddMinutes(5), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(10), date);
			date = field.SnapForward(date.AddMinutes(1));

			Assert.AreEqual(control.AddMinutes(65), date);
		}

		[Test]
		public void Should_throw_exceptions_on_invalid_expressions()
		{
			Assert.Throws<EmptyStringException>(() => new CronMinute(""));
			Assert.Throws<CronException>(() => new CronMinute("invalid"));
			Assert.Throws<CronException>(() => new CronMinute("10-60"));
			Assert.Throws<CronException>(() => new CronMinute("60-60"));
			Assert.Throws<CronException>(() => new CronMinute("10-5"));
			
			new CronMinute("*/15");
		}
	}
}