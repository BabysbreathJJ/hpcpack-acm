FROM jjtest.azurecr.io/public/hpcpack/hpcacm/runtime AS final
WORKDIR /app/scripts
COPY Docker/scripts/* ./
WORKDIR /app
COPY allbuild/app .
ENTRYPOINT ["dotnet", "Bootstrap/Bootstrap.dll"]
