﻿ALTER TABLE [DbF].[Venues]
	ADD CONSTRAINT [DF_Venues_Id]
	DEFAULT NewID()
	FOR [Id]
