#下面是docker生成发布文件的地方
#从官方sdk获取基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

#下面是进行build的命令
FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
#确定工作目录
WORKDIR /src
#将文件夹下对应的项目拷贝到指定的位置，现在指定的位置为"."
# COPY ["Routine.API/Routine.API.csproj", .]
COPY *.csproj .
#还原项目，即引用项目所需要的包
RUN dotnet restore 
#将刚刚还原的其余文件复制到镜像中，即将本地计算机Routine.API中的其余文件赋值到镜像的src文件中
COPY . .
RUN dotnet build -c Release -o /app/build


FROM build-env AS publish
RUN dotnet publish  -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Routine.API.dll"]
