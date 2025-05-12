BEGIN TRANSACTION;

SET IDENTITY_INSERT base.org ON;
INSERT INTO base.org (id, nr, code, descr) VALUES (0, 0, 'Org Base', 'Base Orgisation');
INSERT INTO base.org (id, nr, code, descr) VALUES (1, 1, 'Org 1', 'Org 1 Description');
INSERT INTO base.org (id, nr, code, descr) VALUES (2, 2, 'Org 2', 'Org 2 Description');
INSERT INTO base.org (id, nr, code, descr) VALUES (3, 3, 'Org 3', 'Org 3 Description');
SET IDENTITY_INSERT base.org OFF;


SET IDENTITY_INSERT base.zzz ON;
INSERT INTO base.zzz (id, xxx, yyy, orgs) VALUES (1, '1', '1', '0,1,2');
INSERT INTO base.zzz (id, xxx, yyy, orgs) VALUES (2, 'user', 'password', '1,2,12');
SET IDENTITY_INSERT base.zzz OFF;

SET IDENTITY_INSERT base.permission ON;
INSERT INTO base.permission (id, code, descr) VALUES (10,  'lang',          'Language Labels');
INSERT INTO base.permission (id, code, descr) VALUES (20,  'profs',         'Professionals');
INSERT INTO base.permission (id, code, descr) VALUES (30,  'org',            'Organisations');
INSERT INTO base.permission (id, code, descr) VALUES (40,  'perms',       'Permissions');
INSERT INTO base.permission (id, code, descr) VALUES (50,  'role',            'User Roles');
INSERT INTO base.permission (id, code, descr) VALUES (60,  'user',           'Users');
INSERT INTO base.permission (id, code, descr) VALUES (70,  'ref',              'References');
INSERT INTO base.permission (id, code, descr) VALUES (80,  'machine',   'Machines');
INSERT INTO base.permission (id, code, descr) VALUES (90,  'audit',         'Audit');
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
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (11, 6, 80, 'd');
INSERT INTO base.rolePermission (id, roleId, permissionId, crud) VALUES (12, 1, 90, 'r');

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