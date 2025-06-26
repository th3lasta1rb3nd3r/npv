steps to run back-end & front-end:

1. Clone this repo
2. Open VS Code or VS Studio:

3. Open Terminal and cd ..\npv\back-end\
4. Install .NET 10 (sdk & runtime) if not installed yet
5. dotnet test
6. cd ..\npv\back-end\Npv.Api\
7. dotnet run

8. Open another VS Code
9. Open Terminal and cd ..\npv\front-end\npv-ui
10. Install angular 19 or latest
11. ng test --browsers ChromeHeadless --watch=false
12. ng serve
13. nagivate http://localhost:4200/
