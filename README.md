Add-Migration InitialIdentity -Project UserManagement.Infrastructure -StartupProject UserManagement.API

Update-Database -Project UserManagement.Infrastructure -StartupProject UserManagement.API