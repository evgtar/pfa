CREATE procedure [dbo].[TransactionInfoGet]
(	
	@TransactionId bigint	
)
as
begin	
	select 
		trans_id, 
		trans_date, 
		account_id, 
		category_id, 
		payer_id, 
		asset_id,
		trans_type, 
		amount_local, 
		currency_rate, 
		amount_base, 
		trans_notes 
	from 
		Transactions 
	where 
		trans_id = @TransactionId;
end
;