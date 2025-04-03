INSERT INTO _cntrl.permission (id,code,descr,crud,org_nr,updated) VALUES (10000,'org',      'Read Only Organisations','-R--',0,current_timestamp);
INSERT INTO _cntrl.permission (id,code,descr,crud,org_nr,updated) VALUES (10001,'plan/fix', 'Fixing Simulations',     '*',   0,current_timestamp);

INSERT INTO _cntrl.role (id,code,org_nr,updated,updated_userid) VALUES (10000,'Fix',0,current_timestamp,1);

INSERT INTO _cntrl.role_permission (id,role_id,permission_id,org_nr,updated,updated_userid) VALUES (10000,10000,10001,1,current_timestamp,1);


INSERT INTO reftype (id,code,descr,org_nr,updated,updated_userid) VALUES (10001,'country','Country',1,current_timestamp,1);
INSERT INTO reftype (id,code,descr,org_nr,updated,updated_userid) VALUES (10002,'currency','Currency',1,current_timestamp,1);

INSERT INTO country (id,reftype_id,sort,code,descr,image,org_nr,updated,updated_userid) VALUES (10010,10001,1,'nz','New Zealand',   'nz.png',1,current_timestamp,1);
INSERT INTO country (id,reftype_id,sort,code,descr,image,org_nr,updated,updated_userid) VALUES (10011,10001,2,'au','Australia',     'au.png',1,current_timestamp,1);
INSERT INTO country (id,reftype_id,sort,code,descr,image,org_nr,updated,updated_userid,active) VALUES (10012,10001,3,'uk','United Kingdom','uk.png',1,current_timestamp,1,false);

INSERT INTO currency (id,reftype_id,sort,code,descr,org_nr,updated,updated_userid) VALUES (10020,10002,3,'NZD','New Zealand Dollar' ,1,current_timestamp,1);
INSERT INTO currency (id,reftype_id,sort,code,descr,org_nr,updated,updated_userid) VALUES (10021,10002,2,'AUD','Australian Dollar',  1,current_timestamp,1);
INSERT INTO currency (id,reftype_id,sort,code,descr,org_nr,updated,updated_userid) VALUES (10022,10002,1,'GBP','Great Britain Pound',1,current_timestamp,1);
