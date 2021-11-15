
go
drop proc if exists FakeImage_DataMining
go
create proc FakeImage_DataMining @test bit = 0
as
begin
-----
delete from FakeResults

-- Files
declare @files table(fileId int, isFake bit)
insert into @files(fileId, isFake) select id, isFake from FakeImages where status= 0

-- File attributes
declare @t table(fileId int, attrId int, valueType int, valueLength int, valueText varchar(255), isFake bit)
insert into @t select fi.fileId, attrId, valueType, valueLength, valueText, isFake
from FakeImageAttrs as fia
left join FakeAttrs as fa on fa.id= fia.attrId
left join @files as fi on fi.fileId= fia.fileId
where fia.status= 0 and fa.status= 0


-- Miss attributes
declare @fileId int, @isFake bit
declare cur CURSOR FOR SELECT fileId, isFake FROM @files
OPEN cur FETCH NEXT FROM cur into @fileId, @isFake
WHILE @@FETCH_STATUS = 0
begin
	insert into @t 
	select fileId= @fileId, id as attrId, valueType=0, valueLength=0, valueText= '', isFake= @isFake
	from FakeAttrs where id not in (select attrId from @t as t2 where t2.fileId= @fileId)

	FETCH NEXT FROM cur into @fileId, @isFake
end
CLOSE cur

delete from @files
if(@test= 1) select '' as N'Thuộc tính đơn', * from @t order by attrId asc -- test only

-- Fetch fakeCount, realCount
declare @rs table(attrId int, valueText varchar(255),fakeCount int,realCount int)
insert into @rs 
select attrId, valueText,
	sum(case when isFake=1 then 1 else 0 end) as fakeCount,
	sum(case when isFake=0 then 1 else 0 end) as realCount
	from @t as t
	group by attrId, valueText

-- FakeResults (level 1)
insert into FakeResults(attrId, valueText, fakeCount, isCombine, parentId)
select attrId, valueText, fakeCount, 0 as isCombine, 0 as parentId from @rs where fakeCount>0 and realCount=0

delete from @t where attrId in (select attrId from FakeResults)

if(@test=1) select '' as N'FakeResults (Ảnh giả - 100%)', * from FakeResults order by attrId asc -- test only
if(@test= 1)  select '' as N'Nghi là giả',* from @rs where fakeCount>0 and realCount>0 order by attrId asc -- test only

-- Combine attributes
declare @attrId int, @valueText varchar(255), @fakeCount int, @parentId int
declare @stacks table(attrId int, valueText varchar(255), fakeCount int)
declare @tblAttrIds table(attrId int, valueText varchar(255), fakeCount int)
insert into @tblAttrIds SELECT attrId, valueText, fakeCount FROM @rs where fakeCount>0 and realCount>0

declare cur2 CURSOR FOR SELECT  attrId, valueText, fakeCount FROM @tblAttrIds
OPEN cur2 FETCH NEXT FROM cur2 into  @attrId, @valueText, @fakeCount
WHILE @@FETCH_STATUS = 0
	begin
		delete from @rs
		insert into @rs
		select attrId, valueText,
			sum(case when isFake=1 then 1 else 0 end) as fakeCount,
			sum(case when isFake=0 then 1 else 0 end) as realCount
			from @t where fileId in (select distinct fileId from @t as t2 where t2.attrId= @attrId and valueText= @valueText)
			group by attrId, valueText

		if(@test= 1) select concat(@attrId,'') as N'Thuộc tính kết hợp', * from @rs order by realCount asc -- test only

		if((select count(1) from @rs where fakeCount>0 and realCount= 0)>0)
		begin
			insert into FakeResults(attrId, valueText, fakeCount, isCombine, parentId)
			values(@attrId, @valueText, @fakeCount, 1, 0);

			select @parentId= SCOPE_IDENTITY()

			insert into FakeResults(attrId, valueText, fakeCount, isCombine, parentId)
			select attrId, valueText, fakeCount, 1 as isCombine, @parentId as parentId from @rs where fakeCount>0 and realCount=0;
		end

		--if(@test=1) select '' as N'FakeResults (Ảnh giả - 100%)', * from FakeResults order by attrId asc -- test only

		insert into @tblAttrIds SELECT attrId, valueText, fakeCount FROM @rs where fakeCount>0 and realCount>0
		FETCH NEXT FROM cur2 into @attrId, @valueText, @fakeCount
	end

CLOSE cur
end

--- test
go
select 
Images= (select count(1) from FakeImages),
Attrs= (select count(1) from FakeAttrs),
ImageAttrs= (select count(1) from FakeImageAttrs)

exec FakeImage_DataMining 1