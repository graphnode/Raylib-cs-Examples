build:
	dotnet build Examples/Examples.csproj -f net5.0
build-release:
	dotnet build -c Release
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch -p Examples/Examples.csproj run
start:
	dotnet run -p Examples/Examples.csproj -f net5.0
