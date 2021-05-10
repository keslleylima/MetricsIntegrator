## Generate jar

#### Linux x64

1. In the project folder, run the following command:
> dotnet publish -p:Configuration=Release -p:PublishTrimmed=true -p:SelfContained=true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:UseAppHost=true --runtime linux-x64

2. Go to `src\bin\Release\net5.0\linux-x64\publish` and rename `MetricsIntegrator` to `metrics-integrator-X.Y.Z-ubuntu-x64`, where `X`, `Y` amd `Z` are the version number

3. Add the renamed file to a zip file with the following name: `metrics-integratorX.Y.Z-ubuntu-x64.zip`

4. Move the zip to dist/`X`.x/`X.Y.Z`

#### MacOS x64

1. In the project folder, run the following command:
> dotnet publish -p:Configuration=Release -p:PublishTrimmed=true -p:SelfContained=true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true --runtime osx-x64

2. Go to `src\bin\Release\net5.0\osx-x64\publish` and rename `MetricsIntegrator` to `metrics-integrator-X.Y.Z-osx-x64`, where `X`, `Y` amd `Z` are the version number

3. Run the following command:
> chmod +x `metrics-integratorX.Y.Z-osx-x64`

4. Add the renamed file to a zip file with the following name: `metrics-integratorX.Y.Z-osx-x64.zip`

5. Move the zip to dist/`X`.x/`X.Y.Z`

#### Windows x64

1. In the project folder, run the following command:
> dotnet publish -p:Configuration=Release -p:PublishTrimmed=true -p:SelfContained=true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true --runtime win-x64

2. Go to `src\bin\Release\net5.0\windows-x64\publish` and rename `MetricsIntegrator.exe` to `metrics-integrator-X.Y.Z-windows-x64.exe`, where `X`, `Y` amd `Z` are the version number

3. Add the renamed file to a zip file with the following name: `metrics-integrator-X.Y.Z-windows-x64.zip`

4. Move the zip to dist/`X`.x/`X.Y.Z`
