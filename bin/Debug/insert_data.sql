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
("27-08-2023 15:35:42") , 

-- SessionEndTime : DATETIME
("27-08-2023 15:35:48") , 

-- TotalSessionDuration : DATETIME
("00:00:06-3686230") ,

-- TrackedDirectory : TEXT
"C:\ " ,

-- TotalActiveTime : DATETIME
("00:00:06-3686230") ,

-- TotalAFKTime : DATETIME
("00:00:00") ,

-- TotalTimesAFK : INT
0 ,

-- TotalEvents : INT
307 ,

-- TotalCreations : INT
42 ,

-- TotalDeletions : INT
38 ,

-- TotalRenamings : INT
31 ,

-- TotalErrors : INT
0 ,

-- SessionWasClosedBySystemEvent : BOOL/TINYINT
0 ,

-- DefaultAFKStartLimitInMiliseconds : LONG
600000 );

