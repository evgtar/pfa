CREATE procedure [dbo].[CategoryUpd]
(	
	@CategoryId bigint,
	@ParentCategoryId bigint,
    @CategoryName varchar(255),    
    @CategoryStatus smallint
)
as
begin	
	declare @RetVal table (Id bigint) ;
	
	if @CategoryId > 0 and exists(select 1 from dbo.Categories where category_id = @CategoryId)
	begin
		update dbo.Categories set category_name = @CategoryName, category_status = @CategoryStatus where category_id = @CategoryId;
		insert into @RetVal (Id) values (@CategoryId);
	end
	else
	begin
		insert into dbo.Categories (category_id, pcategory_id, category_name, category_status) 
		output inserted.category_id into @RetVal		
		values (
			coalesce((select max(category_id) from dbo.Categories), 0) + 1, 
			@ParentCategoryId, @CategoryName, @CategoryStatus
		);
	end
	
	select Id as CategoryId from @RetVal;
end