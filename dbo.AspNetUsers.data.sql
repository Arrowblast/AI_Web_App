BULK INSERT dbo.AspNetUsers
FROM 'C:\Users\Robert Walicki\Desktop\Pulpit\Aplikacje_internetowe\AI_Web_App\AI_Web_App\populate_users.csv'
WITH
(
	DATAFILETYPE = 'char',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',  --CSV field delimiter
    ROWTERMINATOR = '\n',
	KEEPNULLS,--Use to shift the control to next row
    TABLOCK
)