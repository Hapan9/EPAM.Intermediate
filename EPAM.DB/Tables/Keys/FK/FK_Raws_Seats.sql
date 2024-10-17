ALTER TABLE [DbF].[Seats]
	ADD CONSTRAINT [FK_Raws_Seats]
	FOREIGN KEY ([RawId])
	REFERENCES [DbF].[Raws] ([Id])
