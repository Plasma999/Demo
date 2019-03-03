use Student;
GO

create proc StudentProfile_summary
@columnName varchar(15)
as
if @columnName = 'Gender'
begin
	select Gender as label, count(*) as data
	from StudentProfile
	group by Gender
end
else if @columnName = 'Blood'
begin
	select Blood as label, count(*) as data
	from StudentProfile
	group by Blood
end
else if @columnName = 'BMI'
begin
	with BMI(value) as
	(
		select Weight / square((Height/100)) from StudentProfile
	)

	select 
	case 
		when value < 15 then '< 15'
		when value between 15 and 18 then '15 ~ 18'
		when value between 18 and 21 then '18 ~ 21'
		when value between 21 and 24 then '21 ~ 24'
		when value between 24 and 27 then '24 ~ 27'
		when value > 27 then '> 27'
	end as label, count(*) as data
	from BMI
	group by 
	case 
		when value < 15 then '< 15'
		when value between 15 and 18 then '15 ~ 18'
		when value between 18 and 21 then '18 ~ 21'
		when value between 21 and 24 then '21 ~ 24'
		when value between 24 and 27 then '24 ~ 27'
		when value > 27 then '> 27'
	end
end

GO