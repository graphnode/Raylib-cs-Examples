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
run-examples:
	dotnet run -p Examples/Examples.csproj -f net5.0
run-imgui:
	dotnet run -p ImGuiDemo/ImGuiDemo.csproj -f net5.0
