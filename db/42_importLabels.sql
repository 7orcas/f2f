DELETE FROM base.langCode;
DELETE FROM base.langLabel;
DELETE FROM base.langKey;

INSERT INTO base.langCode (code,descr) VALUES ('en','English');
INSERT INTO base.langCode (code,descr) VALUES ('de','Deutsch');
INSERT INTO base.langCode (code,descr) VALUES ('it','Italian');
INSERT INTO base.langCode (code,descr) VALUES ('es',N'Español');
INSERT INTO base.langCode (code,descr) VALUES ('ma','Maori');

INSERT INTO base.langKey (code)
	SELECT DISTINCT Code FROM zImportLabels;

DROP TABLE IF EXISTS zzLangLabel;
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

DROP TABLE IF EXISTS zzLangLabel;

/*
SELECT k.code, l.*
	FROM base.langKey k
	LEFT JOIN base.langLabel l on l.langKeyId = k.id
	ORDER BY k.code 
*/