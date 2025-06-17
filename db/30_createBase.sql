--Do individually
/*
CREATE SCHEMA app;
CREATE SCHEMA base;
CREATE SCHEMA cntrl;
*/

CREATE SEQUENCE cntrl.temp_id
    AS BIGINT
    START WITH 10000
    INCREMENT BY 1;

/* DELETE
CREATE TABLE cntrl.token (
    id         INT             PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	token      NVARCHAR (MAX)  NOT NULL,
	expires    DATETIME        NOT NULL
);
*/

CREATE TABLE base.org (
    id          BIGINT             PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	nr          INT             NOT NULL  UNIQUE,
	code        NVARCHAR (100)  NOT NULL,
	descr       NVARCHAR (MAX)  NOT NULL,
	encoded     NVARCHAR (MAX)  NULL,
	updated     DATETIME        NOT NULL DEFAULT GETDATE(),
	isActive    BIT             NOT NULL DEFAULT 1,
	langCode  NVARCHAR (4)    NULL,
	langLabelVariant  INT             NULL
);
CREATE TABLE base.zzz
(
	id             BIGINT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	xxx            NVARCHAR (40)   NOT NULL UNIQUE,
	yyy            NVARCHAR (100)  NOT NULL,
	orgs           NVARCHAR (MAX) NOT  NULL,
	langCode       NVARCHAR (4)  NULL,
 	attempts       INT             NULL DEFAULT (0),
 	lastlogin      DATETIME        NOT NULL DEFAULT GETDATE(),
	classification INT             NULL DEFAULT (0),
	isAdmin        BIT             NOT NULL DEFAULT 0,
	isActive       BIT             NOT NULL DEFAULT 1
);
CREATE TABLE base.permission
(
	id          BIGINT             PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	code        NVARCHAR (100)  NOT NULL,
	descr       NVARCHAR (MAX)  NULL,
	encoded     NVARCHAR (MAX)  NULL,
	updated     DATETIME        NOT NULL DEFAULT GETDATE(),
	isActive    BIT             NOT NULL DEFAULT 1,
	CONSTRAINT permission_uq_code UNIQUE (code)
);
CREATE TABLE base.role
(
	id          BIGINT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	orgId       BIGINT            NOT NULL,
	code        NVARCHAR (100)  NOT NULL,
	descr       NVARCHAR (MAX)  NULL,
	encoded     NVARCHAR (MAX)  NULL,
	updated     DATETIME        NOT NULL DEFAULT GETDATE(),
	isActive    BIT             NOT NULL DEFAULT 1,
	FOREIGN KEY (orgId)    REFERENCES      base.org(id),
	CONSTRAINT role_uq_code UNIQUE (orgId, code)
);
CREATE TABLE base.rolePermission
(
	id           BIGINT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
    roleId       BIGINT           NOT NULL,
    permissionId BIGINT             NOT NULL,
	crud         NVARCHAR (10)   NULL,
	updated     DATETIME         NOT NULL DEFAULT GETDATE(),
	isActive    BIT              NOT NULL DEFAULT 1,
	FOREIGN KEY (roleId)         REFERENCES base.role(id),
	FOREIGN KEY (permissionId)   REFERENCES base.permission(id),
	CONSTRAINT rolePermission_uq_role_persmission UNIQUE (roleId, permissionId)
);
CREATE TABLE base.zzzRole
(
	id           BIGINT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	zzzId        BIGINT             NOT NULL,
	roleId       BIGINT             NOT NULL,
	updated      DATETIME        NOT NULL DEFAULT GETDATE(),
	isActive     BIT             NOT NULL DEFAULT 1,
	FOREIGN KEY (zzzId)          REFERENCES base.zzz(id),
	FOREIGN KEY (roleId)         REFERENCES base.role(id),
	CONSTRAINT zzzRole_uq_zzz_role UNIQUE (zzzId, roleId)
);
CREATE TABLE base.langCode
(
	id         BIGINT             PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	code       NVARCHAR (4)    NOT NULL,
	descr      NVARCHAR (MAX)  NOT NULL,
	encoded    NVARCHAR (MAX)  NULL,
	updated    DATETIME        NOT NULL DEFAULT GETDATE(),
	isActive   BIT             NOT NULL DEFAULT 1
);
CREATE TABLE base.langKey
(
	id         BIGINT             PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	pack       NVARCHAR (MAX)  NULL,
	code       NVARCHAR (100)  NOT NULL,
	descr      NVARCHAR (MAX)  NULL,
	encoded    NVARCHAR (MAX)  NULL,
	updated    DATETIME        NOT NULL DEFAULT GETDATE(),
);
CREATE TABLE base.langLabel
(
	id             BIGINT             PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	langKeyId  BIGINT             NOT NULL,
    langCode   NVARCHAR (4)    NOT NULL,
	variant    INT             NULL,
	code           NVARCHAR (MAX)  NOT NULL,
	tooltip        NVARCHAR (MAX)  NULL,
	encoded        NVARCHAR (MAX)  NULL,
	updated        DATETIME        NOT NULL DEFAULT GETDATE(),
	FOREIGN KEY (langKeyId) REFERENCES base.langKey(id),
	CONSTRAINT langLabel_uq_code   UNIQUE (langKeyId, langCode, variant)
);
CREATE TABLE base.audit (
    id                       BIGINT                 PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	orgId                 BIGINT                 NOT NULL,
	source              INT                 NOT NULL,
	entityTypeId    INT                 NOT NULL,
	entityId            BIGINT                 NULL,
	userId               BIGINT                 NOT NULL,
	created            DATETIME    NOT NULL DEFAULT GETDATE(),
	crud                  NVARCHAR (10)   NULL,
	details             NVARCHAR (MAX)  NULL
);
