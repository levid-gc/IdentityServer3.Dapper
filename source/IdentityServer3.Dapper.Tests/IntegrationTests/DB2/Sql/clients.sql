﻿DROP TABLE DB2ADMIN.CLIENTCLAIMS;
DROP TABLE DB2ADMIN.CLIENTSECRETS;
DROP TABLE DB2ADMIN.CLIENTCUSTOMGRANTTYPES;
DROP TABLE DB2ADMIN.CLIENTIDPRESTRICTIONS;
DROP TABLE DB2ADMIN.CLIENTPOSTLOGOUTREDIRECTURIS;
DROP TABLE DB2ADMIN.CLIENTREDIRECTURIS;
DROP TABLE DB2ADMIN.CLIENTSCOPES;
DROP TABLE DB2ADMIN.CLIENTCORSORIGINS;
DROP TABLE DB2ADMIN.CLIENTS;

CREATE TABLE DB2ADMIN.CLIENTS (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    ENABLED SMALLINT NOT NULL,
    CLIENTID VARCHAR(200) NOT NULL,
    CLIENTNAME VARCHAR(200) NOT NULL,
    CLIENTURI VARCHAR(2000),
    LOGOURI VARCHAR(15000),
    REQUIRECONSENT SMALLINT NOT NULL,
    ALLOWREMEMBERCONSENT SMALLINT NOT NULL,
    FLOW INT NOT NULL,
    IDENTITYTOKENLIFETIME INT NOT NULL,
    ACCESSTOKENLIFETIME INT NOT NULL,
    AUTHORIZATIONCODELIFETIME INT NOT NULL,
    ABSOLUTEREFRESHTOKENLIFETIME INT NOT NULL,
    SLIDINGREFRESHTOKENLIFETIME INT NOT NULL,
    REFRESHTOKENUSAGE INT NOT NULL,
    REFRESHTOKENEXPIRATION INT NOT NULL,
    ACCESSTOKENTYPE INT NOT NULL,
    ENABLELOCALLOGIN SMALLINT NOT NULL,
    INCLUDEJWTID SMALLINT NOT NULL,
    ALWAYSSENDCLIENTCLAIMS SMALLINT NOT NULL,
    PREFIXCLIENTCLAIMS SMALLINT NOT NULL,
    ALLOWACCESSTOALLSCOPES SMALLINT NOT NULL,
    ALLOWACCESSTOALLGRANTTYPES SMALLINT NOT NULL,
    ALLOWCLIENTCREDENTIALSONLY SMALLINT NOT NULL,
    UPDATEACCESSTOKENONREFRESH SMALLINT NOT NULL,
    LOGOUTURI VARCHAR(15000),
    LOGOUTSESSIONREQUIRED SMALLINT NOT NULL,
    REQUIRESIGNOUTPROMPT SMALLINT NOT NULL,
    ALLOWACCESSTOKENSVIABROWSER SMALLINT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.CLIENTS" PRIMARY KEY ("ID")
);
CREATE UNIQUE INDEX IX_CLIENTS_CLIENTID ON DB2ADMIN.CLIENTS("CLIENTID");

CREATE TABLE DB2ADMIN.CLIENTCLAIMS (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    TYPE VARCHAR(250) NOT NULL,
    VALUE VARCHAR(250) NOT NULL,
    CLIENT_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.CLIENTCLAIMS" PRIMARY KEY ("ID")
);
CREATE INDEX IX_CLIENTCLAIMS_CLIENT_ID ON DB2ADMIN.CLIENTCLAIMS("CLIENT_ID");

CREATE TABLE DB2ADMIN.CLIENTSECRETS (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    VALUE VARCHAR(250) NOT NULL,
    TYPE VARCHAR(250),
    DESCRIPTION VARCHAR(2000),
    EXPIRATION TIMESTAMP,
    CLIENT_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.CLIENTSECRETS" PRIMARY KEY ("ID")
);
CREATE INDEX IX_CLIENTSECRETS_CLIENT_ID ON DB2ADMIN.CLIENTSECRETS("CLIENT_ID");

CREATE TABLE DB2ADMIN.CLIENTCUSTOMGRANTTYPES (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    GRANTTYPE VARCHAR(250) NOT NULL,
    CLIENT_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.CLIENTCUSTOMGRANTTYPES" PRIMARY KEY ("ID")
);
CREATE INDEX IX_CLIENTCUSTOMGRANTTYPES_CLIENT_ID ON DB2ADMIN.CLIENTCUSTOMGRANTTYPES("CLIENT_ID");

CREATE TABLE DB2ADMIN.CLIENTIDPRESTRICTIONS (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    PROVIDER VARCHAR(200) NOT NULL,
    CLIENT_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.CLIENTIDPRESTRICTIONS" PRIMARY KEY ("ID")
);
CREATE INDEX IX_CLIENTIDPRESTRICTIONS_CLIENT_ID ON DB2ADMIN.CLIENTIDPRESTRICTIONS("CLIENT_ID");

CREATE TABLE DB2ADMIN.CLIENTPOSTLOGOUTREDIRECTURIS (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    URI VARCHAR(2000) NOT NULL,
    CLIENT_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.CLIENTPOSTLOGOUTREDIRECTURIS" PRIMARY KEY ("ID")
);
CREATE INDEX IX_CLIENTPOSTLOGOUTREDIRECTURIS_CLIENT_ID ON DB2ADMIN.CLIENTPOSTLOGOUTREDIRECTURIS("CLIENT_ID");

CREATE TABLE DB2ADMIN.CLIENTREDIRECTURIS (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    URI VARCHAR(2000) NOT NULL,
    CLIENT_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.CLIENTREDIRECTURIS" PRIMARY KEY ("ID")
);
CREATE INDEX IX_CLIENTREDIRECTURIS_CLIENT_ID ON DB2ADMIN.CLIENTREDIRECTURIS("CLIENT_ID");

CREATE TABLE DB2ADMIN.CLIENTSCOPES (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    SCOPE VARCHAR(200) NOT NULL,
    CLIENT_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.CLIENTSCOPES" PRIMARY KEY ("ID")
);
CREATE INDEX IX_CLIENTSCOPES_CLIENT_ID ON DB2ADMIN.CLIENTSCOPES("CLIENT_ID");

CREATE TABLE DB2ADMIN.CLIENTCORSORIGINS (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    ORIGIN VARCHAR(150) NOT NULL,
    CLIENT_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.CLIENTCORSORIGINS" PRIMARY KEY ("ID")
);
CREATE INDEX IX_CLIENTCORSORIGINS_CLIENT_ID ON DB2ADMIN.CLIENTCORSORIGINS("CLIENT_ID");

ALTER TABLE DB2ADMIN.CLIENTCLAIMS ADD CONSTRAINT "FK_DB2ADMIN.CLIENTCLAIMS_DB2ADMIN.CLIENTS_CLIENT_ID" FOREIGN KEY ("CLIENT_ID") REFERENCES DB2ADMIN.CLIENTS("ID") ON DELETE CASCADE;
ALTER TABLE DB2ADMIN.CLIENTSECRETS ADD CONSTRAINT "FK_DB2ADMIN.CLIENTSECRETS_DB2ADMIN.CLIENTS_CLIENT_ID" FOREIGN KEY ("CLIENT_ID") REFERENCES DB2ADMIN.CLIENTS("ID") ON DELETE CASCADE;
ALTER TABLE DB2ADMIN.CLIENTCUSTOMGRANTTYPES ADD CONSTRAINT "FK_DB2ADMIN.CLIENTCUSTOMGRANTTYPES_DB2ADMIN.CLIENTS_CLIENT_ID" FOREIGN KEY ("CLIENT_ID") REFERENCES DB2ADMIN.CLIENTS("ID") ON DELETE CASCADE;
ALTER TABLE DB2ADMIN.CLIENTIDPRESTRICTIONS ADD CONSTRAINT "FK_DB2ADMIN.CLIENTIDPRESTRICTIONS_DB2ADMIN.CLIENTS_CLIENT_ID" FOREIGN KEY ("CLIENT_ID") REFERENCES DB2ADMIN.CLIENTS ("ID") ON DELETE CASCADE;
ALTER TABLE DB2ADMIN.CLIENTPOSTLOGOUTREDIRECTURIS ADD CONSTRAINT "FK_DB2ADMIN.CLIENTPOSTLOGOUTREDIRECTURIS_DB2ADMIN.CLIENTS_CLIENT_ID" FOREIGN KEY ("CLIENT_ID") REFERENCES DB2ADMIN.CLIENTS ("ID") ON DELETE CASCADE;
ALTER TABLE DB2ADMIN.CLIENTREDIRECTURIS ADD CONSTRAINT "FK_DB2ADMIN.CLIENTREDIRECTURIS_DB2ADMIN.CLIENTS_CLIENT_ID" FOREIGN KEY ("CLIENT_ID") REFERENCES DB2ADMIN.CLIENTS ("ID") ON DELETE CASCADE;
ALTER TABLE DB2ADMIN.CLIENTSCOPES ADD CONSTRAINT "FK_DB2ADMIN.CLIENTSCOPES_DB2ADMIN.CLIENTS_CLIENT_ID" FOREIGN KEY ("CLIENT_ID") REFERENCES DB2ADMIN.CLIENTS ("ID") ON DELETE CASCADE;
ALTER TABLE DB2ADMIN.CLIENTCORSORIGINS ADD CONSTRAINT "FK_DB2ADMIN.CLIENTCORSORIGINS_DB2ADMIN.CLIENTS_CLIENT_ID" FOREIGN KEY ("CLIENT_ID") REFERENCES DB2ADMIN.CLIENTS ("ID") ON DELETE CASCADE;

ALTER TABLE DB2ADMIN.CLIENTS ALTER COLUMN ALLOWACCESSTOALLSCOPES SET WITH DEFAULT 0;
ALTER TABLE DB2ADMIN.CLIENTS ALTER COLUMN ALLOWACCESSTOALLGRANTTYPES SET WITH DEFAULT 0;
ALTER TABLE DB2ADMIN.CLIENTS ALTER COLUMN ALLOWCLIENTCREDENTIALSONLY SET WITH DEFAULT 0;
ALTER TABLE DB2ADMIN.CLIENTS ALTER COLUMN UPDATEACCESSTOKENONREFRESH SET WITH DEFAULT 0;
ALTER TABLE DB2ADMIN.CLIENTS ALTER COLUMN LOGOUTSESSIONREQUIRED SET WITH DEFAULT 0;
ALTER TABLE DB2ADMIN.CLIENTS ALTER COLUMN REQUIRESIGNOUTPROMPT SET WITH DEFAULT 0;
ALTER TABLE DB2ADMIN.CLIENTS ALTER COLUMN ALLOWACCESSTOKENSVIABROWSER SET WITH DEFAULT 1;