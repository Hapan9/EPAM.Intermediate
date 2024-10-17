ALTER TABLE [DbF].[Sections]
	ADD CONSTRAINT [FK_Venues_Sections]
	FOREIGN KEY ([VenueId])
	REFERENCES [DbF].[Venues] ([Id])
