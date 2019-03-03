use Student;
GO

create proc StudentProfile_Sel
@Id_operator varchar(4),
@Id_value varchar(12),
@Name_operator varchar(4),
@Name_value varchar(50),
@Coupon_operator varchar(4),
@Coupon_value varchar(15),
@Height_operator varchar(7),
@Height_value decimal(6,2),
@Height_value2 decimal(6,2),
@Weight_operator varchar(7),
@Weight_value decimal(6,2),
@Weight_value2 decimal(6,2),
@Gender_value varchar(1),
@Blood_value varchar(20),
@Start int,
@Length int,
@OrderBy varchar(20),
@OrderByAsc varchar(4)
as
declare @sql_count nvarchar(1000) = 'select count(*) as count ';
declare @sql_topN nvarchar(1000) = 'select top 1000 * ';
declare @sql nvarchar(1000) = 'from StudentProfile where 1=1 ';

--DataTables.js Server-side processing
if @Length > 0
begin
	set @sql_topN = 'select * ';
end

--Äæ¦ì·j´M±ø¥ó
if @Id_operator <> ''
begin
	if @Id_value = 'null'
		set @sql = @sql + ' and Id is null';
	else
		set @sql = @sql + ' and Id ' + @Id_operator + ' ''' + @Id_value + '''';
end
if @Name_operator <> ''
begin
	if @Name_value = 'null'
		set @sql = @sql + ' and Name is null';
	else
		set @sql = @sql + ' and Name ' + @Name_operator + ' ''' + @Name_value + '''';
end
if @Coupon_operator <> ''
begin
	if @Coupon_value = 'null'
		set @sql = @sql + ' and Coupon is null';
	else
		set @sql = @sql + ' and Coupon ' + @Coupon_operator + ' ''' + @Coupon_value + '''';
end
if @Height_operator <> ''
begin
	if @Height_operator = 'between'
		set @sql = @sql + ' and Height between ' + cast(@Height_value as nvarchar(7)) + ' and ' + cast(@Height_value2 as nvarchar(7));
	else
		set @sql = @sql + ' and Height ' + @Height_operator + ' ' + cast(@Height_value as nvarchar(7));
end
if @Weight_operator <> ''
begin
	if @Weight_operator = 'between'
		set @sql = @sql + ' and Weight between ' + cast(@Weight_value as nvarchar(7)) + ' and ' + cast(@Weight_value2 as nvarchar(7));
	else
		set @sql = @sql + ' and Weight ' + @Weight_operator + ' ' + cast(@Weight_value as nvarchar(7));
end
if @Gender_value <> ''
begin
	set @sql = @sql + ' and Gender = ''' + @Gender_value + '''';
end
if @Blood_value <> ''
begin
	set @sql = @sql + ' and Blood in (''' + @Blood_value + ''')';
end

set @sql_count = @sql_count + @sql;
set @sql_topN = @sql_topN + @sql;

--DataTables.js Server-side processing
if @Length > 0
begin
	set @sql_topN = @sql_topN + '
		order by ' + @OrderBy + ' ' + @OrderByAsc + '
		offset ' + cast(@Start as varchar) + ' rows
		fetch next ' + cast(@Length as varchar) + ' rows only';
end

print @sql_count;
print @sql_topN;
exec sp_executesql @sql_count;
exec sp_executesql @sql_topN;

GO