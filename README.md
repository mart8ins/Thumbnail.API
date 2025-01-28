# Thumbnail.API

Web.Api service project that generates thumbnails for provided files. Accepted files - images, videos, office documents. Current implementation is to load file names from database, find file contents in source folder, upload files and generate for each one a thumbnail what is saved in different folder. API has two endpoints - "create-all" to loop through all database and generate thumbnails for all files and also route "create" for specific file 
using file id. Both routes use background job service Hangfire. 

## Used NuGet packages
1. For images https://github.com/dlemstra/Magick.NET

 
