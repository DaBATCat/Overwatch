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
("27-08-2023 14:40:34") , 

-- SessionEndTime : DATETIME
("27-08-2023 14:41:14") , 

-- TotalSessionDuration : DATETIME
("00:00:39-9747659") ,

-- TrackedDirectory : TEXT
"C:\ " ,

-- TotalActiveTime : DATETIME
("00:00:10-1392806") ,

-- TotalAFKTime : DATETIME
("00:00:29-8354853") ,

-- TotalTimesAFK : INT
2 ,

-- TotalEvents : INT
122 ,

-- TotalCreations : INT
3 ,

-- TotalDeletions : INT
2 ,

-- TotalRenamings : INT
0 ,

-- TotalErrors : INT
0 ,

-- SessionWasClosedBySystemEvent : BOOL/TINYINT
0 ,

-- DefaultAFKStartLimitInMiliseconds : LONG
10000 );

