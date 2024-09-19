# If EF Core tools need to be updated, use:
dotnet tool update --global dotnet-ef

# Drop any existing database
dotnet ef database drop -f -c SqlServerDbContext -p ../DbContext -s ../DbContext

# Remove any existing migrations folder
Remove-Item -Recurse -Force ../DbContext/Migrations