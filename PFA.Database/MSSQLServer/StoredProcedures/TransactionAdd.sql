CREATE procedure [dbo].[TransactionAdd]
(	
	@TransactionId varchar(20),
	@TransactionDate varchar(20),
	@PaymentType bigint,
	@CategoryId bigint,
	@AccountId bigint,
	@AccountIdRef bigint = -1,
	@PayerId bigint,
	@Amount float,
	@CurrencyRate float = 1,
	@AmountBase float,
	@Notes varchar(MAX),
	@AssetId bigint = -1
)
as
begin	
	declare @b_TransactionId bigint;
	declare @f_AmountLocal float = 0.0;
	declare @f_AmountBase float = 0.0;
	
	if nullif(nullif(ltrim(rtrim(@TransactionId)), ''), -1) is null or not exists(select 1 from Transactions where trans_id = cast(@TransactionId as bigint))
	begin
		set @b_TransactionId = case
			when nullif(nullif(ltrim(rtrim(@TransactionId)), ''), -1) is null then isnull((select max(trans_id) from Transactions), 0) + 1
			else cast(@TransactionId as bigint)
		 end;


		insert into Transactions (trans_id, trans_date, trans_type, category_id, account_id, account_id_ref, payer_id, asset_id, amount_local, currency_rate, amount_base, trans_notes, trans_status) 
		values (@b_TransactionId, convert(datetime, @TransactionDate, 120), @PaymentType, @CategoryId, @AccountId, @AccountIdRef, @PayerId, @AssetId, @Amount, @CurrencyRate, @AmountBase, @Notes, 1);
		
		update Accounts set 
			local_balance = local_balance + case when @PaymentType = 1 then -1 else 1 end * @Amount, 
			base_balance = base_balance + case when @PaymentType = 1 then -1 else 1 end * @AmountBase 
		where account_id = @AccountId;
	end
	else
	begin
		set @b_TransactionId = cast(@TransactionId as bigint);
		select 
			@f_AmountLocal = amount_local,
			@f_AmountBase = amount_base
		from Transactions 
		where trans_id = @b_TransactionId;
		
		update Transactions set 
            trans_date = convert(datetime, @TransactionDate, 120),
            trans_type = @PaymentType,
            category_id = @CategoryId,
            account_id = @AccountId,
            payer_id = @PayerId,
			asset_id = @AssetId,
            amount_local = @Amount,
            currency_rate = @CurrencyRate,
            amount_base = @AmountBase,
            trans_notes = @Notes
		where 
			trans_id = @TransactionId;     
		print concat('(local) new=', @Amount, ' old=', @f_AmountLocal);
		print concat('(base) new=', @AmountBase, ' old=', @f_AmountBase);
		update Accounts set 
			local_balance = local_balance + case when @PaymentType = 1 then -1 else 1 end * @Amount + case when @PaymentType = 1 then 1 else -1 end * @f_AmountLocal, 
			base_balance = base_balance + case when @PaymentType = 1 then -1 else 1 end * @AmountBase + case when @PaymentType = 1 then -1 else 1 end * @f_AmountBase
		where account_id = @AccountId;
	end;
	
	select @b_TransactionId as TransactionId;
end
;