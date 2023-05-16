CREATE procedure [dbo].[PayerUpd]
(	
	@PayerId bigint,
	@PayerName varchar(1000)
)
as
begin	
	declare @b_PayerId bigint;
	declare @Res table(Id bigint);
	
	if nullif(@PayerId, -1) is null or not exists(select 1 from dbo.Payers where payer_id = @PayerId)
	begin
		insert into dbo.Payers (payer_id, payer_name) 
		output inserted.payer_id into @Res(Id)
		values 
		(
			isnull((select max(payer_id) from dbo.Payers), 0) + 1,
			@PayerName
		)
	end
	else
	begin
		update Payers set payer_name = @PayerName where payer_id = @PayerId;
		insert into @Res(Id) values (@PayerId);
	
	end;
	
	select Id as PayerId from @Res;	
end
;