#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM  mcr.microsoft.com/dotnet/aspnet:8.0 AS base
COPY . /app
WORKDIR /app
EXPOSE 5004

ENV TZ=Asia/Shanghai

CMD ["nohup","dotnet", "IMaoTai.MasaBlazor.dll"]