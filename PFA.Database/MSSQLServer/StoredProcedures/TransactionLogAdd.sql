CREATE procedure [dbo].[TransactionLogAdd]
(	
	@TransactionId varchar(20),	
	@TransactionText ntext,
	@TransactionSourceId smallint = 1
)
as
begin	
	declare @b_TransactionId bigint;
	set @b_TransactionId = cast(@TransactionId as bigint);

	MERGE TransactionsLog AS t
    USING (SELECT @b_TransactionId, @TransactionText, @TransactionSourceId) AS s(TransactionId, TransactionText, TransactionSourceId)  
    ON (t.trans_id = s.TransactionId)  
    WHEN MATCHED THEN   
        UPDATE SET 
			trans_log_dt = GETDATE(), 
			trans_log_txt1 = s.TransactionText, 
			trans_source = s.TransactionSourceId
	WHEN NOT MATCHED THEN  
		INSERT (trans_id, trans_log_dt, trans_log_txt1, trans_source)  
		VALUES (s.TransactionId, GETDATE(), s.TransactionText, s.TransactionSourceId);
end
;