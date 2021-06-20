Create	Function [DaysInBattle]
(
@startdate date,
@enddate date
)
returns int
as
begin
declare @days int
select @days=DATEDIFF(D,@startdate,@enddate) + 1
return @days
end
