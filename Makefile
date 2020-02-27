build:
	dotnet build
build-release:
	dotnet build -c Release
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch --project=Examples/Examples.csproj run
start:
	dotnet run --framework=netcoreapp3.1 --project=Examples/Examples.csproj
