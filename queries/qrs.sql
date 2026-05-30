CREATE TABLE work_sessions
(
    serialno SERIAL PRIMARY KEY,
    machinename VARCHAR(30) NOT NULL,
    username VARCHAR(30) NOT NULL,
    checkin_time TIMESTAMP NOT NULL,
    checkout_time TIMESTAMP,
    status VARCHAR(20),
    synced BOOLEAN DEFAULT FALSE
);  


CREATE TABLE work_sessions
(
    SerialNo SERIAL PRIMARY KEY,
    MachineName VARCHAR(30) NOT NULL,
    UserName VARCHAR(30) NOT NULL,
    CheckIntime TIMESTAMP NOT NULL,
    CheckOutTme TIMESTAMP,
    Status VARCHAR(20),
    Synced BOOLEAN DEFAULT FALSE
);  