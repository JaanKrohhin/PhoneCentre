
# AlphaCRC web test

A test project for testing my capabilities


# PhoneCentre

## Project Setup:
 1. Go to SQL Server Management Studio
 2. Login
 3. Right-click on the database folder
 4. Click "Import Data-Tier Application"
 5. Find Project Directory -> Data
 6. Select CallerDb.bacpac and continue the import
 7. Go to Visual Studio -> View -> Server Explorer
 8. Select the server where you imported the database, input all needed info and click "Connect"
 9. Unfold the newly connected server, find and right-click on CallerDb
 10. Click Properties and copy the connection string
 11. Now go to Project -> Project Properties -> Resources and you will see a connection string variable. Replace its value with your own.
 12. The Database is ready 

## Errors with the project:

   * Plugin "react" was conflicted between "package.json » eslint-config-react-app » ........
   
   To fix this problem, it is neccasary to go into ClientApp, find the package.json file and re-save it. Afterwards problem should be gone. This problem happens only on Windows.
