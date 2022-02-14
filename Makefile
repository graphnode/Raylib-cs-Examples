build:
	dotnet build Examples/Examples.csproj -f net6.0
build-release:
	dotnet build -c Release
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch --project Examples/Examples.csproj run
run-examples:
	dotnet run --project Examples/Examples.csproj -f net6.0
run-imgui:
	dotnet run --project ImGuiDemo/ImGuiDemo.csproj -f net6.0
