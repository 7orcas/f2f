--Do individually
/*
CREATE SCHEMA _base;
CREATE SCHEMA _cntrl;
*/

CREATE SEQUENCE _cntrl.temp_id
    AS BIGINT
    START WITH 10000
    INCREMENT BY 1;

CREATE TABLE _cntrl.token (
    id         INT             PRIMARY KEY NOT NULL,
	token      NVARCHAR (MAX)  NOT NULL,
	updated    DATETIME        NOT NULL
);

CREATE TABLE _base.org (
    id         INT             PRIMARY KEY NOT NULL,
	code       NVARCHAR (100)  NOT NULL,
	descr      NVARCHAR (MAX)  NOT NULL
);
CREATE TABLE _base.entity (
	_orgId     INT             NOT NULL,
    id         INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	descrim    NVARCHAR (100)  NOT NULL,
	code       NVARCHAR (100)  NOT NULL,
	descr      NVARCHAR (MAX)  NULL,
	encoded    NVARCHAR (MAX)  NULL,
	updated    DATETIME        NOT NULL DEFAULT GETDATE(),
	FOREIGN KEY (_orgId) REFERENCES _base.org(id)
);
CREATE TABLE _base.label (
    id         INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	lang       NVARCHAR (4)    NOT NULL,
	code       NVARCHAR (100)  NOT NULL,
	_orgId     INT             NULL,
	descr      NVARCHAR (MAX)  NOT NULL,
	tooltip    NVARCHAR (MAX)  NULL,
	encoded    NVARCHAR (MAX)  NULL,
	updated    DATETIME        NOT NULL DEFAULT GETDATE(),
	FOREIGN KEY (_orgId) REFERENCES _base.org(id),
	CONSTRAINT label_uq_code UNIQUE (lang, code, _orgId)
);
CREATE TABLE _base.zzz
(
	id         INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	xxx        NVARCHAR (4)    NOT NULL UNIQUE,
	yyy        NVARCHAR (4)    NOT NULL,
	_orgs      NVARCHAR (MAX)  NULL,
 	attempts   INT             NULL DEFAULT (0),
 	lastlogin  DATETIME        NOT NULL
);
CREATE TABLE _base.permission
(
	id         INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	crud       NVARCHAR (MAX)  NULL
);
CREATE TABLE _base.role
(
	id         INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL
);
CREATE TABLE _base.rolePermission
(
	id           INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
    roleId       INT             NOT NULL,
    permissionId INT             NOT NULL,
	FOREIGN KEY (roleId) REFERENCES _base.role(id),
	FOREIGN KEY (permissionId) REFERENCES _base.permission(id),
	CONSTRAINT rolePermission_uq_role_persmission UNIQUE (roleId, permissionId)
);
CREATE TABLE _base.zzzRole
(
	id           INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	zzzId        INT             NOT NULL,
	roleId       INT             NOT NULL,
	FOREIGN KEY (zzzId) REFERENCES _base.zzz(id),
	FOREIGN KEY (roleId) REFERENCES _base.role(id),
	CONSTRAINT zzzRole_uq_zzz_role UNIQUE (zzzId, roleId)
);
CREATE TABLE _base.lang
(
	id           INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL
);
CREATE TABLE _base.langKey
(
	id           INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	pack         NVARCHAR (MAX)  NULL
);
CREATE TABLE _base.langLabel
(
	id           INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	langKeyId    INT             NOT NULL,
    lang		 NVARCHAR (MAX)  NOT NULL,
	FOREIGN KEY (langKeyId) REFERENCES _base.langKey(id),
);

/*
create table reftype
(
	id bigserial primary key
) INHERITS (_app.base);
alter table reftype OWNER to postgres;
alter sequence reftype_id_seq restart with 10000;

create table _app.baseref
(
	sort integer default 0,
	dvalue boolean default false,
	reftype_id bigint references reftype (id)
) INHERITS (_app.base);
alter table _app.baseref OWNER to postgres;

*/

