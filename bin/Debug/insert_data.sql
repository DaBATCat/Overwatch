-- Please note: This SQL File ist just a template for python to replace the values.

INSERT INTO SessionInfo
(
    SessionID,
    SessionStartTime ,
    SessionEndTime ,
    TotalSessionDuration ,
    TrackedDirectory ,
    TotalActiveTime ,
    TotalAFKTime ,
    TotalTimesAFK ,
    TotalEvents ,
    TotalCreations ,
    TotalDeletions ,
    TotalRenamings ,
    TotalErrors ,
    SessionWasClosedBySystemEvent ,
    DefaultAFKStartLimitInMiliseconds 
) VALUES
-- Auto incremented automatically
(NULL, 

-- SessionStartTime : DATETIME
("26-08-2023 19:05:58") , 

-- SessionEndTime : DATETIME
("26-08-2023 19:07:25") , 

-- TotalSessionDuration : DATETIME
("00:01:26-6195021") ,

-- TrackedDirectory : TEXT
"C:\ " ,

-- TotalActiveTime : DATETIME
("00:00:59-0536501") ,

-- TotalAFKTime : DATETIME
("00:00:27-5658520") ,

-- TotalTimesAFK : INT
2 ,

-- TotalEvents : INT
499 ,

-- TotalCreations : INT
29 ,

-- TotalDeletions : INT
33 ,

-- TotalRenamings : INT
32 ,

-- TotalErrors : INT
0 ,

-- SessionWasClosedBySystemEvent : BOOL/TINYINT
0 ,

-- DefaultAFKStartLimitInMiliseconds : LONG
10000 );

