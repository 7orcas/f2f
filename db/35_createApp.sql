

CREATE TABLE app.machineGroup (
    id                     INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	orgId                  INT             NOT NULL,
	code                   NVARCHAR (100)  NOT NULL,
	descr                  NVARCHAR (MAX)  NULL,
	encoded                NVARCHAR (MAX)  NULL,
	updated                DATETIME        NOT NULL DEFAULT GETDATE(),
	FOREIGN KEY (orgId)    REFERENCES      base.org(id)
);
CREATE TABLE app.machine (
    id                     INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	orgId                  INT             NOT NULL,
	classification         INT             NULL,
	machineGroupId         INT             NULL,
	stationPairs           INT             NOT NULL,
	code                   NVARCHAR (100)  NOT NULL,
	descr                  NVARCHAR (MAX)  NULL,
	encoded                NVARCHAR (MAX)  NULL,
	updated                DATETIME        NOT NULL DEFAULT GETDATE(),
	FOREIGN KEY (orgId)    REFERENCES      base.org(id),
	FOREIGN KEY (machineGroupId) REFERENCES app.machineGroup(id)
);


/*
create table country
(
	id bigserial primary key,
	image varchar
) INHERITS (app.baseref);
alter table country OWNER to postgres;
alter sequence country_id_seq restart with 10000;

create table currency
(
	id bigserial primary key	
) INHERITS (app.baseref);
alter table currency OWNER to postgres;
alter sequence currency_id_seq restart with 10000;
*/