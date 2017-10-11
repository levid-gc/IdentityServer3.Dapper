﻿DROP TABLE DB2ADMIN.SCOPECLAIMS;
DROP TABLE DB2ADMIN.SCOPESECRETS;
DROP TABLE DB2ADMIN.SCOPES;

CREATE TABLE DB2ADMIN.SCOPES (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    ENABLED SMALLINT NOT NULL,
    NAME VARCHAR(200) NOT NULL,
    DISPLAYNAME VARCHAR(200),
    DESCRIPTION VARCHAR(1000),
    REQUIRED SMALLINT NOT NULL,
    EMPHASIZE SMALLINT NOT NULL,
    TYPE INT NOT NULL,
    INCLUDEALLCLAIMSFORUSER SMALLINT NOT NULL,
    CLAIMSRULE VARCHAR(200),
    SHOWINDISCOVERYDOCUMENT SMALLINT NOT NULL,
    ALLOWUNRESTRICTEDINTROSPECTION SMALLINT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.SCOPES" PRIMARY KEY ("ID")
);

CREATE TABLE DB2ADMIN.SCOPECLAIMS (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    NAME VARCHAR(200) NOT NULL,
    DESCRIPTION VARCHAR(1000),
    ALWAYSINCLUDEINIDTOKEN SMALLINT NOT NULL,
    SCOPE_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.SCOPECLAIMS" PRIMARY KEY ("ID")
);
CREATE INDEX IX_SCOPECLAIMS_SCOPE_ID ON DB2ADMIN.SCOPECLAIMS("SCOPE_ID");

CREATE TABLE DB2ADMIN.SCOPESECRETS (
    ID INT NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1, NO CACHE),
    DESCRIPTION VARCHAR(1000),
    EXPIRATION TIMESTAMP,
    TYPE VARCHAR(250),
    VALUE VARCHAR(250) NOT NULL,
    SCOPE_ID INT NOT NULL,
    CONSTRAINT "PK_DB2ADMIN.SCOPESECRETS" PRIMARY KEY ("ID")
);
CREATE INDEX IX_SCOPESECRETS_SCOPE_ID ON DB2ADMIN.SCOPESECRETS("SCOPE_ID");

ALTER TABLE DB2ADMIN.SCOPECLAIMS ADD CONSTRAINT "FK_DB2ADMIN.SCOPECLAIMS_DB2ADMIN.SCOPES_SCOPE_ID" FOREIGN KEY ("SCOPE_ID") REFERENCES DB2ADMIN.SCOPES ("ID") ON DELETE CASCADE;
ALTER TABLE DB2ADMIN.SCOPESECRETS ADD CONSTRAINT "FK_DB2ADMIN.SCOPESECRETS_DB2ADMIN.SCOPES_SCOPE_ID" FOREIGN KEY ("SCOPE_ID") REFERENCES DB2ADMIN.SCOPES ("ID") ON DELETE CASCADE;

ALTER TABLE DB2ADMIN.SCOPES ALTER COLUMN ALLOWUNRESTRICTEDINTROSPECTION SET WITH DEFAULT 0;