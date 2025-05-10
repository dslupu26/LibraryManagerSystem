-- Generate and execute SQL to drop foreign key constraints
DECLARE @sql NVARCHAR(MAX) = N'';

-- Generate ALTER TABLE DROP CONSTRAINT statements for foreign keys
SELECT @sql += 'ALTER TABLE ' 
             + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' 
             + QUOTENAME(OBJECT_NAME(parent_object_id)) 
             + ' DROP CONSTRAINT ' 
             + QUOTENAME(name) + ';' + CHAR(13)
FROM sys.foreign_keys;

-- Execute the SQL for dropping constraints
EXEC sp_executesql @sql;

-- Reset the SQL variable for dropping tables
SET @sql = N'';

-- Generate DROP TABLE statements for all tables
SELECT @sql += 'DROP TABLE ' 
             + QUOTENAME(TABLE_SCHEMA) + '.' 
             + QUOTENAME(TABLE_NAME) + ';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

-- Execute the SQL for dropping tables
EXEC sp_executesql @sql;