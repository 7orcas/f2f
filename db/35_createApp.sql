

CREATE TABLE _app.machineGroup (
    id                     INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	orgId                  INT             NOT NULL,
	code                   NVARCHAR (100)  NOT NULL,
	descr                  NVARCHAR (MAX)  NULL,
	encoded                NVARCHAR (MAX)  NULL,
	updated                DATETIME        NOT NULL DEFAULT GETDATE(),
	FOREIGN KEY (orgId)    REFERENCES      _base.org(id)
);
CREATE TABLE _app.machine (
    id                     INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	orgId                  INT             NOT NULL,
	machineGroupId         INT             NULL,
	stationPairs           INT             NOT NULL,
	code                   NVARCHAR (100)  NOT NULL,
	descr                  NVARCHAR (MAX)  NULL,
	encoded                NVARCHAR (MAX)  NULL,
	updated                DATETIME        NOT NULL DEFAULT GETDATE(),
	FOREIGN KEY (orgId)    REFERENCES      _base.org(id),
	FOREIGN KEY (machineGroupId) REFERENCES _app.machineGroup(id)
);


/*
create table country
(
	id bigserial primary key,
	image varchar
) INHERITS (_app.baseref);
alter table country OWNER to postgres;
alter sequence country_id_seq restart with 10000;

create table currency
(
	id bigserial primary key	
) INHERITS (_app.baseref);
alter table currency OWNER to postgres;
alter sequence currency_id_seq restart with 10000;
*/