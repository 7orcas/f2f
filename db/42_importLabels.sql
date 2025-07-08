﻿DELETE FROM base.langCode;
DELETE FROM base.langLabel;
DELETE FROM base.langKey;

INSERT INTO base.langCode (code,descr) VALUES ('en','English');
INSERT INTO base.langCode (code,descr) VALUES ('de','Deutsch');
INSERT INTO base.langCode (code,descr) VALUES ('it','Italian');
INSERT INTO base.langCode (code,descr) VALUES ('es',N'Español');
INSERT INTO base.langCode (code,descr) VALUES ('ma','Maori');
--TESTING
insert into base.langCode (code,descr) select 'c1', 'xxx'
insert into base.langCode (code,descr) select 'c2', 'xxx'
insert into base.langCode (code,descr,isActive) select 'c3', 'xxx', 0
insert into base.langCode (code,descr) select 'c4', 'xxx'
insert into base.langCode (code,descr) select 'c5', 'xxx'
insert into base.langCode (code,descr) select 'c6', 'xxx'
insert into base.langCode (code,descr) select 'c7', 'xxx'
insert into base.langCode (code,descr) select 'c8', 'xxx'

DROP TABLE IF EXISTS zzImportLabelsBaseEn;
DROP TABLE IF EXISTS zzImportLabelsBaseDe;
DROP TABLE IF EXISTS zzLangLabel;

--Base Labels
CREATE TABLE zzImportLabelsBaseEn (
    langKey   NVARCHAR (100)  NOT NULL,
	label       NVARCHAR (MAX)  NOT NULL,
	tooltip  NVARCHAR (MAX)  NULL
);
CREATE TABLE zzImportLabelsBaseDe (
    langKey   NVARCHAR (100)  NOT NULL,
	label       NVARCHAR (MAX)  NOT NULL,
	tooltip  NVARCHAR (MAX)  NULL
);

BULK INSERT zzImportLabelsBaseEn
FROM 'C:\src\f2f\db\Labels\BaseEn.txt'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 1
);

BULK INSERT zzImportLabelsBaseDe
FROM 'C:\src\f2f\db\Labels\BaseDe.txt'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 1
);

UPDATE zzImportLabelsBaseEn SET label = REPLACE (label, '||', ',') WHERE label LIKE '%||%';
UPDATE zzImportLabelsBaseEn SET tooltip = REPLACE (tooltip, '||', ',') WHERE tooltip LIKE '%||%';
UPDATE zzImportLabelsBaseDe SET label = REPLACE (label, '||', ',') WHERE label LIKE '%||%';
UPDATE zzImportLabelsBaseDe SET tooltip = REPLACE (tooltip, '||', ',') WHERE tooltip LIKE '%||%';

INSERT INTO base.langKey (code) SELECT DISTINCT langKey FROM zzImportLabelsBaseEn;

SELECT 0 AS langKeyId
		,'en' AS langCode
		,langKey
		,label AS code
		,tooltip
	INTO zzLangLabel
	FROM zzImportLabelsBaseEn;

INSERT INTO zzLangLabel (langKeyId,langCode,langKey,code,tooltip)
	SELECT 0 AS langKeyId
		,'de' AS langCode
		,langKey
		,label AS code
		,tooltip
	FROM zzImportLabelsBaseDe;

	
UPDATE zzLangLabel SET langKeyId = 
	(SELECT l.id FROM base.langKey l
	 WHERE l.code = zzLangLabel.langKey)

--select * from zzImportLabelsBase


INSERT INTO base.langLabel (langKeyId,langCode,code,tooltip)
	SELECT DISTINCT langKeyId,langCode,code,tooltip FROM zzLangLabel;

DELETE FROM zImportLabels WHERE code IN (SELECT code FROM base.langKey)

--mff Labels
DROP TABLE IF EXISTS zzLangLabel;

INSERT INTO base.langKey (code)
	SELECT DISTINCT Code FROM zImportLabels;


SELECT 0 AS langKeyId
		,Lang AS langCode
		,Code AS langKey
		,Descr AS code
		,Tooltip AS tooltip
	INTO zzLangLabel
	FROM zImportLabels;
	
UPDATE zzLangLabel SET langKeyId = 
	(SELECT l.id FROM base.langKey l
	 WHERE l.code = zzLangLabel.langKey)

INSERT INTO base.langLabel (langKeyId,langCode,code,tooltip)
	SELECT DISTINCT langKeyId,langCode,code,tooltip FROM zzLangLabel;

UPDATE base.langLabel SET Tooltip = null WHERE Tooltip = 'NULL';

DROP TABLE IF EXISTS zzImportLabelsBaseEn;
DROP TABLE IF EXISTS zzImportLabelsBaseDe;
DROP TABLE IF EXISTS zzLangLabel;

SELECT 'Keys', Count(*) FROM base.langKey;

/*
SELECT k.code, l.*
	FROM base.langKey k
	LEFT JOIN base.langLabel l on l.langKeyId = k.id
	ORDER BY k.code 
*/