insert into _cntrl.lang (id, code, descr, org_nr, updated) values (1, 'en', 'English', 0, NOW());
insert into _cntrl.lang (id, code, descr, org_nr, updated) values (2, 'de', 'Deutsch', 0, NOW());
insert into _cntrl.lang (id, code, descr, org_nr, updated) values (3, 'es', 'Espanol', 0, NOW());

create table _app._lang
(
  code       varchar,
  pack       varchar,
  org_nr     int,
  en         varchar,
  de         varchar
);

\copy _app._lang from 'C:/src/blue/db/labels.csv' with DELIMITER ','  CSV  ENCODING 'UTF-8';  

create table _app._langKey
(
  id         bigint,
  code       varchar,
  pack       varchar
);
insert into _app._langKey (code, pack) select distinct code, pack from _app._lang;
update _app._langKey set id = NEXTVAL('_cntrl.lang_key_id_seq');

alter table _app._lang add column lang_key_id bigint;
alter table _app._lang add column en_id bigint;
alter table _app._lang add column de_id bigint;
update _app._lang as l set lang_key_id = (select id from _app._langKey as k where l.code = k.code);
update _app._lang set en_id = NEXTVAL('_cntrl.lang_label_id_seq');
update _app._lang set de_id = NEXTVAL('_cntrl.lang_label_id_seq'); --need to repeat new id's for each language

insert into _cntrl.lang_key (id, code, pack, org_nr, updated) select id, code, pack, 0, NOW() from _app._langkey;
insert into _cntrl.lang_label (lang_key_id, id, lang, code, org_nr, updated) select lang_key_id, en_id, 'en', en, org_nr, NOW() from _app._lang where length(en) > 0;
insert into _cntrl.lang_label (lang_key_id, id, lang, code, org_nr, updated) select lang_key_id, de_id, 'de', de, org_nr, NOW() from _app._lang where length(de) > 0;

drop table if exists _app._lang;
drop table if exists _app._langKey;