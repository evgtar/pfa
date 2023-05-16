CREATE procedure [dbo].[CurrencyDel]
(	
	@CurrencyCode varchar(3)    
)
as
begin	
	delete from Currencies where currency_code = @CurrencyCode;
end