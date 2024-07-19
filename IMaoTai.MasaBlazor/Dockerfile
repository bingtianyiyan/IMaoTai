#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#docker build -t imaotaiblazor -f ./Dockerfile .
#docker run -d  -p 8080:8080 -e TZ=Asia/Shanghai   -v /opt/IMaoTaiBlazor:/app  -v /opt/IMaoTaiBlazor/logs:/app/logs  -v /etc/localtime:/etc/localtime --network custom-local-net   --name imaotaiblazor imaotaiblazor
FROM  mcr.microsoft.com/dotnet/aspnet:8.0 AS base
COPY . /app
WORKDIR /app
EXPOSE 5004

ENV TZ=Asia/Shanghai

CMD ["nohup","dotnet", "IMaoTai.MasaBlazor.dll"]