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
("27-08-2023 12:38:20") , 

-- SessionEndTime : DATETIME
("27-08-2023 12:38:36") , 

-- TotalSessionDuration : DATETIME
("00:00:16-1704777") ,

-- TrackedDirectory : TEXT
"C:\ " ,

-- TotalActiveTime : DATETIME
("00:00:16-1704777") ,

-- TotalAFKTime : DATETIME
("00:00:00") ,

-- TotalTimesAFK : INT
0 ,

-- TotalEvents : INT
212 ,

-- TotalCreations : INT
22 ,

-- TotalDeletions : INT
22 ,

-- TotalRenamings : INT
19 ,

-- TotalErrors : INT
0 ,

-- SessionWasClosedBySystemEvent : BOOL/TINYINT
0 ,

-- DefaultAFKStartLimitInMiliseconds : LONG
10000 );

