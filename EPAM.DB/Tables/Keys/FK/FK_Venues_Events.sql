ALTER TABLE [DbF].[Events]
	ADD CONSTRAINT [FK_Venues_Events]
	FOREIGN KEY ([VenueId])
	REFERENCES [DbF].[Venues] ([Id])
