CREATE procedure [dbo].[InvestmentsGet]
(	
	@InvestmentStatus int = -1
)
as
begin	
	select 
		i.invest_id, 
		i.invest_name, 
		i.invest_qty, 
		i.invest_ave_price, 
		i.invest_last_price, 
		i.invest_price2calc, 
		ir.invest_date, 
		ir.rate_buy, 
		ir.rate_sell 
	from 
		Investment i 
		left join InvestRates ir on ir.invest_id = i.invest_id and ir.invest_date = (select max(ir2.invest_date) from InvestRates ir2 where ir2.invest_id = ir.invest_id)
        where 
			(nullif(@InvestmentStatus, -1) is null or i.invest_status = @InvestmentStatus)
end