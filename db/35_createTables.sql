

CREATE TABLE machineGroup (
    id                 INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
);
CREATE TABLE machine (
    id                 INT             PRIMARY KEY IDENTITY (10000, 1) NOT NULL,
	machineGroupId     INT             NULL,
	FOREIGN KEY (machineGroupId) REFERENCES machineGroup(id)
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