
CREATE OR ALTER PROCEDURE sp_InitialSeedCategories
AS 
BEGIN
	IF NOT EXISTS
	(
		SELECT 1
		FROM CDL_Identifiers C
		WHERE C.[Set] = 'CAT'
	)
	BEGIN
		
		INSERT INTO CoastalPharmacyDB.dbo.CDL_Identifiers (Id,[Set],ElementNumber,Code,Description,[Use],ParentId) VALUES
		 (N'AC8DE04E-654F-412E-AFA7-28816D96EC8F',N'CAT',1,N'CAT1',N'Medicamentos',N'Categoria de los productos',NULL),
		 (N'EFFCFE75-5163-4A90-B6B8-B10CA0A96775',N'CAT',2,N'CAT2',N'Cuidado Personal',N'Categoria de los productos',NULL),
		 (N'0A93A1A7-59ED-41FE-83EF-2C49F54876E9',N'CAT',4,N'CAT4',N'Bebés y Maternidad',N'Categoria de los productos',NULL),
		 (N'F484E970-E67B-4DAC-B4A6-507D70CC46CB',N'CAT',3,N'CAT3',N'Equipos médicos',N'Categoria de los productos',NULL)	
		 
		 PRINT 'OK CATEGORIES'
	END
	
	IF NOT EXISTS
	(
		SELECT 1
		FROM CDL_Identifiers C
		WHERE C.[Set] = 'SAT'
	)
	
	BEGIN
		INSERT INTO CoastalPharmacyDB.dbo.CDL_Identifiers (Id,[Set],ElementNumber,Code,Description,[Use],ParentId) VALUES
		 (N'9112155D-CCC9-421B-AF59-67F90CA3B6AB',N'SAT',2,N'SAT2',N'Antibióticos',N'SubCategoria de los productos',N'AC8DE04E-654F-412E-AFA7-28816D96EC8F'),
		 (N'267462B8-4901-4A52-AD0D-6C51CA3F413B',N'SAT',11,N'SAT11',N'Hormonas',N'SubCategoria de los productos',N'AC8DE04E-654F-412E-AFA7-28816D96EC8F'),
		 (N'DD63A56C-0F0C-4C32-96C8-7DDD2C1699EB',N'SAT',6,N'SAT6',N'Cuidado de la Piel',N'SubCategoria de los productos',N'EFFCFE75-5163-4A90-B6B8-B10CA0A96775'),
		 (N'C8279B78-9F3B-4483-9B46-98242C790798',N'SAT',9,N'SAT9',N'Pañales y Toallitas',N'SubCategoria de los productos',N'0A93A1A7-59ED-41FE-83EF-2C49F54876E9'),
		 (N'9E823E9D-9E1C-4BAC-87BF-9A622873BB79',N'SAT',5,N'SAT5',N'Higiene Oral',N'SubCategoria de los productos',N'EFFCFE75-5163-4A90-B6B8-B10CA0A96775'),
		 (N'432B3536-6129-4093-8613-A11215D62A71',N'SAT',7,N'SAT7',N'Monitoreo',N'SubCategoria de los productos',N'F484E970-E67B-4DAC-B4A6-507D70CC46CB'),
		 (N'2D7B3235-636D-4E4C-879C-BA08DCB270A3',N'SAT',1,N'SAT1',N'Analgésicos',N'SubCategoria de los productos',N'AC8DE04E-654F-412E-AFA7-28816D96EC8F'),
		 (N'7AAE3756-8A2A-4DD6-A0E2-1D9416F18709',N'SAT',4,N'SAT4',N'Cuidado Capilar',N'SubCategoria de los productos',N'EFFCFE75-5163-4A90-B6B8-B10CA0A96775'),
		 (N'4E3D5A7D-5CE5-46C9-823C-2209E78748AA',N'SAT',8,N'SAT8',N'Primeros Auxilios',N'SubCategoria de los productos',N'F484E970-E67B-4DAC-B4A6-507D70CC46CB'),
		 (N'5001189B-D1C2-4BCA-B163-24E089B5EE6F',N'SAT',3,N'SAT3',N'Gastrointestinales',N'SubCategoria de los productos',N'AC8DE04E-654F-412E-AFA7-28816D96EC8F'),
		 (N'75F3B02F-DA73-43D5-A0E5-4A762A800589',N'SAT',10,N'SAT10',N'Alimentación Infantil',N'SubCategoria de los productos',N'0A93A1A7-59ED-41FE-83EF-2C49F54876E9')
		 
		 PRINT 'OK SUBCATEGORIES'
	END
END
