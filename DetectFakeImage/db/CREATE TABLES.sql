drop table if exists FakeImages
go
create table FakeImages(
id int primary key identity(1,1),
[fileName] nvarchar(255),
sha256 varchar(64),
isFake bit,
createTime DateTime,
lastModify DateTime,
[status] tinyint default(0)
)
go

drop table if exists FakeAttrs
go
create table FakeAttrs(
id int primary key,
[name] nvarchar(200),
[desc] nvarchar(1000),
[status] tinyint default(0)
)
go

drop table if exists FakeImageAttrs
go
create table FakeImageAttrs(
id int primary key identity(1,1),
fileId int,
attrId int,
valueType int,
valueLength int,
valueText varchar(255),
[status] tinyint default(0)
)
go

drop table if exists FakeResults
go
create table FakeResults(
id int primary key identity(1,1),
attrId int,
valueText ntext,
fakeCount int,
parentId int
)

go
select 
Images= (select count(1) from FakeImages),
Attrs= (select count(1) from FakeAttrs),
ImageAttrs= (select count(1) from FakeImageAttrs)
