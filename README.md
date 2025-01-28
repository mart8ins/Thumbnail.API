# Thumbnail.API

Web API service project that generates thumbnails for provided files. Accepted files - images, videos, office documents. Current implementation is to load file names from database, find file contents in source folder, upload files and generate for each one a thumbnail what is saved in different folder. API has two endpoints - "create-all" to loop through all database and generate thumbnails for all files and also route "create" for specific file using file id. Both routes use background job service Hangfire. 

## Used NuGet packages
1. For images https://github.com/dlemstra/Magick.NET
2. For videos https://www.nuget.org/packages/FFMpegCore/ + executable https://ffmpeg.org/download.html
3. For Office documents https://www.nuget.org/packages/FreeSpire.Office/
4. For PDF https://github.com/jbaarssen/PdfLibCore
5. For logging https://serilog.net/
6. Background jobs https://www.hangfire.io/
7. For database, ORM https://github.com/DapperLib/Dapper
 
