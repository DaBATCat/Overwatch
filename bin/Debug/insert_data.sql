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
("07-09-2023 16:13:44") , 

-- SessionEndTime : DATETIME
("07-09-2023 16:14:46") , 

-- TotalSessionDuration : DATETIME
("00:01:02-8540722") ,

-- TrackedDirectory : TEXT
"C:\ " ,

-- TotalActiveTime : DATETIME
("00:01:02-8540722") ,

-- TotalAFKTime : DATETIME
("00:00:00") ,

-- TotalTimesAFK : INT
0 ,

-- TotalEvents : INT
1410 ,

-- TotalCreations : INT
147 ,

-- TotalDeletions : INT
143 ,

-- TotalRenamings : INT
54 ,

-- TotalErrors : INT
0 ,

-- SessionWasClosedBySystemEvent : BOOL/TINYINT
0 ,

-- DefaultAFKStartLimitInMiliseconds : LONG
600000 );

