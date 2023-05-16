CREATE procedure [dbo].[PayerDel]
(	
	@PayerId bigint
)
as
begin	
	delete from Payers where payer_id = @PayerId;
end