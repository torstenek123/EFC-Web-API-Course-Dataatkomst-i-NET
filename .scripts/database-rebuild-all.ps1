# PowerShell Script for EF Core Migration and Database Update

# Make sure DbConnection is: SQLServer-musicefc-azkv-docker-sysadmin
# Ensure "DbSetActiveIdx": 0 is set in DbContext/appsettings.json

# If EF Core tools need to be updated, use:
dotnet tool update --global dotnet-ef

# Drop any existing database
dotnet ef database drop -f -c SqlServerDbContext -p ../DbContext -s ../DbContext

# Remove any existing migrations folder
Remove-Item -Recurse -Force ../DbContext/Migrations

# Create a new full migration
dotnet ef migrations add miInitial -c SqlServerDbContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/SqlServerDbContext

# Update the database from the new migration
dotnet ef database update -c SqlServerDbContext -p ../DbContext -s ../DbContext

# To initialize the database, you may need to run the SQL scripts in Azure Data Studio:
# ../DbContext/SqlScripts/initDatabase.sql
# Or run a separate script: ./database-init.ps1