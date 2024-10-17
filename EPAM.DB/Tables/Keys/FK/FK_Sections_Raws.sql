ALTER TABLE [DbF].[Raws]
	ADD CONSTRAINT [FK_Sections_Raws]
	FOREIGN KEY ([SectionId])
	REFERENCES [DbF].[Sections] ([Id])
