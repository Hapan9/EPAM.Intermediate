ALTER TABLE [DbF].[Sections]
	ADD CONSTRAINT [DF_Sections_Id]
	DEFAULT NewID()
	FOR [Id]
