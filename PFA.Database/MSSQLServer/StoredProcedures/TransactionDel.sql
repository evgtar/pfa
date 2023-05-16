CREATE procedure [dbo].[TransactionDel]
(	
	@TransactionId bigint
)
as
begin	
	delete from Transactions where trans_id = @TransactionId;
end
;