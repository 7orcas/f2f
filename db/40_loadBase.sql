SET IDENTITY_INSERT _base.org ON;
INSERT INTO _base.org (id, nr, code, descr) VALUES (1, 1, 'Org 1', 'Org 1 Description');
INSERT INTO _base.org (id, nr, code, descr) VALUES (2, 2, 'Org 2', 'Org 2 Description');
INSERT INTO _base.org (id, nr, code, descr) VALUES (3, 3, 'Org 3', 'Org 3 Description');
SET IDENTITY_INSERT _base.org OFF;


SET IDENTITY_INSERT _base.zzz ON;
INSERT INTO _base.zzz (id, xxx, yyy, orgs) VALUES (1, '1', '1', '0');
INSERT INTO _base.zzz (id, xxx, yyy, orgs) VALUES (2, 'user', 'password', '1,2,12');
SET IDENTITY_INSERT _base.zzz OFF;


--Unit tests rely on existance of permission id = 10
--INSERT INTO _base.permission (id, code, descr, crud, org_nr, updated) VALUES (10,  'lang',           'Language Labels',      '*', 0, current_timestamp);
--INSERT INTO _base.permission (id, code, descr, crud, org_nr, updated) VALUES (20,  '*/listcache',    'Login cache (server)', '*', 0, current_timestamp);
--INSERT INTO _base.permission (id, code, descr, crud, org_nr, updated) VALUES (30,  'org',            'Organisations',        '*', 0, current_timestamp);
--INSERT INTO _base.permission (id, code, descr, crud, org_nr, updated) VALUES (40,  'permission',     'Permissions',          '*', 0, current_timestamp);
--INSERT INTO _base.permission (id, code, descr, crud, org_nr, updated) VALUES (50,  'role',           'User Roles',           '*', 0, current_timestamp);
--INSERT INTO _base.permission (id, code, descr, crud, org_nr, updated) VALUES (60,  'user',           'Users',                '*', 0, current_timestamp);
--INSERT INTO _base.permission (id, code, descr, crud, org_nr, updated) VALUES (70,  'ref',            'References',           '*', 0, current_timestamp);

--INSERT INTO _base.permission (id, code, descr, crud, org_nr, updated) VALUES (500,  'org',           'Organisations RO',     '-R--', 0, current_timestamp);
--INSERT INTO _base.permission (id, code, descr, crud, org_nr, updated) VALUES (501,  'lang',          'Labels RO',            '-R--', 0, current_timestamp);

--INSERT INTO _base.role (id, code, org_nr, updated, updated_userid) VALUES (1, 'Admin', 0, current_timestamp, 1);
--INSERT INTO _base.role (id, code, org_nr, updated, updated_userid) VALUES (2, 'Org RO', 0, current_timestamp, 1);
--INSERT INTO _base.role (id, code, org_nr, updated, updated_userid) VALUES (3, 'LangEdit', 0, current_timestamp, 1);

--role admin
--INSERT INTO _base.role_permission (id, role_id, permission_id, org_nr, updated, updated_userid) VALUES (1, 1, 10, 0, current_timestamp, 1);
--INSERT INTO _base.role_permission (id, role_id, permission_id, org_nr, updated, updated_userid) VALUES (2, 1, 20, 0, current_timestamp, 1);
--INSERT INTO _base.role_permission (id, role_id, permission_id, org_nr, updated, updated_userid) VALUES (3, 1, 30, 0, current_timestamp, 1);
--INSERT INTO _base.role_permission (id, role_id, permission_id, org_nr, updated, updated_userid) VALUES (4, 1, 40, 0, current_timestamp, 1);
--INSERT INTO _base.role_permission (id, role_id, permission_id, org_nr, updated, updated_userid) VALUES (5, 1, 50, 0, current_timestamp, 1);
--INSERT INTO _base.role_permission (id, role_id, permission_id, org_nr, updated, updated_userid) VALUES (6, 1, 60, 0, current_timestamp, 1);
--INSERT INTO _base.role_permission (id, role_id, permission_id, org_nr, updated, updated_userid) VALUES (7, 1, 70, 0, current_timestamp, 1);
--role others
--INSERT INTO _base.role_permission (id, role_id, permission_id, org_nr, updated, updated_userid) VALUES (100, 2, 500, 0, current_timestamp, 1);
--INSERT INTO _base.role_permission (id, role_id, permission_id, org_nr, updated, updated_userid) VALUES (101, 3, 501, 0, current_timestamp, 1);

