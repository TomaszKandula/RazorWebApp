SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET XACT_ABORT ON
SET NOCOUNT ON

BEGIN TRY
    BEGIN TRANSACTION

        BULK INSERT 
            Countries
        FROM 
            '/Countries.csv'
        WITH
        (
            FIRSTROW = 2,
            FIELDTERMINATOR = ';',  
            ROWTERMINATOR = '\n',   
            TABLOCK
        )

        BULK INSERT 
            Cities
        FROM 
            '/Cities.csv'
        WITH
        (
            FIRSTROW = 2,
            FIELDTERMINATOR = ';',  
            ROWTERMINATOR = '\n',   
            TABLOCK
        )

    COMMIT TRANSACTION

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
    ;THROW
END CATCH
