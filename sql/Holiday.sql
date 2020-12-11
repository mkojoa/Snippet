
-- PREVENT  CODE BLOCKS
SET DATEFIRST  7, -- 1 = Monday, 7 = Sunday
    DATEFORMAT mdy, 
    LANGUAGE   US_ENGLISH;
-- assume the above is here in all subsequent code blocks.

--************************************************
--CREATE NEEDED TABLES.
CREATE TABLE TblDateDimension(
[pkId] [uniqueidentifier] NOT NULL DEFAULT newid(),
[TheDate] [date] NOT NULL, 
[TheDay] [int] NOT NULL,
[TheDaySuffix] [nvarchar](100) NOT NULL,
[TheDayName] [nvarchar](50) NOT NULL,
[TheDayOfWeek] [int] NOT NULL,
[TheDayOfWeekInMonth]  [int] NOT NULL,         
[TheDayOfYear] [int] NOT NULL,
[IsWeekend] [bit] NOT NULL,        
[TheWeek] [int] NOT NULL,  
[TheISOweek] [int] NOT NULL,  
[TheFirstOfWeek] [date] NOT NULL,     
[TheLastOfWeek] [date] NOT NULL,      
[TheWeekOfMonth] [int] NOT NULL,        
[TheMonth] [int] NOT NULL,
[TheMonthName] [nvarchar](100) NOT NULL,
[TheFirstOfMonth] [date] NOT NULL,
[TheLastOfMonth] [date] NOT NULL,     
[TheFirstOfNextMonth] [date] NOT NULL,
[TheLastOfNextMonth] [date] NOT NULL, 
[TheQuarter] [int] NOT NULL,
[TheFirstOfQuarter] [date] NOT NULL,  
[TheLastOfQuarter] [date] NOT NULL,
[TheYear]  [int] NOT NULL,
[TheISOYear] [int] NOT NULL,         
[TheFirstOfYear] [date] NOT NULL,     
[TheLastOfYear] [date] NOT NULL,
[IsLeapYear] [int] NOT NULL,          
[Has53Weeks] [int] NOT NULL,         
[Has53ISOWeeks] [int] NOT NULL,      
[MMYYYY] [nvarchar](100) NOT NULL,             
[Style101] [nvarchar](100) NOT NULL,           
[Style103] [nvarchar](100) NOT NULL,           
[Style112] [nvarchar](100) NOT NULL,          
[Style120] [nvarchar](100) NOT NULL,
)
GO
CREATE UNIQUE CLUSTERED INDEX PK_DateDimension ON dbo.TblDateDimension(TheDate);


CREATE TABLE TblHolidayDimension
(
  pkId [uniqueidentifier] NOT NULL,
  TheDate date NOT NULL,
  HolidayText nvarchar(255) NOT NULL,
  CONSTRAINT FK_TblDateDimension FOREIGN KEY(TheDate) REFERENCES dbo.TblDateDimension(TheDate)
);

CREATE CLUSTERED INDEX CIX_TblHolidayDimension ON dbo.TblHolidayDimension(TheDate);

-- *********************************************************************************
--DECLARE START & END DATE FOR POPULATION
DECLARE @StartDate  date = '20200101'; --'20201001' --
DECLARE @CutoffDate date = DATEADD(DAY, -1, DATEADD(YEAR, 30, @StartDate)); --'20201231' --

;WITH seq(n) AS 
(
  SELECT 0 UNION ALL SELECT n + 1 FROM seq
  WHERE n < DATEDIFF(DAY, @StartDate, @CutoffDate)
),
d(d) AS 
(
  SELECT DATEADD(DAY, n, @StartDate) FROM seq
),
src AS
(
  SELECT
	Id				= newid(),
    TheDate         = CONVERT(date, d),
    TheDay          = DATEPART(DAY,       d),
    TheDayName      = DATENAME(WEEKDAY,   d),
    TheWeek         = DATEPART(WEEK,      d),
    TheISOWeek      = DATEPART(ISO_WEEK,  d),
    TheDayOfWeek    = DATEPART(WEEKDAY,   d),
    TheMonth        = DATEPART(MONTH,     d),
    TheMonthName    = DATENAME(MONTH,     d),
    TheQuarter      = DATEPART(Quarter,   d),
    TheYear         = DATEPART(YEAR,      d),
    TheFirstOfMonth = DATEFROMPARTS(YEAR(d), MONTH(d), 1),
    TheLastOfYear   = DATEFROMPARTS(YEAR(d), 12, 31),
    TheDayOfYear    = DATEPART(DAYOFYEAR, d)
  FROM d
),
dim AS
(
  SELECT
    Id,
    TheDate, 
    TheDay,
    TheDaySuffix        = CONVERT(char(2), CASE WHEN TheDay / 10 = 1 THEN 'th' ELSE 
                            CASE RIGHT(TheDay, 1) WHEN '1' THEN 'st' WHEN '2' THEN 'nd' 
                            WHEN '3' THEN 'rd' ELSE 'th' END END),
    TheDayName,
    TheDayOfWeek,
    TheDayOfWeekInMonth = CONVERT(tinyint, ROW_NUMBER() OVER 
                            (PARTITION BY TheFirstOfMonth, TheDayOfWeek ORDER BY TheDate)),
    TheDayOfYear,
    IsWeekend           = CASE WHEN TheDayOfWeek IN (CASE @@DATEFIRST WHEN 1 THEN 6 WHEN 7 THEN 1 END,7) 
                            THEN 1 ELSE 0 END,
    TheWeek,
    TheISOweek,
    TheFirstOfWeek      = DATEADD(DAY, 1 - TheDayOfWeek, TheDate),
    TheLastOfWeek       = DATEADD(DAY, 6, DATEADD(DAY, 1 - TheDayOfWeek, TheDate)),
    TheWeekOfMonth      = CONVERT(tinyint, DENSE_RANK() OVER 
                            (PARTITION BY TheYear, TheMonth ORDER BY TheWeek)),
    TheMonth,
    TheMonthName,
    TheFirstOfMonth,
    TheLastOfMonth      = MAX(TheDate) OVER (PARTITION BY TheYear, TheMonth),
    TheFirstOfNextMonth = DATEADD(MONTH, 1, TheFirstOfMonth),
    TheLastOfNextMonth  = DATEADD(DAY, -1, DATEADD(MONTH, 2, TheFirstOfMonth)),
    TheQuarter,
    TheFirstOfQuarter   = MIN(TheDate) OVER (PARTITION BY TheYear, TheQuarter),
    TheLastOfQuarter    = MAX(TheDate) OVER (PARTITION BY TheYear, TheQuarter),
    TheYear,
    TheISOYear          = TheYear - CASE WHEN TheMonth = 1 AND TheISOWeek > 51 THEN 1 
                            WHEN TheMonth = 12 AND TheISOWeek = 1  THEN -1 ELSE 0 END,      
    TheFirstOfYear      = DATEFROMPARTS(TheYear, 1,  1),
    TheLastOfYear,
    IsLeapYear          = CONVERT(bit, CASE WHEN (TheYear % 400 = 0) 
                            OR (TheYear % 4 = 0 AND TheYear % 100 <> 0) 
                            THEN 1 ELSE 0 END),
    Has53Weeks          = CASE WHEN DATEPART(ISO_WEEK, TheLastOfYear) = 53 THEN 1 ELSE 0 END,
    Has53ISOWeeks       = CASE WHEN DATEPART(WEEK,     TheLastOfYear) = 53 THEN 1 ELSE 0 END,
    MMYYYY              = CONVERT(char(2), CONVERT(char(8), TheDate, 101))
                          + CONVERT(char(4), TheYear),
    Style101            = CONVERT(char(10), TheDate, 101),
    Style103            = CONVERT(char(10), TheDate, 103),
    Style112            = CONVERT(char(8),  TheDate, 112),
    Style120            = CONVERT(char(10), TheDate, 120)
  FROM src
)
INSERT INTO TblDateDimension
SELECT *  FROM dim
  ORDER BY TheDate
  OPTION (MAXRECURSION 0);

-- *****************************************************
;WITH x AS 
(
  SELECT
    TheDate,
    TheFirstOfYear,
    TheDayOfWeekInMonth, 
    TheMonth, 
    TheDayName, 
    TheDay,
    TheLastDayOfWeekInMonth = ROW_NUMBER() OVER 
    (
      PARTITION BY TheFirstOfMonth, TheDayOfWeek
      ORDER BY TheDate DESC
    )
  FROM dbo.TblDateDimension
),
s AS
(
  SELECT TheDate, HolidayText = CASE
  WHEN (TheDate = TheFirstOfYear) 
    THEN 'New Year''s Day'
  WHEN (TheDayOfWeekInMonth = 3 AND TheMonth = 1 AND TheDayName = 'Monday')
    THEN 'Martin Luther King Day'    -- (3rd Monday in January)
  WHEN (TheDayOfWeekInMonth = 3 AND TheMonth = 2 AND TheDayName = 'Monday')
    THEN 'President''s Day'          -- (3rd Monday in February)
  WHEN (TheLastDayOfWeekInMonth = 1 AND TheMonth = 5 AND TheDayName = 'Monday')
    THEN 'Memorial Day'              -- (last Monday in May)
  WHEN (TheMonth = 7 AND TheDay = 4)
    THEN 'Independence Day'          -- (July 4th)
  WHEN (TheDayOfWeekInMonth = 1 AND TheMonth = 9 AND TheDayName = 'Monday')
    THEN 'Labour Day'                -- (first Monday in September)
  WHEN (TheDayOfWeekInMonth = 2 AND TheMonth = 10 AND TheDayName = 'Monday')
    THEN 'Columbus Day'              -- Columbus Day (second Monday in October)
  WHEN (TheMonth = 11 AND TheDay = 11)
    THEN 'Veterans'' Day'            -- (November 11th)
  WHEN (TheDayOfWeekInMonth = 4 AND TheMonth = 11 AND TheDayName = 'Thursday')
    THEN 'Thanksgiving Day'          -- (Thanksgiving Day ()fourth Thursday in November)
  WHEN (TheMonth = 12 AND TheDay = 25)
    THEN 'Christmas Day'
  END
  FROM x
  WHERE 
    (TheDate = TheFirstOfYear)
    OR (TheDayOfWeekInMonth = 3     AND TheMonth = 1  AND TheDayName = 'Monday')
    OR (TheDayOfWeekInMonth = 3     AND TheMonth = 2  AND TheDayName = 'Monday')
    OR (TheLastDayOfWeekInMonth = 1 AND TheMonth = 5  AND TheDayName = 'Monday')
    OR (TheMonth = 7 AND TheDay = 4)
    OR (TheDayOfWeekInMonth = 1     AND TheMonth = 9  AND TheDayName = 'Monday')
    OR (TheDayOfWeekInMonth = 2     AND TheMonth = 10 AND TheDayName = 'Monday')
    OR (TheMonth = 11 AND TheDay = 11)
    OR (TheDayOfWeekInMonth = 4     AND TheMonth = 11 AND TheDayName = 'Thursday')
    OR (TheMonth = 12 AND TheDay = 25)
)
INSERT dbo.TblHolidayDimension(TheDate, HolidayText)
SELECT TheDate, HolidayText FROM s 
UNION ALL 
SELECT DATEADD(DAY, 1, TheDate), 'Black Friday'
  FROM s WHERE HolidayText = 'Thanksgiving Day'
ORDER BY TheDate;

GO

--**************************************************
CREATE FUNCTION dbo.GetEasterHolidays(@TheYear INT) 
RETURNS TABLE
WITH SCHEMABINDING
AS 
RETURN 
(
  WITH x AS 
  (
    SELECT TheDate = DATEFROMPARTS(@TheYear, [Month], [Day])
      FROM (SELECT [Month], [Day] = DaysToSunday + 28 - (31 * ([Month] / 4))
      FROM (SELECT [Month] = 3 + (DaysToSunday + 40) / 44, DaysToSunday
      FROM (SELECT DaysToSunday = paschal - ((@TheYear + (@TheYear / 4) + paschal - 13) % 7)
      FROM (SELECT paschal = epact - (epact / 28)
      FROM (SELECT epact = (24 + 19 * (@TheYear % 19)) % 30) 
        AS epact) AS paschal) AS dts) AS m) AS d
  )
  SELECT TheDate, HolidayText = 'Easter Sunday' FROM x
    UNION ALL SELECT DATEADD(DAY, -2, TheDate), 'Good Friday'   FROM x
    UNION ALL SELECT DATEADD(DAY,  1, TheDate), 'Easter Monday' FROM x
);
GO


-- *********************************************
INSERT dbo.TblHolidayDimension(TheDate, HolidayText)
  SELECT d.TheDate, h.HolidayText
    FROM dbo.TblDateDimension AS d
    CROSS APPLY dbo.GetEasterHolidays(d.TheYear) AS h
    WHERE d.TheDate = h.TheDate;
GO


--***********************************************
CREATE VIEW TheCalendar AS 
  SELECT
    d.TheDate,
    d.TheDay,
    d.TheDaySuffix,
    d.TheDayName,
    d.TheDayOfWeek,
    d.TheDayOfWeekInMonth,
    d.TheDayOfYear,
    d.IsWeekend,
    d.TheWeek,
    d.TheISOweek,
    d.TheFirstOfWeek,
    d.TheLastOfWeek,
    d.TheWeekOfMonth,
    d.TheMonth,
    d.TheMonthName,
    d.TheFirstOfMonth,
    d.TheLastOfMonth,
    d.TheFirstOfNextMonth,
    d.TheLastOfNextMonth,
    d.TheQuarter,
    d.TheFirstOfQuarter,
    d.TheLastOfQuarter,
    d.TheYear,
    d.TheISOYear,
    d.TheFirstOfYear,
    d.TheLastOfYear,
    d.IsLeapYear,
    d.Has53Weeks,
    d.Has53ISOWeeks,
    d.MMYYYY,
    d.Style101,
    d.Style103,
    d.Style112,
    d.Style120,
    IsHoliday = CASE WHEN h.TheDate IS NOT NULL THEN 1 ELSE 0 END,
    h.HolidayText
  FROM dbo.TblDateDimension AS d
  LEFT OUTER JOIN dbo.TblHolidayDimension AS h
  ON d.TheDate = h.TheDate;


select * from TblHolidayDimension
