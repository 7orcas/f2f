BEGIN TRANSACTION;

SET IDENTITY_INSERT base.org ON;
INSERT INTO base.org (id, code, descr) VALUES (0, 'Org Base', 'Base Organisation');
INSERT INTO base.org (id, code, descr,langCode,encoded) VALUES (1, 'Org 1', 'Org 1 Description','en','{Languages:[{LangCode:"en",IsEditable:true},{LangCode:"de",IsEditable:true},{LangCode:"c1",IsEditable:false},{LangCode:"c2",IsEditable:false}]}');
INSERT INTO base.org (id, code, descr,langCode,langLabelVariant,encoded) VALUES (2, 'Org 2', 'Org 2 Description','en',2,'{Languages:[{LangCode:"de",IsEditable:true}]}');
INSERT INTO base.org (id, code, descr,langCode) VALUES (3, 'Org 3', 'Org 3 Description','de');
SET IDENTITY_INSERT base.org OFF;


--{IsLangCodeEditable:false,Languages:["en","de","c1","c2"]}
--{IsLangCodeEditable:true,Languages:["de","c1","c2"]}
--{Languages:[{LangCode:"en",IsEditable:true},{LangCode:"de",IsEditable:true},{LangCode:"c1",IsEditable:false},{LangCode:"c2",IsEditable:false}]}
--{Languages:[{LangCode:"de",IsEditable:true}]}

SET IDENTITY_INSERT base.zzz ON;
INSERT INTO base.zzz (id, xxx, yyy, orgs,langCode) VALUES (1, '1', '1', '0,1,2','de');
INSERT INTO base.zzz (id, xxx, yyy, orgs,langCode) VALUES (2, 'user', 'xx123', '1,2,12','en');
SET IDENTITY_INSERT base.zzz OFF;

SET IDENTITY_INSERT base.permission ON;
INSERT INTO base.permission (id, code, descr) VALUES (10,  'label',         'Language Labels');
INSERT INTO base.permission (id, code, descr) VALUES (20,  'profs',         'Professionals');
INSERT INTO base.permission (id, code, descr) VALUES (30,  'org',            'Organisations');
INSERT INTO base.permission (id, code, descr) VALUES (40,  'perms',       'Permissions');
INSERT INTO base.permission (id, code, descr) VALUES (50,  'role',            'User Roles');
INSERT INTO base.permission (id, code, descr) VALUES (60,  'user',           'Users');
INSERT INTO base.permission (id, code, descr) VALUES (70,  'ref',              'References');
INSERT INTO base.permission (id, code, descr) VALUES (80,  'machine',   'Machines');
INSERT INTO base.permission (id, code, descr) VALUES (90,  'audit',         'Audit');
--INSERT INTO base.permission (id, code, descr) VALUES (100,  'lang',         'Lang');
SET IDENTITY_INSERT base.permission OFF;


SET IDENTITY_INSERT base.role ON;
INSERT INTO base.role (id, orgId, code, descr) VALUES (1, 0, 'Admin', 'Full Admin Access');
INSERT INTO base.role (id, orgId, code) VALUES (2, 0, 'Org RO');
INSERT INTO base.role (id, orgId, code) VALUES (3, 0, 'LangEdit');
INSERT INTO base.role (id, orgId, code) VALUES (4, 0, 'Machines0');
INSERT INTO base.role (id, orgId, code) VALUES (5, 1, 'Machines1');
INSERT INTO base.role (id, orgId, code) VALUES (6, 2, 'Machines2');
SET IDENTITY_INSERT base.role OFF;

SET IDENTITY_INSERT base.rolePermission ON;

INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (1, 1, 10, 'crud');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (2, 1, 20, 'crud');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (3, 1, 30, 'r');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (4, 1, 40, 'cru');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (5, 1, 50, 'crud');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (6, 1, 60, 'crud');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (7, 1, 70, 'crud');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (8, 1, 80, 'cd');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (9, 4, 80, 'd');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (10, 5, 80, 'ur');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (11, 6, 80, 'rd');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (12, 1, 90, 'r');
--INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (13, 1, 100, 'r');

SET IDENTITY_INSERT base.rolePermission OFF;

SET IDENTITY_INSERT base.zzzRole ON;
INSERT INTO base.zzzRole (id, zzzId, roleId) VALUES (1, 2,1);
INSERT INTO base.zzzRole (id, zzzId, roleId) VALUES (2, 2, 2);
INSERT INTO base.zzzRole (id, zzzId, roleId) VALUES (3, 2, 3);
INSERT INTO base.zzzRole (id, zzzId, roleId) VALUES (4, 2, 4);
INSERT INTO base.zzzRole (id, zzzId, roleId) VALUES (5, 2, 5);
INSERT INTO base.zzzRole (id, zzzId, roleId) VALUES (6, 2, 6);
SET IDENTITY_INSERT base.zzzRole OFF;


COMMIT;