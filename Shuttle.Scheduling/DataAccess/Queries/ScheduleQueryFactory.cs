﻿using System;
using Shuttle.Core.Data;

namespace Shuttle.Scheduling
{
	public class ScheduleQueryFactory : IScheduleQueryFactory
	{
		public IQuery All()
		{
			return RawQuery.Create(@"
select
	[Name],
	[InboxWorkQueueUri],
	[CronExpression],
	[NextNotification]
from
	[dbo].[Schedule]
order by
	[Name]
");
		}

		public IQuery HasScheduleStructures()
		{
			return
				RawQuery.Create(
					"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Schedule') select 1 ELSE select 0");
		}

		public IQuery Add(string name, string inboxWorkQueueUri, string cronExpression, DateTime nextNotification)
		{
			return RawQuery.Create(@"
insert into [dbo].[Schedule]
(
	[Name],
	[InboxWorkQueueUri],
	[CronExpression],
	[NextNotification]
)
values
(
	@Name,
	@InboxWorkQueueUri,
	@CronExpression,
	@NextNotification
)
")
			               .AddParameterValue(ScheduleColumns.Name, name)
			               .AddParameterValue(ScheduleColumns.InboxWorkQueueUri, inboxWorkQueueUri)
			               .AddParameterValue(ScheduleColumns.CronExpression, cronExpression)
			               .AddParameterValue(ScheduleColumns.NextNotification, nextNotification);
		}

		public IQuery Remove(string name)
		{
			return RawQuery.Create(@"delete from [dbo].[Schedule] where [Name] = @Name")
			               .AddParameterValue(ScheduleColumns.Name, name);
		}

		public IQuery Contains(string name)
		{
			return RawQuery.Create(@"if exists (select null from [dbo].[Schedule] where [Name] = @Name) select 1 else select 0")
						   .AddParameterValue(ScheduleColumns.Name, name);
		}

		public IQuery Save(string name, string inboxWorkQueueUri, string cronExpression, DateTime nextNotification)
		{
			return RawQuery.Create(@"
update [dbo].[Schedule] set
	[InboxWorkQueueUri] = @InboxWorkQueueUri,
	[CronExpression] = @CronExpression,
	[NextNotification] = @NextNotification
where
	[Name] = @Name
")
						   .AddParameterValue(ScheduleColumns.Name, name)
						   .AddParameterValue(ScheduleColumns.InboxWorkQueueUri, inboxWorkQueueUri)
						   .AddParameterValue(ScheduleColumns.CronExpression, cronExpression)
						   .AddParameterValue(ScheduleColumns.NextNotification, nextNotification);
		}

		public IQuery SaveNextNotification(string name, DateTime nextNotification)
		{
			return RawQuery.Create(@"
update [dbo].[Schedule] set
	[NextNotification] = @NextNotification
where
	[Name] = @Name
")
						   .AddParameterValue(ScheduleColumns.Name, name)
						   .AddParameterValue(ScheduleColumns.NextNotification, nextNotification);
		}
	}
}