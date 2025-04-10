BEGIN TRANSACTION;

SET IDENTITY_INSERT base.languageCode ON;
INSERT INTO base.languageCode (id, code, descr) VALUES (1, 'en', 'English');
INSERT INTO base.languageCode (id, code, descr) VALUES (2, 'de', 'Deutsch');
INSERT INTO base.languageCode (id, code, descr) VALUES (3, 'es', 'Espanol');
SET IDENTITY_INSERT base.languageCode OFF;


COMMIT;